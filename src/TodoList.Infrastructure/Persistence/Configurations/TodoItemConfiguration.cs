using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        // 领域事件字段不用写入数据库
        builder.Ignore(e => e.DomainEvents);

        // 设置实体属性的检验规则
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
    }
}