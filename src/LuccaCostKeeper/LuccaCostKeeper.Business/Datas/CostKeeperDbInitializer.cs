using LuccaCostKeeper.Model.Entities;
using LuccaCostKeeper.Model.Enums;

namespace LuccaCostKeeper.Business.Datas;

public class CostKeeperDbInitializer
{
	private readonly CostKeeperDbContext _dbContext;

	public CostKeeperDbInitializer(CostKeeperDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Seeds default data to the database if it is empty
	/// </summary>
	/// <returns></returns>
	public async Task SeedAsync()
	{
		if (_dbContext.Users.ToList().Count() == 0)
		{
			_dbContext.Users.Add(new User
			{
				FirstName = "Anthony",
				LastName = "Stark",
				Currency = CurrencyEnum.USD,
			});

			_dbContext.Users.Add(new User
			{
				FirstName = "Natasha",
				LastName = "Romanova",
				Currency = CurrencyEnum.RUB,
			});

			await _dbContext.SaveChangesAsync();
		}
	}
}
