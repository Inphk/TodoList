using TodoList.Infrastructure.Log;
using TodoList.Infrastructure;
using TodoList.Application;

var builder = WebApplication.CreateBuilder(args);


#region builder配置代码块
// 配置日志
builder.ConfigureLog();

#endregion

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services配置代码块
// 添加基础设施配置 (EFCore, Repository)
builder.Services.AddInfrastructure(builder.Configuration);

// 添加应用层配置 (MediatR, AutoMapper)
builder.Services.AddApplication();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region app配置代码块
// 启动程序时，对数据库进行迁移，正式环境需注释掉
app.MigrateDatabase();

#endregion

app.Run();
