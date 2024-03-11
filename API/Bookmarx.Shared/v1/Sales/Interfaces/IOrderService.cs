using Bookmarx.Shared.v1.Sales.Entities;

namespace Bookmarx.Shared.v1.Sales.Interfaces;

public interface IOrderService
{
	/// <summary>
	/// Creates a new Order and OrderProducts for a new signup.
	/// Each new signup gets a free trial and 2 GB plan by default.
	/// </summary>
	/// <param name="newMemberAccountID"></param>
	/// <returns></returns>
	Task<Order> SaveNewAccountFreeTrialOrder(int newMemberAccountID);

	Task<Order> SaveOrder(Order order);
}