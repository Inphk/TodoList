using TodoList.Infrastructure.Log;
using TodoList.Infrastructure;
using TodoList.Application;
using TodoList.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region builder配置代码块
// 配置日志
builder.ConfigureLog();

#endregion

#region Services配置代码块
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 添加基础设施配置 (EFCore, Repository)
builder.Services.AddInfrastructure(builder.Configuration);

// 添加应用层配置 (MediatR, AutoMapper, Validators)
builder.Services.AddApplication();

#endregion

var app = builder.Build();

#region app配置代码块
// 配置全局异常处理中间件
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 启动程序时，对数据库进行迁移，正式环境需注释掉
app.MigrateDatabase();

#endregion

app.Run();

