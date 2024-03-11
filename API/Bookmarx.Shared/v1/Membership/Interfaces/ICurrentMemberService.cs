namespace Bookmarx.Shared.v1.Membership.Interfaces;

public interface ICurrentMemberService
{
	/// <summary>
	/// Gets the currently signed in member account based on the values passed in the headers.
	/// It's safer to do it this way rather than passing parameters as we've already used
	/// the AuthHandler to validate that the user's token was valid and that the Guid matches.
	/// Grabs the X-Api-Key value which is actually an auth provider UID which has already been
	/// validated using the auth policy middleware so we know we can trust the value.
	/// NOTE: This returns a non-tracked member account. E.g. it's a read-only return value.
	/// </summary>
	/// <returns>A member account or null.</returns>
	MemberAccount? GetMember();
}