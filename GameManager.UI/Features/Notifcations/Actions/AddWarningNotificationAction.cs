using Microsoft.Extensions.Logging;

namespace GameManager.UI.Features.Notifcations.Actions;

public record AddWarningNotificationAction(string Message, string Title, double? DisplayInterval = null);

internal class AddWarningNotificationActionEffect : Effect<AddWarningNotificationAction>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<AddWarningNotificationActionEffect> _logger;

    public AddWarningNotificationActionEffect(INotificationService notificationService, ILogger<AddWarningNotificationActionEffect> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public override Task HandleAsync(AddWarningNotificationAction action, IDispatcher dispatcher)
    {
        return _notificationService.Error(action.Message, action.Title, _ => _.IntervalBeforeClose = action.DisplayInterval);
    }
}
