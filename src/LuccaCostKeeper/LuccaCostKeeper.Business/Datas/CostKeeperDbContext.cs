using LuccaCostKeeper.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuccaCostKeeper.Business.Datas;

public class CostKeeperDbContext : DbContext
{
	/// <summary>
	/// Parameterless constructor for unit tests
	/// </summary>
	public CostKeeperDbContext()
    {
        
    }

    public CostKeeperDbContext(DbContextOptions<CostKeeperDbContext> options)
	   : base(options)
	{
	}

	/// <summary>
	/// Expenses table
	/// </summary>
	public virtual DbSet<Expense> Expenses => Set<Expense>();

	/// <summary>
	/// Users table
	/// </summary>
	public virtual DbSet<User> Users => Set<User>();
}