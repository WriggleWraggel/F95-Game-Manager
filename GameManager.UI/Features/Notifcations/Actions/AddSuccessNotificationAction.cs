using Microsoft.Extensions.Logging;

namespace GameManager.UI.Features.Notifcations.Actions;

public record AddSuccessNotificationAction(string Message, string Title);

internal class AddSuccessNotificationActionEffect : Effect<AddSuccessNotificationAction>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<AddSuccessNotificationActionEffect> _logger;

    public AddSuccessNotificationActionEffect(INotificationService notificationService, ILogger<AddSuccessNotificationActionEffect> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public override Task HandleAsync(AddSuccessNotificationAction action, IDispatcher dispatcher)
    {
        _logger.LogInformation($"{action.Title}{Environment.NewLine}{action.Message}");
        return _notificationService.Success(action.Message, action.Title);
    }
}
