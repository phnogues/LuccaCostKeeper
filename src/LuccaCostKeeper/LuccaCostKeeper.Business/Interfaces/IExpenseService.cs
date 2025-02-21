using FluentResults;
using LuccaCostKeeper.Model.Dtos;
using LuccaCostKeeper.Model.Entities;

namespace LuccaCostKeeper.Business.Interfaces;

public interface IExpenseService
{
	Task<Result> Create(Expense expense, CancellationToken cancellationToken);

	Task<Result<List<ExpenseDto>>> GetByUserId(Guid userId, CancellationToken cancellationToken, string sortBy);
}
