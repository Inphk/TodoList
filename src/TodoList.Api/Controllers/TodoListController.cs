using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.TodoLists.Commands.CreateTodoList;
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
    public async Task<IActionResult> Create([FromBody] CreateTodoListCommand command)
    {
        var createdTodoList = await _mediator.Send(command);

        return CreatedAtRoute("TodListById", new { id = createdTodoList.Id }, createdTodoList);
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
}