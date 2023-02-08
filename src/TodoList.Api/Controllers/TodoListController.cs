using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Filters;
using TodoList.Api.Models;
using TodoList.Application.TodoLists.Commands.CreateTodoList;
using TodoList.Application.TodoLists.Commands.DeleteTodoList;
using TodoList.Application.TodoLists.Queries.GetSingleTodo;
using TodoList.Application.TodoLists.Queries.GetTodos;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("/todo-list")]
public class TodoListController : ControllerBase
{
    private readonly IMediator _mediator;

    // 注入MediatR
    public TodoListController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    [ServiceFilter(typeof(LogFilterAttribute))]
    public async Task<ApiResponse<Domain.Entities.TodoList>> Create([FromBody] CreateTodoListCommand command)
    {
        return ApiResponse<Domain.Entities.TodoList>.Success(await _mediator.Send(command));
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoListBriefDto>>> Get()
    {
        return await _mediator.Send(new GetTodosQuery());
    }

    [HttpGet("{id:Guid}", Name = "TodListById")]
    public async Task<ActionResult<TodoListDto>> GetSingleTodoList(Guid id)
    {
        return await _mediator.Send(new GetSingleTodoQuery
        {
            ListId = id
        }) ?? throw new InvalidOperationException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        return ApiResponse<object>.Success(await _mediator.Send(new DeleteTodoListCommand { Id = id }));
    }
}