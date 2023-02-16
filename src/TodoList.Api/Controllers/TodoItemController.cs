using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Models;
using TodoList.Application.Common.Models;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItems;
using TodoList.Domain.Entities;

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

    [HttpPut("{id:Guid}")]
    public async Task<ApiResponse<TodoItem>> Update(Guid id, [FromBody] UpdateTodoItemCommand command)
    {
        if (id != command.Id)
        {
            return ApiResponse<TodoItem>.Fail("Query id not match with body");
        }

        return ApiResponse<TodoItem>.Success(await _mediator.Send(command));
    }

    // 对于查询来说，一般参数是来自查询字符串的，所以这里用[FromQuery]
    [HttpGet]
    public async Task<ApiResponse<PaginatedList<TodoItemDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
    {
        return ApiResponse<PaginatedList<TodoItemDto>>.Success(await _mediator.Send(query));
    }
}