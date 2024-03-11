using Microsoft.AspNetCore.Authorization;

namespace Bookmarx.Shared.v1.Identity.Requirements;

public class AccessTokenRequirement : IAuthorizationRequirement
{
	/// <summary>
	/// The requirements for this are very minimal, but could become more
	/// complex in the future.
	/// </summary>
	/// <param name="isAuthorized"></param>
	public AccessTokenRequirement()
	{
	}

	// Set this to false by default. More restrictive that way.
	public bool AccessTokenIsValid { get; set; } = false;
}