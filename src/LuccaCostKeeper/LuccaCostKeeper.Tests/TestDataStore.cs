using LuccaCostKeeper.Model.Entities;
using LuccaCostKeeper.Model.Enums;

namespace LuccaCostKeeper.Business.Datas;

/// <summary>
/// Data store used for test data
/// </summary>
public class TestDataStore
{
	public static Guid USER_ID_1 = Guid.Parse("33a3a34a-a363-42e8-9259-589259e17490");
	public static Guid USER_ID_2 = Guid.Parse("dd07bb26-c898-4056-b83e-b5d9ae555562");
	public static Guid USER_ID_3 = Guid.Parse("361e0bf1-6bfa-4783-82d1-c96da04d22a5");

	public static List<User> GetUsers()
	{
		return new List<User>()
		{
			new User
			{
				Id = USER_ID_1,
				Currency = CurrencyEnum.EUR,
				FirstName = "John",
				LastName = "Doe"
			},
			new User
			{
				Id = USER_ID_2,
				Currency = CurrencyEnum.USD,
				FirstName = "Anthony",
				LastName = "Stark"
			},
			new User
			{
				Id = USER_ID_3,
				Currency = CurrencyEnum.RUB,
				FirstName = "Natasha",
				LastName = "Romanova"
			}
		};
	}

	public static List<Expense> GetExpenses()
	{
		var users = GetUsers();

		return new List<Expense>()
		{
			new Expense
			{
				Id = Guid.NewGuid(),
				Amount = 100,
				Comment = "Test",
				Currency = CurrencyEnum.EUR,
				Date = DateTime.UtcNow.AddDays(-1),
				Type = ExpenseTypeEnum.Restaurant,
				UserId =users[2].Id,
				User = users[2],
			},
			new Expense
			{
				Id = Guid.NewGuid(),
				Amount = 100,
				Comment = "Test",
				Currency = CurrencyEnum.EUR,
				Date = DateTime.UtcNow.AddDays(-12),
				Type = ExpenseTypeEnum.Restaurant,
				UserId =users[1].Id,
				User = users[1],
			},
			new Expense
			{
				Id = Guid.NewGuid(),
				Amount = 100,
				Comment = "Test",
				Currency = CurrencyEnum.EUR,
				Date = DateTime.UtcNow.AddDays(-24),
				Type = ExpenseTypeEnum.Restaurant,
				UserId =users[0].Id,
				User = users[0],
			}
		};
	}

	public static Expense GetDefaultExpense()
	{
		return new Expense()
		{
			Amount = 120,
			Comment = "Restaurant with John",
			Currency = CurrencyEnum.USD,
			Date = DateTime.UtcNow.AddDays(-1),
			Type = ExpenseTypeEnum.Restaurant,
			UserId = USER_ID_2,
			User = GetUsers().First(u => u.Id == USER_ID_2)
		};
	}
}
