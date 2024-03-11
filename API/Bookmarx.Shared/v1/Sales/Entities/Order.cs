namespace Bookmarx.Shared.v1.Sales.Entities;

public class Order
{
	public Order(
		bool emailConfirmationSent,
		int memberAccountID,
		DateTime orderDateTime,
		Guid orderGuid)
	{
		this.EmailConfirmationSent = emailConfirmationSent;
		this.MemberAccountID = memberAccountID;
		this.OrderDateTimeUTC = orderDateTime;
		this.OrderGuid = orderGuid;
	}

	private Order()
	{
		// Needed for EF
	}

	public bool EmailConfirmationSent { get; private set; }

	public int MemberAccountID { get; private set; }

	public DateTime OrderDateTimeUTC { get; private set; }

	public Guid OrderGuid { get; private set; }

	public int OrderID { get; private set; }

	public List<OrderProduct> OrderProducts { get; private set; } = new List<OrderProduct>();

	public List<SalesTransaction> Transactions { get; private set; } = new List<SalesTransaction>();

	public void AddOrderProduct(OrderProduct product)
	{
		this.OrderProducts.Add(product);
	}
}