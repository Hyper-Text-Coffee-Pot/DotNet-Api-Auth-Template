namespace Bookmarx.API.v1.Controllers.Membership.Models;

public class MemberAccountCreateRequest
{
	public string? AccessToken { get; set; }

	/// <summary>
	/// APID = AuthProviderID
	/// Intentionally obfuscating this a bit so it isn't so obvious in the app.
	/// </summary>
	public string APID { get; set; }

	public string EmailAddress { get; set; }

	public string? FirstName { get; set; }

	/// <summary>
	/// IG stands for InvitationGuid.
	/// This needs to match an actual invitation guid to be an approved signup.
	/// </summary>
	public string? IG { get; set; }

	public string? LastName { get; set; }

	public string ReCAPTCHAToken { get; set; }
}