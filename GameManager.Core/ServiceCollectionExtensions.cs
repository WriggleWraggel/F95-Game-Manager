using System.Reflection;

using Flurl.Http.Newtonsoft;

using GameManager.Core.Migrations;
using GameManager.Core.Services;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameManager.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, ISettingsPathProvider provider)
    {

        //typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly
        var serializerWrapper = new NewtonSoftJsonSerializerWrapper();
        services.AddSingleton(new HttpSessionWrapper());
        services.AddSingleton(serializerWrapper);
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly)
        );
        services.AddSingleton(provider);
        services.AddScoped<IFileRepo, FileRepo>();
        services.AddScoped<ILocalGameRepo, LocalGameJsonFileRepo>();
        services.AddScoped<IF95Client, F95Client>();

        FlurlHttp.ConfigureClientForUrl(HttpConsts.F95Root)
            .Settings.JsonSerializer = new NewtonsoftJsonSerializer(serializerWrapper.Settings);

        //get all types that implement the IGameManagerMigration interface
        var migrations = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IGameManagerMigration)))
            .ToList();

        //project each migration into a ServiceDescriptor
        var migrationDescriptors = migrations.Select(_ =>
            new ServiceDescriptor(typeof(IGameManagerMigration), _, ServiceLifetime.Singleton)
        );

        services.TryAddEnumerable(migrationDescriptors);

        return services;
    }
}
