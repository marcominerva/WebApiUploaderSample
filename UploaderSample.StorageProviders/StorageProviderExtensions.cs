using System;
using Microsoft.Extensions.DependencyInjection;

namespace UploaderSample.StorageProviders
{
    public static class StorageProviderExtensions
    {
        public static IServiceCollection AddFileSystemStorageProvider(this IServiceCollection services, Action<FileSystemStorageSettings> configuration)
        {
            var settings = new FileSystemStorageSettings();
            configuration.Invoke(settings);
            services.AddSingleton(settings);

            services.AddScoped<IStorageProvider, FileSystemStorageProvider>();
            return services;
        }

        public static IServiceCollection AddAzureStorageProvider(this IServiceCollection services, Action<AzureStorageSettings> configuration)
        {
            var settings = new AzureStorageSettings();
            configuration.Invoke(settings);
            services.AddSingleton(settings);

            services.AddScoped<IStorageProvider, AzureStorageProvider>();
            return services;
        }
    }
}
