namespace LuccaCostKeeper.Model;

public class ErrorMessages
{
	public const string EXPENSE_EXISTS = "Expense already exists";

	public const string COMMENT_EMPTY = "Comment cannot be empty";

	public const string AMOUNT_INVALID = "Amount must be greater than 0";

	public const string DATE_INVALID_FUTUR = "Date cannot be in the future";

	public const string DATE_INVALID_3MONTHS = "Date cannot be older than 3 months";

	public const string USER_NOT_FOUND = "User not found";

	public const string CURRENCY_MISMATCH = "User currency and expense currency do not match";

	public const string TYPE_NOT_DEFINED = "Type must be defined";

	public const string CURRENCY_NOT_DEFINED = "Currency must be defined";
}
