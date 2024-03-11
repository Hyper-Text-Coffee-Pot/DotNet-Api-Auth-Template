namespace Bookmarx.Shared.v1.Sales.Interfaces;

public interface ISubscriptionService
{
	Task<Subscription> SaveAccountFreeTrialSubscription(int memberAccountID);

	Task<Subscription> SaveSubscription(Subscription subscription);
}