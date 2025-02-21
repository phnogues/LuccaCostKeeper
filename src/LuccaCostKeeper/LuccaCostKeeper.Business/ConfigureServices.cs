using LuccaCostKeeper.Business.Datas;
using LuccaCostKeeper.Business.Interfaces;
using LuccaCostKeeper.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuccaCostKeeper.Business;

public static class ConfigureServices
{
	public static IServiceCollection AddBusiness(this IServiceCollection services, IConfigurationManager configuration)
	{
		services.AddDbContext<CostKeeperDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("CostKeeperDb"));
		});

		services.AddScoped<IExpenseService, ExpenseService>();
		services.AddScoped<CostKeeperDbInitializer>();

		return services;
	}
}
