namespace Bookmarx.Shared.v1.Membership.Services;

/// <summary>
/// The Application API BaseService provides each service with properties they will all likely need.
/// Including things like the CurrentMemberAccount which is collected from middleware on every HTTP request.
/// </summary>
public abstract class BaseMembershipService
{
	private readonly ICurrentMemberService _currentMemberService;

	private readonly ISubscriptionValidationService _subscriptionValidationService;

	private MemberAccount? currentMemberAccount = null;

	public BaseMembershipService(
		ICurrentMemberService currentMemberService,
		ISubscriptionValidationService subscriptionValidationService)
	{
		this._currentMemberService = currentMemberService;
		this._subscriptionValidationService = subscriptionValidationService;
	}

	/// <summary>
	/// Gets the member account information that is passed in the headers on every request from the application.
	/// </summary>
	protected MemberAccount? CurrentMemberAccount
	{
		get
		{
			if (this.currentMemberAccount == null)
			{
				// If the current member wasn't already retrieved let's go get it.
				// This helps to reduce db hits.
				this.currentMemberAccount = this._currentMemberService.GetMember();
				return this.currentMemberAccount;
			}
			else
			{
				// Otherwise just return it.
				return this.currentMemberAccount;
			}
		}

		private set { }
	}

	protected Subscription? CurrentSubscription
	{
		get
		{
			return this.CurrentMemberAccount?
				.Subscriptions
				.OrderByDescending(s => s.ActivatedDateTimeUTC)
				.FirstOrDefault();
		}
	}

	/// <summary>
	/// Returns true if the current member has a valid storage subscription, otherwise false.
	/// </summary>
	protected bool IsSubscriptionValid
	{
		get
		{
			bool isSubscriptionValid = false;

			if (this.CurrentMemberAccount != null)
			{
				isSubscriptionValid = this._subscriptionValidationService.ValidateSubscription(this.CurrentMemberAccount);
			}

			return isSubscriptionValid;
		}

		private set { }
	}
}