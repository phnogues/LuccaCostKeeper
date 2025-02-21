using LuccaCostKeeper.Model.Enums;
using System.Text.Json.Serialization;

namespace LuccaCostKeeper.Model.Dtos;

public class ExpenseDto
{
	public Guid Id { get; set; }

	[JsonIgnore]
	public string FirstName { get; set; }

	[JsonIgnore]
	public string LastName { get; set; }

	public string User => $"{FirstName} {LastName}";

	public required DateTime Date { get; set; }

	public required ExpenseTypeEnum Type { get; set; }

	public required decimal Amount { get; set; }

	public required CurrencyEnum Currency { get; set; }

	public required string Comment { get; set; }
}
