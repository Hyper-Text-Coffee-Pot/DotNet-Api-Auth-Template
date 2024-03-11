namespace Bookmarx.Shared.v1.Membership.Entities;

public class Subscription
{
	public Subscription(
		int memberAccountID,
		DateTime createdDateTime,
		string subscriptionName,
		string providerSubscriptionID)
	{
		this.MemberAccountID = memberAccountID;
		this.SetStartAndEndDate(createdDateTime);
		this.SubscriptionTypeID = this.ConvertSubscriptionNameToType(subscriptionName);
		this.ProviderSubscriptionID = providerSubscriptionID;
	}

	private Subscription()
	{
		// Needed for EF
	}

	public DateTime ActivatedDateTimeUTC { get; private set; }

	public DateTime? CanceledDateTimeUTC { get; private set; }

	public DateTime ExpirationDateTimeUTC { get; private set; }

	public int MemberAccountID { get; private set; }

	/// <summary>
	/// The subscription ID associated with this checkout.session.completed event.
	/// Looks like sub_lkl2j34lk23j43lj and helps to associate internal subscription
	/// records with Stripes records.
	/// </summary>
	public string ProviderSubscriptionID { get; private set; }

	public int SubscriptionDaysRemaining
	{
		get
		{
			TimeSpan subscriptionTimeSpan = this.ExpirationDateTimeUTC - DateTime.UtcNow;

			/// Total days minus 2 <see cref="SetStartAndEndDate"/> where we add a 2 day buffer to be safe.
			return subscriptionTimeSpan.Days - 2;
		}
	}

	public int SubscriptionID { get; private set; }

	public SubscriptionType SubscriptionTypeID { get; private set; }

	/// <summary>
	/// Updates an existing subscription to increase the expiration date.
	/// This is some stupid nonsense with the newest version of PostgreSQL. We need to get crazy in creating a UTC timestamp.
	/// </summary>
	/// <returns></returns>
	public void UpdateSubscriptionEndDate(DateTime originalRenewalDateTime)
	{
		// Setting timestamp portion to midnight, we give a 2 day buffer anyhow so this should be fine.
		DateTime originalRenewalDate = new DateTime(originalRenewalDateTime.Year, originalRenewalDateTime.Month, originalRenewalDateTime.Day, 0, 0, 0);
		DateTime convertedToUTC = DateTime.SpecifyKind(originalRenewalDate, DateTimeKind.Utc).AddDays(32);
		DateTimeOffset convertedRenewalStartDate = new DateTimeOffset(convertedToUTC);

		// Adding 2 days buffer so subscriptions can properly resolve.
		this.ExpirationDateTimeUTC = convertedRenewalStartDate.UtcDateTime;
	}

	/// <summary>
	/// Sets the subscription type based on the name of the plan that comes back from the webhook request.
	/// </summary>
	/// <param name="subscriptionName"></param>
	private SubscriptionType ConvertSubscriptionNameToType(string subscriptionName)
	{
		SubscriptionType subscriptionType = SubscriptionType.None;

		// TODO: Someday make this not so terrible.
		if (subscriptionName.Contains("25 GB"))
		{
			subscriptionType = SubscriptionType.TwentyFiveGBPlan;
		}
		else if (subscriptionName.Contains("50 GB"))
		{
			subscriptionType = SubscriptionType.FiftyGBPlan;
		}
		else if (subscriptionName.Contains("100 GB"))
		{
			subscriptionType = SubscriptionType.OneHundredGBPlan;
		}
		else if (string.Equals(subscriptionName, "FREETRIAL", StringComparison.OrdinalIgnoreCase))
		{
			subscriptionType = SubscriptionType.TwoGBPlan;
		}

		return subscriptionType;
	}

	/// <summary>
	/// Automatically sets the end datetime based on the start datetime by just adding 30 days.
	/// </summary>
	/// <returns></returns>
	private void SetStartAndEndDate(DateTime createdDateTime)
	{
		this.ActivatedDateTimeUTC = createdDateTime;

		// Adding 2 days buffer so subscriptions can properly resolve.
		this.ExpirationDateTimeUTC = createdDateTime.AddDays(32);
	}
}