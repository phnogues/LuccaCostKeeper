using LuccaCostKeeper.Business.Interfaces;
using LuccaCostKeeper.Model.Dtos;
using LuccaCostKeeper.Model.Entities;
using LuccaCostKeeper.WebApi.Middlewares;
using LuccaCostKeeper.WebApi.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LuccaCostKeeper.WebApi.Controllers;

[ApiController]
[ApiKeyAuth]
[Route("expenses/v1")]
public class ExpenseController : ControllerBase
{
	private readonly ILogger<ExpenseController> _logger;
	private readonly IExpenseService _expenseService;

	public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService)
	{
		_logger = logger;
		_expenseService = expenseService;
	}

	/// <summary>
	/// List expenses by userId
	/// </summary>
	/// <param name="userId">The user id, as guid</param>
	/// <param name="sort" example="date,amount">Optional sort param</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Name = "{userId}")]
	[SwaggerOperation(Summary = "List expenses by userId", OperationId = "List")]
	[SwaggerResponse((int)HttpStatusCode.OK, "List of expenses by user", typeof(IEnumerable<ExpenseDto>))]
	[SwaggerResponse(400, "Request data is invalid")]
	[SwaggerResponse(401, "Unauthorized")]
	public async Task<IActionResult> List(
		[SwaggerParameter("UserId", Required = true)] Guid userId, 
		CancellationToken cancellationToken, 
		[FromQuery, SwaggerParameter("Sort by 'date' 'amount'", Required = false)] string sort = "date")
	{
		var result = await _expenseService.GetByUserId(userId, cancellationToken, sort);

		if (result.IsFailed)
			return BadRequest(result.Errors.First().Message);

		return Ok(result.Value);
	}

	[HttpPost(Name = "expense")]
	[SwaggerOperation(Summary = "Create an expense", OperationId = "Create")]
	[SwaggerResponse((int)HttpStatusCode.OK, "Creation status", typeof(string))]
	[SwaggerResponse(400, "Request data is invalid")]
	[SwaggerResponse(401, "Unauthorized")]
	public async Task<IActionResult> Create(ExpenseRequest expenseRequest, CancellationToken cancellationToken)
	{
		Expense expense = new Expense
		{
			UserId = expenseRequest.UserId,
			Amount = expenseRequest.Amount,
			Currency = expenseRequest.Currency,
			Comment = expenseRequest.Comment,
			Type = expenseRequest.Type,
			Date = expenseRequest.Date
		};

		var result = await _expenseService.Create(expense, cancellationToken);

		if (result.IsFailed)
			return BadRequest(result.Errors?.First().Message);

		return Ok(result.Successes?.First().Message);
	}
}
