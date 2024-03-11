namespace Bookmarx.Shared.v1.Identity.Interfaces;

public interface ITokenValidatorService
{
	/// <summary>
	/// Validates a JWT token using the implemented token provider.
	/// Currently that provider is Google Firebase Auth.
	/// Use this method for everything except for account creation because it
	/// also runs a check that the given AccountGuid matches AS WELL as the AuthProviderUID.
	/// This is a much more thorough check that the auth provider ID and token that was sent along
	/// with the account guid are exact matches. If it matches on these two together there is a very
	/// small chance that someone was able to spoof the credentials, making this much more secure.
	/// </summary>
	/// <param name="jwtToken">The token to validate.</param>
	/// <param name="authProviderUID">The UID given by the auth provider.</param>
	/// <returns>If the token is valid (true) or not (false).</returns>
	Task<bool> CheckTokenIsValidAndMemberExists(string accessToken, string authProviderUID, Guid accountGuid);

	/// <summary>
	/// Validates a JWT token using the implemented token provider.
	/// Currently that provider is Google Firebase Auth.
	/// This also then sets the HttpContext User Claims and sets is Authenticated.
	/// </summary>
	/// <param name="bearerToken">The JWT token to validate.</param>
	/// <param name="authProviderUID">The UID given by the auth provider.</param>
	/// <returns>If the token is valid (true) or not (false).</returns>
	Task<bool> CheckTokenIsValidAndSetIdentityUser(string bearerToken, string authProviderUID);
}