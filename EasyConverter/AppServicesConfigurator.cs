using Core.Interfaces;
using Infrastructure.Services;

namespace EasyConverter;

public static class AppServicesConfigurator
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IConverterService, ConverterService>();
            
        return serviceCollection;
    }
}
