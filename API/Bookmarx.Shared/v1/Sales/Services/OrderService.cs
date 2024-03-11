namespace Bookmarx.Shared.v1.Sales.Services;

public class OrderService : IOrderService
{
	private readonly ILogger<OrderService> _logger;

	//private readonly PictyrsDbContext _pictyrsDbContext;

	public OrderService(
		//PictyrsDbContext pictyrsDbContext, 
		ILogger<OrderService> logger)
	{
		//this._pictyrsDbContext = pictyrsDbContext ?? throw new ArgumentNullException(nameof(pictyrsDbContext));
		this._logger = logger;
	}

	public async Task<Order> SaveNewAccountFreeTrialOrder(int newMemberAccountID)
	{
		Guid orderGuid = Guid.NewGuid();
		Order freeTrialOrder = new Order(true, newMemberAccountID, DateTime.UtcNow, orderGuid);

		try
		{
			OrderProduct freeTrialOrderProduct = new OrderProduct("FREETRIAL", 1);
			freeTrialOrder.AddOrderProduct(freeTrialOrderProduct);

			// TODO: Wire this up
			//await this._pictyrsDbContext.Orders.AddAsync(freeTrialOrder);
			//await this._pictyrsDbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			this._logger.LogError(ex, "Failed to save new account free trial order.");
		}

		return freeTrialOrder;
	}

	public async Task<Order> SaveOrder(Order order)
	{
		if (order != null)
		{
			// TODO: Wire this up
			//await this._pictyrsDbContext.Orders.AddAsync(order);
			//await this._pictyrsDbContext.SaveChangesAsync();
		}

		return order;
	}
}