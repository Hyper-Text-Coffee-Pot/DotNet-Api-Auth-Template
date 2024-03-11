namespace Bookmarx.Shared.v1.Identity.Policies;

public static class ApiAuthPolicy
{
	/// <summary>
	/// Currently, our only policy and is the main one used for all API interactions at the moment.
	/// </summary>
	public const string ActiveSessionAuthorization = "ActiveSessionAuthorization";
}