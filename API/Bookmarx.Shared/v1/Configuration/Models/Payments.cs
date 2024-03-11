namespace Bookmarx.Shared.v1.Configuration.Models;

public class Payments
{
	public StripeAPIKeys StripeAPIKeys { get; set; }

	public StripeWebhookSigningSecrets StripeWebhookSigningSecrets { get; set; }
}