using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Base;
using TodoList.Domain.Base.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Persistence;

public class TodoListDbContext : DbContext
{
    private readonly IDomainEventService _domainEventService;

    public TodoListDbContext(
        DbContextOptions<TodoListDbContext> options,
        IDomainEventService domainEventService) : base(options)
    {
        _domainEventService = domainEventService;
    }

    public DbSet<Domain.Entities.TodoList> TodoLists => Set<Domain.Entities.TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // 在我们重写的SaveChangesAsync方法中，去设置审计相关的字段，目前对于修改人这个字段暂时先给个定值，等到后面讲到认证鉴权的时候再回过头来看这里
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "Anonymous";
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = "Anonymous";
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }

        // 在写数据库的时候同时发送领域事件，这里要注意一定要保证写入数据库成功后再发送领域事件，否则会导致领域对象状态的不一致问题。
        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 将当前Assembly中定义的实体类配置（继承IEntityTypeConfiguration的TodoListConfiguration,TodoItemConfiguration）添加到EF中，类似于如下效果
        // builder.ApplyConfiguration<Domain.Entities.TodoList>(new TodoListConfiguration());
        // builder.ApplyConfiguration<TodoItem>(new TodoItemConfiguration());
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}