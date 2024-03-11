namespace Bookmarx.Shared.v1.Sales.Enums;

/// <summary>
/// https://stripe.com/docs/api/checkout/sessions/object#checkout_session_object-status
/// </summary>
public enum SessionStatusType
{
	None = 0,

	Open = 1,

	Complete = 2,

	Expired = 3
}