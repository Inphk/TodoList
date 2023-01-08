using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(assembly);

        services.AddAutoMapper(assembly);

        return services;
    }
}