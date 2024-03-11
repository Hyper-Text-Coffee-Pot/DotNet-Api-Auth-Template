namespace Bookmarx.Shared.v1.Membership.Services;

public class SubscriptionValidationService : ISubscriptionValidationService
{
	private readonly AppSettings _appSettings;

	public SubscriptionValidationService(IOptions<AppSettings> appSettings)
	{
		this._appSettings = appSettings.Value;
	}

	public bool ValidateSubscription(MemberAccount memberAccount)
	{
		bool subscriptionIsValid = false;

		// Giving a trial window (see appsettings) after initial signup before we ask a user to select a subscription plan.
		if ((DateTime.UtcNow - memberAccount.CreatedDateTimeUTC).TotalDays <= Convert.ToInt32(this._appSettings.Products.SubscriptionFreeTrialDays))
		{
			subscriptionIsValid = true;
		}
		else if (memberAccount.Subscriptions
									  .OrderByDescending(s => s.ActivatedDateTimeUTC)
									  .FirstOrDefault()?.ExpirationDateTimeUTC >= DateTime.UtcNow)
		{
			// Otherwise check that the user has an active subscription, and if so that the expiration date is after the current datetime.
			subscriptionIsValid = true;
		}

		return subscriptionIsValid;
	}
}