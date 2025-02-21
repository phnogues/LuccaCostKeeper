using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using LuccaCostKeeper.Business.Datas;
using LuccaCostKeeper.Business.Services;
using LuccaCostKeeper.Model;
using LuccaCostKeeper.Model.Entities;
using LuccaCostKeeper.Model.Enums;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace LuccaCostKeeper.Tests;

public class Tests
{
	Mock<CostKeeperDbContext> _dbContext;
	ILogger<ExpenseService> _logger;

	public Tests()
	{
		var expensesMock = TestDataStore.GetExpenses();
		var usersMock = TestDataStore.GetUsers();
		var expensesMockDbSet = expensesMock.BuildMock().BuildMockDbSet();
		var usersMockDbSet = usersMock.BuildMock().BuildMockDbSet();

		_dbContext = new Mock<CostKeeperDbContext>();
		_dbContext.Setup(x => x.Expenses).Returns(expensesMockDbSet.Object);
		_dbContext.Setup(x => x.Users).Returns(usersMockDbSet.Object);

		_logger = Mock.Of<ILogger<ExpenseService>>();

	}

	[Fact]
	public async Task GetExpensesByUserId_Should_ReturnsExpenses()
	{
		// Arrange
		Guid userId = TestDataStore.USER_ID_3;

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.GetByUserId(userId, CancellationToken.None);

		// Assert
		Assert.True(result.IsSuccess && result.Value.Count > 0);
	}

	[Fact]
	public async Task CreateExpense_CannotHaveDateInFuture_ReturnsError()
	{
		// Arrange
		Expense expense = TestDataStore.GetDefaultExpense();
		expense.Date = DateTime.UtcNow.AddDays(1);
		
		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeFailure().And.HaveError(ErrorMessages.DATE_INVALID_FUTUR); ;
	}

	[Fact]
	public async Task CreateExpense_CannotBeDatedMore3Months_ReturnsError()
	{
		// Arrange
		Expense expense = TestDataStore.GetDefaultExpense();
		expense.Date = DateTime.UtcNow.AddMonths(-3);

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeFailure().And.HaveError(ErrorMessages.DATE_INVALID_3MONTHS); 
	}

	[Fact]
	public async Task CreateExpense_WithoutComment_ReturnsError()
	{
		// Arrange
		Expense expense = TestDataStore.GetDefaultExpense();
		expense.Comment = string.Empty;

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeFailure().And.HaveError(ErrorMessages.COMMENT_EMPTY);
	}

	[Fact]
	public async Task CreateExpense_SameTwice_ReturnsError()
	{
		// Arrange
		Expense expense = TestDataStore.GetExpenses().First();

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeFailure().And.HaveError(ErrorMessages.EXPENSE_EXISTS);
	}

	[Fact]
	public async Task CreateExpense_ExpenseCurrencyMustMatchUserCurrency_ReturnsError()
	{
		// Arrange
		Expense expense = TestDataStore.GetDefaultExpense();
		expense.Currency = CurrencyEnum.USD;

		var randomUser = TestDataStore.GetUsers().First(u => u.Currency != expense.Currency);
		expense.User = randomUser;
		expense.UserId = randomUser.Id;

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeFailure().And.HaveError(ErrorMessages.CURRENCY_MISMATCH);
	}

	[Fact]
	public async Task CreateExpense_ReturnsSuccess()
	{
		// Arrange
		Expense expense = TestDataStore.GetDefaultExpense();

		// Act
		ExpenseService expenseService = new(_dbContext.Object, _logger);
		var result = await expenseService.Create(expense, CancellationToken.None);

		// Assert
		result.Should().BeSuccess();
	}
}