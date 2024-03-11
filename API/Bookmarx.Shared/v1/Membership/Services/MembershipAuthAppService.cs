namespace Bookmarx.Shared.v1.Membership.Services;

public class MembershipAuthAppService : IMembershipAuthAppService
{
	private readonly ILogger<MembershipAuthAppService> _logger;

	private readonly IMapper _mapper;

	private readonly IOrderService _orderService;

	//private readonly PictyrsDbContext _pictyrsDbContext;

	private readonly ISubscriptionService _subscriptionService;

	public MembershipAuthAppService(

		//PictyrsDbContext pictyrsDbContext,
		IMapper _mapper,
		IOrderService orderService,

		ISubscriptionService subscriptionService,
		ILogger<MembershipAuthAppService> logger)
	{
		//this._pictyrsDbContext = pictyrsDbContext;
		this._mapper = _mapper;
		this._orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

		this._subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
		this._logger = logger;
	}

	public async Task<MemberAccount> CreateNewMemberAccountMember(MemberAccountDto memberAccount, string? invitationGuid)
	{
		// Being very thorough about sanitizing
		memberAccount.EmailAddress = memberAccount.EmailAddress.Trim();
		var newMemberAccount = this._mapper.Map<MemberAccount>(memberAccount);

		// TODO: Wire this up
		//using var dbTransaction = await this._pictyrsDbContext.Database.BeginTransactionAsync();

		try
		{
			// Do a check for any potential existing member with this email
			bool memberExists = false;

			//bool memberExists = this._pictyrsDbContext.MemberAccounts
			//	.Where(ma => ma.EmailAddress == memberAccount.EmailAddress)
			//	.AsNoTracking()
			//	.Any();

			// TODO: Need to handle the scenario where a member account already exists.
			if (!memberExists)
			{
				// Upon creation setup the date time here cuz db is goofy
				newMemberAccount.CreatedDateTimeUTC = DateTime.UtcNow;
				newMemberAccount.LastLoginDateTimeUTC = DateTime.UtcNow;
				newMemberAccount.AccountGuid = Guid.NewGuid(); // Make a new Guid! :)

				//// The save here will generate our new MemberAccountID for use later.
				//this._pictyrsDbContext.MemberAccounts.Add(newMemberAccount);

				//await this._pictyrsDbContext.SaveChangesAsync();

				// Finally, create a new order and subscription for the new member!
				Order newMemberOrder = await this._orderService.SaveNewAccountFreeTrialOrder(newMemberAccount.MemberAccountID);
				await this._subscriptionService.SaveAccountFreeTrialSubscription(newMemberAccount.MemberAccountID);

				//await dbTransaction.CommitAsync();
			}
		}
		catch (Exception ex)
		{
			// TODO: Add logging here.
		}

		// Send back the AccountGuid because we'll validate that the save worked
		return newMemberAccount;
	}

	public List<MemberAccount> GetMembers()
	{
		// TODO: Wire this up
		//return this._pictyrsDbContext.MemberAccounts.ToList();
		return new List<MemberAccount>();
	}

	public MemberAccount SignInWithEmailAndPassword(string authProviderUID)
	{
		MemberAccount memberAccount = new MemberAccount();

		MemberAccount currentMember = null;

		//var currentMember = this._pictyrsDbContext.MemberAccounts
		//	.Include(ma => ma.Subscriptions)
		//	.SingleOrDefault(ma => ma.AuthProviderUID == authProviderUID);

		if (currentMember?.MemberAccountID > 0)
		{
			currentMember.LastLoginDateTimeUTC = DateTime.UtcNow;

			//this._pictyrsDbContext.SaveChanges();
			memberAccount = currentMember;
		}

		return memberAccount;
	}

	public async Task<MemberAccount> SignInWithGoogle(MemberAccountDto memberAccountDto)
	{
		var memberAccount = this._mapper.Map<MemberAccount>(memberAccountDto);

		try
		{
			MemberAccount existingMember = null;

			//var existingMember = this._pictyrsDbContext.MemberAccounts
			//	.Include(ma => ma.Subscriptions)
			//	.SingleOrDefault(member => member.AuthProviderUID == memberAccount.AuthProviderUID);

			if (existingMember?.MemberAccountID > 0)
			{
				// If an account exists then just update the login details
				existingMember.LastLoginDateTimeUTC = DateTime.UtcNow;

				//await this._pictyrsDbContext.SaveChangesAsync();

				memberAccount = existingMember;
			}
			else
			{
				// If no account exists then make one
				// Upon creation setup the date time here cuz db is goofy
				var dateTimeUTCNow = DateTime.UtcNow;
				memberAccount.CreatedDateTimeUTC = dateTimeUTCNow;
				memberAccount.LastLoginDateTimeUTC = dateTimeUTCNow;
				memberAccount.AccountGuid = Guid.NewGuid(); // Set a new account guid

				//this._pictyrsDbContext.MemberAccounts.Add(memberAccount);

				//await this._pictyrsDbContext.SaveChangesAsync();

				// Finally, create a new order and subscription for the new member!
				Order newMemberOrder = await this._orderService.SaveNewAccountFreeTrialOrder(memberAccount.MemberAccountID);
				await this._subscriptionService.SaveAccountFreeTrialSubscription(memberAccount.MemberAccountID);
			}
		}
		catch (Exception ex)
		{
			this._logger.LogError(ex, "Failed to sign in with Google.");
		}

		// Send back the AccountGuid because we'll validate that the save worked
		return memberAccount;
	}
}