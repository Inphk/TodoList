using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("/todo-item")]
public class TodoItemController : ControllerBase
{
    private readonly IMediator _mediator;

    // 注入MediatR
    public TodoItemController(IMediator mediator) 
        => _mediator = mediator;

    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateTodoItemCommand command)
    {
        var createdTodoItem = await _mediator.Send(command);

        // 处于演示的目的，这里只返回创建出来的TodoItem的Id，理由同前
        return createdTodoItem;
    }
}