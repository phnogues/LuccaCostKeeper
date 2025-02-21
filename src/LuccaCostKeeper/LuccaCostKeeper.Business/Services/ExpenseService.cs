using FluentResults;
using LuccaCostKeeper.Business.Datas;
using LuccaCostKeeper.Business.Interfaces;
using LuccaCostKeeper.Model;
using LuccaCostKeeper.Model.Dtos;
using LuccaCostKeeper.Model.Entities;
using LuccaCostKeeper.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LuccaCostKeeper.Business.Services;

public class ExpenseService : IExpenseService
{
	private readonly CostKeeperDbContext _dbContext;
	private readonly ILogger<ExpenseService> _logger;

	public ExpenseService(CostKeeperDbContext dbContext, ILogger<ExpenseService> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	/// <summary>
	/// Create an expense
	/// </summary>
	/// <param name="expense"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A message including the new expense ID</returns>
	public async Task<Result> Create(Expense expense, CancellationToken cancellationToken)
	{
		try
		{
			// Validate expense
			var validationResult = await IsValid(expense);
			if (validationResult.IsFailed)
				return validationResult;

			// Create expense in database
			await _dbContext.Expenses.AddAsync(expense, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Result.Ok().WithSuccess($"Expense {expense.Id} was successfully created");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occured while creating expense");
			return Result.Fail(new Error("An error occured while creating expense").CausedBy(ex));
		}
	}

	/// <summary>
	/// Get expenses by user id
	/// </summary>
	/// <param name="userId">The user guid id</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of expenses</returns>
	public async Task<Result<List<ExpenseDto>>> GetByUserId(Guid userId, CancellationToken cancellationToken, string sortBy = "date")
	{
		try
		{
			var query = _dbContext.Expenses
				.Where(e => e.UserId == userId)
				.AsNoTracking();

			switch (sortBy)
			{
				case "date":
					query = query.OrderBy(e => e.Date);
					break;
				case "amount":
					query = query.OrderBy(e => e.Amount);
					break;
			}

			var expenses = await query
				.Select(e => new ExpenseDto
				{
					Id = e.Id,
					Date = e.Date,
					Type = e.Type,
					Amount = e.Amount,
					Currency = e.Currency,
					Comment = e.Comment,
					FirstName = e.User.FirstName,
					LastName = e.User.LastName
				})
				.ToListAsync(cancellationToken);

			return Result.Ok(expenses);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occured while getting expenses");
			return Result.Fail(new Error("An error occured while getting expenses").CausedBy(ex));
		}
	}

	/// <summary>
	/// Check if expense is valid
	///  - An expense cannot have a date in the future,
	///  - An expense cannot be dated more than 3 months ago,
	///  - The comment is mandatory,
	///  - A user cannot declare the same expense twice(same date and amount),
	///  - The currency of the expense must match the user’s currency.
	/// </summary>
	/// <param name="expense"></param>
	/// <returns></returns>
	public async Task<Result> IsValid(Expense expense)
	{
		// The type of the expense must be defined
		if (expense.Type == ExpenseTypeEnum.NotDefined)
			return Result.Fail(ErrorMessages.TYPE_NOT_DEFINED);

		// The currency of the expense must be defined
		if (expense.Currency == CurrencyEnum.NotDefined)
			return Result.Fail(ErrorMessages.CURRENCY_NOT_DEFINED);

		// An expense cannot have an amount less than or equal to 0
		if (expense.Amount <= 0)
			return Result.Fail(ErrorMessages.AMOUNT_INVALID);

		// An expense cannot have a date in the future
		if (expense.Date > DateTime.UtcNow)
			return Result.Fail(ErrorMessages.DATE_INVALID_FUTUR);

		// An expense cannot be dated more than 3 months ago
		if (expense.Date < DateTime.UtcNow.AddMonths(-3))
			return Result.Fail(ErrorMessages.DATE_INVALID_3MONTHS);

		// The comment is mandatory,
		if (string.IsNullOrEmpty(expense.Comment))
			return Result.Fail(ErrorMessages.COMMENT_EMPTY);

		// Check if expense was already created
		// A user cannot declare the same expense twice(same date and amount)
		var existingExpense = await _dbContext.Expenses.AnyAsync(e => e.UserId == expense.UserId && e.Date.Date == expense.Date.Date && e.Amount == expense.Amount);
		if (existingExpense)
			return Result.Fail(ErrorMessages.EXPENSE_EXISTS);

		// Associated user
		var user = await _dbContext.Users
								.AsNoTracking()
								.Select(u => new { u.Id, u.Currency })
								.FirstOrDefaultAsync(u => u.Id == expense.UserId);
		if (user == null)
			return Result.Fail(ErrorMessages.USER_NOT_FOUND);

		// The currency of the expense must match the user’s currency
		if (user.Currency != expense.Currency)
			return Result.Fail(ErrorMessages.CURRENCY_MISMATCH);

		return Result.Ok();
	}
}
