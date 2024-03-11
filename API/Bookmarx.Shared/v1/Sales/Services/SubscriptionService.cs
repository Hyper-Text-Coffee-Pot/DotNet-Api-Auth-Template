namespace Bookmarx.Shared.v1.Sales.Services;

public class SubscriptionService : ISubscriptionService
{
	private readonly ILogger<SubscriptionService> _logger;

	//private readonly PictyrsDbContext _pictyrsDbContext;

	public SubscriptionService(

		//PictyrsDbContext pictyrsDbContext,
		ILogger<SubscriptionService> logger)
	{
		//this._pictyrsDbContext = pictyrsDbContext ?? throw new ArgumentNullException(nameof(pictyrsDbContext));
		this._logger = logger;
	}

	public async Task<Subscription> SaveAccountFreeTrialSubscription(int memberAccountID)
	{
		var dummyProviderSubscriptionID = this.GenerateFreeProviderSubscriptionID();
		Subscription freeTrialSubscription = new Subscription(memberAccountID, DateTime.UtcNow, "FREETRIAL", dummyProviderSubscriptionID);

		try
		{
			// TODO: Wire this up
			//await this._pictyrsDbContext.Subscriptions.AddAsync(freeTrialSubscription);
			//await this._pictyrsDbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			this._logger.LogError(ex, "Failed to save new account free trial subscription.");
		}

		return freeTrialSubscription;
	}

	public async Task<Subscription> SaveSubscription(Subscription subscription)
	{
		if (subscription != null)
		{
			// TODO: Wire this up
			//this._pictyrsDbContext.Subscriptions.Add(subscription);
			//await this._pictyrsDbContext.SaveChangesAsync();
		}

		return subscription;
	}

	/// <summary>
	/// The ProviderTransactionID is required so just supply a dummy free_ style one.
	/// This is probably total overkill, but hey. It's neat.
	/// https://briancaos.wordpress.com/2022/02/24/c-datetime-to-unix-timestamps/
	/// </summary>
	/// <returns></returns>
	private string GenerateFreeProviderSubscriptionID()
	{
		string guidWithoutDashes = Guid.NewGuid().ToString("N");
		DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
		string unixTime = dto.ToUnixTimeSeconds().ToString();
		string dummyProviderSubscriptionID = $"free_{guidWithoutDashes}_{unixTime}";
		return dummyProviderSubscriptionID;
	}
}