using Blazorise.Extensions;

using GameManager.Core.MediatR.DdosGaurdBypass.Commands;
using GameManager.Core.MediatR.Migrations.Commands;
using GameManager.Core.MediatR.Migrations.Queries;
using GameManager.Core.MediatR.Settings.Commands;
using GameManager.Core.MediatR.Settings.Queries;
using GameManager.UI.Features.Settings.Actions.Set;

using Microsoft.Extensions.Logging;

namespace GameManager.UI.Features.Settings.Actions.Init;

public class InitSettingsAction { }

internal class InitSettingsActionEffect : Effect<InitSettingsAction>
{
    private readonly IMediator _mediator;
    private readonly ILogger<InitSettingsActionEffect> _logger;


    public InitSettingsActionEffect(IMediator mediator, ILogger<InitSettingsActionEffect> logger, INotificationService notificationService)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task HandleAsync(InitSettingsAction action, IDispatcher dispatcher)
    {
        try
        {
            _logger.LogInformation("Initing Settings");
            var settings = await _mediator.Send(new GetSettingsQuery());

            try
            {
                var unappliedMigrations = await _mediator.Send(new GetUnappliedMigrationsQuery(settings));
                if ( unappliedMigrations.Any() )
                {
                    var appliedMigrations = await _mediator.Send(new ApplyMigrationsCommand(unappliedMigrations, settings));
                    settings.AppliedMigrations.AddRange(appliedMigrations);
                    await _mediator.Send(new SaveSettingsCommand { AppSettings = settings });
                }
            }
            catch ( Exception e )
            {
                dispatcher.Dispatch(new AddErrorNotificationAction("Error applying migrations", e, "Migrations Error"));
                _logger.LogError(e, "Error while applying migrations");
            }

            dispatcher.Dispatch(new SetAppSettingsAction(settings));

            try
            {
                if ( settings.AuthSettings.Username.IsNullOrEmpty() && settings.AuthSettings.Password.IsNullOrEmpty() )
                {
                    dispatcher.Dispatch(new AddWarningNotificationAction("Please enter your username and password in the settings page", "Login Missing"));
                    _logger.LogWarning("Username and password are empty");
                }
                else
                {
                    await _mediator.Send(new GetBypassCookiesCommand(settings.AuthSettings.Cookies));
                }
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex, "Error getting bypass cookies");
            }

            dispatcher.Dispatch(new F95AuthAction());
            dispatcher.Dispatch(new GetExistingGamesAction(settings.GameLibrarySettings.Folders ?? new List<GameLibraryFolder>()));
            dispatcher.Dispatch(new ScanAllDownloadFoldersAction());

            dispatcher.Dispatch(new InitSettingsSuccessAction());
        }
        catch ( FileNotFoundException )
        {
            _logger.LogInformation("Setting default settings");
            var settings = new AppSettings();

            //blank settings file we're going to assume its a new install and don't need to apply migrations
            var unappliedMigrations = await _mediator.Send(new GetUnappliedMigrationsQuery(settings));
            settings.AppliedMigrations.AddRange(unappliedMigrations);

            await _mediator.Send(new SaveSettingsCommand { AppSettings = settings });

            dispatcher.Dispatch(new SetAppSettingsAction(settings));

            dispatcher.Dispatch(new F95AuthAction());
            dispatcher.Dispatch(new GetExistingGamesAction(settings.GameLibrarySettings.Folders));
            dispatcher.Dispatch(new ScanAllDownloadFoldersAction());

            dispatcher.Dispatch(new InitSettingsSuccessAction());
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error getting initial settings"));
        }
    }
}

