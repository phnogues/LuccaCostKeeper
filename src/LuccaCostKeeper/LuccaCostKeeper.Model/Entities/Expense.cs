using LuccaCostKeeper.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuccaCostKeeper.Model.Entities;

public class Expense : BaseEntity
{
	public virtual User User { get; set; }

	[ForeignKey(nameof(User))]
	public required Guid UserId { get; set; }

	public required DateTime Date { get; set; }

	public required ExpenseTypeEnum Type { get; set; }

	public required decimal Amount { get; set; }

	public required CurrencyEnum Currency { get; set; }

	public required string Comment { get; set; }
}
