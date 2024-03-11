namespace Bookmarx.Shared.v1.Membership.Services;

public class CurrentMemberService : ICurrentMemberService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	//private readonly IPictyrsDbContext _pictyrsDbContext;

	public CurrentMemberService(
		IHttpContextAccessor httpContextAccessor

		//IPictyrsDbContext pictyrsDbContext)
		)
	{
		this._httpContextAccessor = httpContextAccessor;

		//this._pictyrsDbContext = pictyrsDbContext;
	}

	public MemberAccount? GetMember()
	{
		MemberAccount? currentMemberAccount = null;

		var authProviderUID = this._httpContextAccessor.HttpContext.User.FindFirst("AuthProviderUID");
		if (!string.IsNullOrEmpty(authProviderUID?.Value))
		{
			// TODO: Implement this
			//currentMemberAccount = this._pictyrsDbContext.MemberAccounts
			//	.Where(ma => ma.AuthProviderUID == authProviderUID.Value)
			//	.Include(ma => ma.Subscriptions)
			//	.AsNoTracking()
			//	.FirstOrDefault();
		}
		else
		{
			throw new Exception("Request failed.");
		}

		return currentMemberAccount;
	}
}