
using LuccaCostKeeper.Model.Enums;

namespace LuccaCostKeeper.WebApi.RequestModels;

public class ExpenseRequest
{
	public Guid UserId { get; set; }

	public decimal Amount { get; set; }

	public CurrencyEnum Currency { get; set; }

	public string Comment { get; set; }

	public ExpenseTypeEnum Type { get; set; }

	public DateTime Date { get; set; }
}
