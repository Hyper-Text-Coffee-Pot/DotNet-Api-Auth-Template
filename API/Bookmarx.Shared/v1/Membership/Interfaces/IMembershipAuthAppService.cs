namespace Bookmarx.Shared.v1.Membership.Interfaces;

public interface IMembershipAuthAppService
{
	/// <summary>
	/// Create a new AccountMember.
	/// </summary>
	/// <param name="member"></param>
	/// <returns>The MemberAccount of the new account member to verify that it worked.</returns>
	Task<MemberAccount> CreateNewMemberAccountMember(MemberAccountDto member, string? invitationGuid);

	List<MemberAccount> GetMembers();

	/// <summary>
	/// Signs a user in and performs some basic actions like updating the last login datetime.
	/// </summary>
	/// <param name="authProviderUID">The User ID given by the auth provider.</param>
	/// <returns>The logged in user.</returns>
	MemberAccount SignInWithEmailAndPassword(string authProviderUID);

	/// <summary>
	/// Sign in or create account if none exists.
	/// The flow for social logins is a little different. Each time they use social logins
	/// we'll check for if an account exists. If not make one, if yes just update the last login info.
	/// </summary>
	/// <param name="memberAccountCreateRequest"></param>
	/// <returns>The AccountGuid of the new account member to verify that it worked.</returns>
	Task<MemberAccount> SignInWithGoogle(MemberAccountDto member);
}