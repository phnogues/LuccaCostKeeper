using LuccaCostKeeper.Model.Enums;

namespace LuccaCostKeeper.Model.Entities;

public class User : BaseEntity
{
	public required string FirstName { get; set; }

	public required string LastName { get; set; }

	public required CurrencyEnum Currency { get; set; }
}
