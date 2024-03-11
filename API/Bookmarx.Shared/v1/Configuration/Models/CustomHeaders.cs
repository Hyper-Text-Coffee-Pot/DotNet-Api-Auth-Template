namespace Bookmarx.Shared.v1.Configuration.Models;

public class CustomHeaders
{
	public string AuthUserIDHeader { get; set; }

	public string StripeWebhookHeader { get; set; }

	public string SubscriptionStatusHeader { get; set; }
}