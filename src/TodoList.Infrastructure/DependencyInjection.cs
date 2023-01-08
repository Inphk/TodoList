using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Common.Interfaces;
using TodoList.Infrastructure.Persistence;
using TodoList.Infrastructure.Persistence.Repositories;
using TodoList.Infrastructure.Services;

namespace TodoList.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoListDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection"),
                b => b.MigrationsAssembly(typeof(TodoListDbContext).Assembly.FullName)));

        // 将TodoListDbContext注入到IApplicationDbContext
        // services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<TodoListDbContext>());
        services.AddScoped<IDomainEventService, DomainEventService>();
        
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        return services;
    }
}