namespace Bookmarx.Shared.v1.Sales.Entities;

public class SalesTransaction
{
	public SalesTransaction(
		string currencyCode,
		decimal grossAmount,
		int memberAccountID,
		decimal? partnerCollectedProductTaxAmount,
		decimal? PartnerCollectedShippingTaxAmount,
		PaymentProviderType paymentProviderType,
		string paymentStatusName,
		string providerTransactionID,
		string sessionStatusName,
		TransactionType transactionType,
		ProviderTransactionType providerTransactionType,
		int orderID)
	{
		this.CurrencyCode = currencyCode;
		this.GrossAmount = grossAmount;
		this.MemberAccountID = memberAccountID;
		this.PartnerCollectedProductTaxAmount = partnerCollectedProductTaxAmount;
		this.PartnerCollectedShippingTaxAmount = PartnerCollectedShippingTaxAmount;
		this.PaymentProviderTypeID = paymentProviderType;
		this.PaymentStatusTypeID = this.ConvertPaymentStatusNameToType(paymentStatusName);
		this.ProviderTransactionID = providerTransactionID;
		this.SessionStatusTypeID = this.ConvertSessionStatusNameToType(sessionStatusName);
		this.TransactionTypeID = transactionType;
		this.TransactionDateTimeUTC = DateTime.UtcNow;
		this.ProviderTransactionTypeID = providerTransactionType;
		this.OrderID = orderID;
	}

	private SalesTransaction()
	{
		// Needed by EF
	}

	/// <summary>
	/// Represents an ISO currency code.
	/// </summary>
	public string CurrencyCode { get; private set; }

	public decimal GrossAmount { get; private set; }

	public string? InvoicePdfURL { get; private set; }

	public int MemberAccountID { get; private set; }

	public Order Order { get; private set; }

	public int OrderID { get; private set; }

	/// <summary>
	/// The product tax amount that was collected by the payment provider(s) we use.
	/// </summary>
	public decimal? PartnerCollectedProductTaxAmount { get; private set; } = 0m;

	/// <summary>
	/// The shipping tax amount that was collected by the payment provider(s) we use.
	/// </summary>
	public decimal? PartnerCollectedShippingTaxAmount { get; private set; } = 0m;

	public PaymentProviderType PaymentProviderTypeID { get; private set; }

	public PaymentStatusType PaymentStatusTypeID { get; private set; }

	/// <summary>
	/// This pairs up to any number of formatted third party transaction IDs.
	/// Invoices: in_123abc456def
	/// </summary>
	public string ProviderTransactionID { get; private set; }

	public ProviderTransactionType? ProviderTransactionTypeID { get; private set; }

	public SessionStatusType SessionStatusTypeID { get; private set; }

	public DateTime TransactionDateTimeUTC { get; private set; }

	public string? TransactionDescription { get; private set; }

	public int TransactionID { get; private set; }

	public TransactionType TransactionTypeID { get; private set; }

	public void UpdateDescription(string description)
	{
		this.TransactionDescription = description;
	}

	public void UpdateInvoicePdfURL(string invoicePdfURL)
	{
		this.InvoicePdfURL = invoicePdfURL;
	}

	private PaymentStatusType ConvertPaymentStatusNameToType(string paymentStatusName)
	{
		PaymentStatusType paymentStatusType = PaymentStatusType.None;

		switch (paymentStatusName)
		{
			case ("paid"):
				paymentStatusType = PaymentStatusType.Paid;
				break;

			case ("unpaid"):
				paymentStatusType = PaymentStatusType.Unpaid;
				break;

			case ("no_payment_required"):
				paymentStatusType = PaymentStatusType.NoPaymentRequired;
				break;

			default:
				break;
		}

		return paymentStatusType;
	}

	private SessionStatusType ConvertSessionStatusNameToType(string sessionStatusName)
	{
		SessionStatusType sessionStatusType = SessionStatusType.None;

		switch (sessionStatusName)
		{
			case ("open"):
				sessionStatusType = SessionStatusType.Open;
				break;

			case ("complete"):
				sessionStatusType = SessionStatusType.Complete;
				break;

			case ("expired"):
				sessionStatusType = SessionStatusType.Expired;
				break;

			default:
				break;
		}

		return sessionStatusType;
	}
}