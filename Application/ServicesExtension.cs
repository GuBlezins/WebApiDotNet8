using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Application;

public static class ServicesExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
