namespace Bookmarx.Shared.v1.Membership.Interfaces;

public interface ISubscriptionValidationService
{
	/// <summary>
	/// Does some logic to check whether the current user has a valid subscription or not.
	/// </summary>
	/// <param name="member"></param>
	/// <returns></returns>
	bool ValidateSubscription(MemberAccount member);
}