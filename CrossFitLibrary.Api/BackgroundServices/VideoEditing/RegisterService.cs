using System;
using CrossFitLibrary.Api;
using CrossFitLibrary.Api.BackgroundServices.VideoEditing;
using CrossFitLibrary.Api.Settings;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class RegisterService
{
    public static IServiceCollection AddFileManager(this IServiceCollection services, IConfiguration config)
    {

        // Pulling the settings from the json (appsettings.Developement.json)
        var settingsSection = config.GetSection(nameof(FileSettings)); // we use the nameof as a trick to force type Checking
        
         // Next we cast it to FileSettings type 
         //(This type is defined by the FileSettings.cs in the settings directory)
        var settings = settingsSection.Get<FileSettings>();
                                                            
                                                            
        // You need this to make it an injectable option object
        services.Configure<FileSettings>(settingsSection);

        if (settings.Provider.Equals(CrossFitLibraryConstants.Files.Providers.Local, StringComparison.InvariantCultureIgnoreCase))
        {
            services.AddSingleton<IFileManager, FileManagerLocal>();
        }
        else if(settings.Provider.Equals(CrossFitLibraryConstants.Files.Providers.S3, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NotImplementedException();
        }
        else
        {
            throw new Exception($"Invalid File Manager Provider: {settings.Provider}");
        }
        
        return services;
    }
}