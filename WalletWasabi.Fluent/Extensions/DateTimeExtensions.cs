using WalletWasabi.Lang;

namespace WalletWasabi.Fluent.Extensions;

public static class DateTimeExtensions
{
	public static string ToUserFacingString(this DateTime value, bool withTime = true)
	{
		return value.ToString(withTime ? "HH:mm on MMMM d, yyyy" : "MMMM d, yyyy");
	}

	public static string ToUserFacingFriendlyString(this DateTime value)
	{
		if (value.Date == DateTime.Today)
		{
			return Resources.Today;
		}

		if (value.Date == DateTime.Today.AddDays(-1))
		{
			return Resources.Yesterday;
		}

		return value.ToString("MMM d, yyyy");
	}
}
