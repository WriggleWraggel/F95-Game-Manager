
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

using GameManager.UI.Features.Settings;

using Microsoft.Extensions.DependencyInjection;

namespace GameManager.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        services
            .AddBlazorise(options =>
            {
                //needed to fix odd issue with textboxes always jumping to the end
                options.Debounce = true;
                options.DebounceInterval = 100;
            })
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();

        services
            .AddFluxor(o => o
                .ScanAssemblies(typeof(SettingsState).Assembly)
            //This is currently causing it to die, but is also uneeded as it doesn't work in maui
            //.UseReduxDevTools()
            );
        return services;
    }
}
