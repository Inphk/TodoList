using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoList.Infrastructure.Persistence.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<Domain.Entities.TodoList>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.TodoList> builder)
    {
        // 忽略领域事件字段
        builder.Ignore(t => t.DomainEvents);

        // 设置实体属性的检验规则
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        // 设置实体类之间的所有关系
        builder.OwnsOne(b => b.Colour);
    }
}