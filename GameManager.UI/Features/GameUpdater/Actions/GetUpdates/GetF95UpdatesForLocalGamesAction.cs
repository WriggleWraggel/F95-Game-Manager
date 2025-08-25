using GameManager.Core.Data;
using GameManager.Core.Data.F95;
using GameManager.Core.MediatR.F95.Queries;
using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.Core.Services;
using GameManager.UI.Features.GameUpdater.Actions.UpdateProgress;

namespace GameManager.UI.Features.GameUpdater.Actions.GetUpdates;

public record GetF95UpdatesForLocalGamesAction();

internal class GetF95UpdatesForLocalGamesActionEffect : Effect<GetF95UpdatesForLocalGamesAction>
{
    private readonly IMediator _mediator;
    private readonly IState<GameLibraryState> _gameLibraryState;
    private readonly int _maxConcurrentUpdates;
    private List<LocalGame> LocalGamesToCheck => _gameLibraryState.Value.Games.Where(_ => _.F95Game != null).ToList();

    public GetF95UpdatesForLocalGamesActionEffect(IMediator mediator, IState<GameLibraryState> GameLibraryState)
    {
        _mediator = mediator;
        _gameLibraryState = GameLibraryState;
        _maxConcurrentUpdates = 2;
    }

    public override async Task HandleAsync(GetF95UpdatesForLocalGamesAction action, IDispatcher dispatcher)
    {
        var gamesToUpdate = new List<LocalGame>(LocalGamesToCheck);

        try
        {
            while ( gamesToUpdate.Any() )
            {
                var gameBatchToUpdate = gamesToUpdate.Take(_maxConcurrentUpdates).ToList();
                var updateTasks = gameBatchToUpdate
                    .Select(_ => _mediator.Send(new GetF95PageSummaryDataQuery(_.F95Game!))).ToArray();

                try
                {
                    await Task.WhenAll(updateTasks);
                }
                catch
                {
                    //squash any errors that happen, we'll handle them after the fact
                }

                var successfulTaskResults = updateTasks
                    .Where(_ => _.IsCompletedSuccessfully)
                    .Select(_ => _.Result)
                    .ToList();

                await HandleSuccessfulUpdateTasks(successfulTaskResults);

                var failedTasks = updateTasks.Where(_ => _.IsFaulted).ToList();
                HandleErroredUpdateTasks(dispatcher, failedTasks);
                //if any tasks failed with a 429 error we need to exit the loop
                if ( failedTasks.Any(_ => _.Exception!.InnerException?.Message.Contains("429") ?? false) )
                {
                    throw new Exception("429 error, too many requests exiting update loop");
                }

                gamesToUpdate.RemoveRange(0, gameBatchToUpdate.Count());
                dispatcher.Dispatch(new IncreaseUpdateProgresAction(successfulTaskResults.Count));
            }

            dispatcher.Dispatch(new GetF95UpdatesForLocalGamesSuccessAction());
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new GetF95UpdatesForLocalGamesFailureAction(ex));
        }
    }

    private async Task HandleSuccessfulUpdateTasks(List<F95SummaryData> successfulTaskResults)
    {
        foreach ( var summaryData in successfulTaskResults )
        {
            var linkedGame = _gameLibraryState.Value.Games.Single(_ => _.F95Game?.Id == summaryData!.Id);
            if ( !NewF95DataAvailable(linkedGame.F95Game!, summaryData) )
            {
                continue;
            }

            linkedGame.F95Game = UpdateF95GameWithSummaryData(linkedGame.F95Game!, summaryData);

            linkedGame.UpdateAvailable = true;

            await _mediator.Send(new SaveLocalGameCommand(linkedGame));
        }
    }

    private static void HandleErroredUpdateTasks(IDispatcher dispatcher, List<Task<F95SummaryData>> failedTasks)
    {
        foreach ( var task in failedTasks )
        {
            switch ( task.Exception!.InnerException )
            {
                case F95GamePageException ex:
                    dispatcher.Dispatch(
                        new AddErrorNotificationAction(
                            $"{ex.Message}",
                            ex,
                            $"Error updating {ex.ErroredGame.Title}"
                        )
                    );
                    break;
                default:
                    dispatcher.Dispatch(
                        new AddErrorNotificationAction(
                            task.Exception!.ToString(),
                            task.Exception,
                            "Unexpected error getting updates"
                        )
                    );
                    break;
            }
        }
    }

    private bool NewF95DataAvailable(F95Game f95Game, F95SummaryData summaryData)
    {
        var datesMatch = f95Game.ThreadLastUpdatedDate == summaryData.UpdateDate;
        var versionsMatch = f95Game.Version == summaryData.Version;
        return !versionsMatch || !datesMatch;
    }

    private F95Game UpdateF95GameWithSummaryData(F95Game game, F95SummaryData f95SummaryData)
    {
        var updatedF95Game = new F95Game(game)
        {
            ThreadLastUpdatedDate = f95SummaryData.UpdateDate,
            Version = f95SummaryData.Version ?? ""
        };

        return updatedF95Game;
    }
}

internal class GetF95UpdatesForLocalGamesActionReducer : Reducer<GameUpdaterState, GetF95UpdatesForLocalGamesAction>
{
    public override GameUpdaterState Reduce(GameUpdaterState state, GetF95UpdatesForLocalGamesAction action) =>
        state with
        {
            SearchingForNewData = true,
            GamesCheckedProgress = 0
        };
}

