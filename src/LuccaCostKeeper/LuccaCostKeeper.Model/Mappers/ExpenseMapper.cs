using LuccaCostKeeper.Model.Dtos;
using LuccaCostKeeper.Model.Entities;

namespace LuccaCostKeeper.Model.Mappers;

public static class ExpenseMapper
{
	public static List<ExpenseDto> MapToDtos(this List<Expense> expenses)
	{
		return expenses.Select(MapToDto).ToList();
	}

	public static ExpenseDto MapToDto(this Expense expense)
	{
		return new ExpenseDto
		{
			Id = expense.Id,
			Amount = expense.Amount,
			Currency = expense.Currency,
			Comment = expense.Comment,
			FirstName = expense.User.FirstName,
			LastName = expense.User.LastName,
			Date = expense.Date,
			Type = expense.Type
		};
	}
}
