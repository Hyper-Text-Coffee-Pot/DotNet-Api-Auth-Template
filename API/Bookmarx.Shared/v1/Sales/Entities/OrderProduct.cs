namespace Bookmarx.Shared.v1.Sales.Entities;

public class OrderProduct
{
	public OrderProduct(
		int orderID,
		string subscriptionName,
		int quantity)
	{
		this.OrderID = orderID;
		this.ProductMasterID = this.ConvertSubscriptionNameToProductMasterID(subscriptionName);
		this.Quantity = quantity;
	}

	public OrderProduct(string subscriptionName, int quantity)
	{
		this.ProductMasterID = this.ConvertSubscriptionNameToProductMasterID(subscriptionName);
		this.Quantity = quantity;
	}

	private OrderProduct()
	{
		// Needed for EF
	}

	public int OrderID { get; set; }

	public int OrderProductID { get; set; }

	public int ProductMasterID { get; set; }

	public int Quantity { get; set; }

	/// <summary>
	/// Sets the product master ID based on the name of the plan that comes back from the webhook request.
	/// This is pretty gross, but it does the trick, for now.
	/// </summary>
	/// <param name="subscriptionName"></param>
	private int ConvertSubscriptionNameToProductMasterID(string subscriptionName)
	{
		// Default to 0, this will cause an error if nothing gets set, which is good.
		int productMasterID = 0;

		// TODO: Someday make this not so terrible.
		if (subscriptionName.Contains("25 GB"))
		{
			productMasterID = 1;
		}
		else if (subscriptionName.Contains("50 GB"))
		{
			productMasterID = 2;
		}
		else if (subscriptionName.Contains("100 GB"))
		{
			productMasterID = 3;
		}
		else if (string.Equals(subscriptionName, "FREETRIAL", StringComparison.OrdinalIgnoreCase))
		{
			productMasterID = 4;
		}
		else if (subscriptionName.Contains("250 GB"))
		{
			productMasterID = 5;
		}
		else if (subscriptionName.Contains("500 GB"))
		{
			productMasterID = 6;
		}

		return productMasterID;
	}
}