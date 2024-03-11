namespace Bookmarx.Shared.v1.Sales.Enums;

/// <summary>
/// https://stripe.com/docs/api/checkout/sessions/object#checkout_session_object-payment_status
/// </summary>
public enum PaymentStatusType
{
	None = 0,

	Paid = 1,

	Unpaid = 2,

	NoPaymentRequired = 3
}