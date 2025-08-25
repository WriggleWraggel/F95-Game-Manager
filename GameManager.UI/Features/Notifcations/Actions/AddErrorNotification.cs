using Microsoft.Extensions.Logging;

namespace GameManager.UI.Features.Notifcations.Actions;

public record AddErrorNotificationAction(string Error, Exception ex, string Title);

internal class AddErrorNotificationActionEffect : Effect<AddErrorNotificationAction>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<AddErrorNotificationActionEffect> _logger;

    public AddErrorNotificationActionEffect(INotificationService notificationService, ILogger<AddErrorNotificationActionEffect> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public override Task HandleAsync(AddErrorNotificationAction action, IDispatcher dispatcher)
    {
        _logger.LogError(action.ex, $"{action.Title}{Environment.NewLine}{action.Error}");
        return _notificationService.Error(action.Error, action.Title, _ => _.IntervalBeforeClose = 30000);
    }
}
