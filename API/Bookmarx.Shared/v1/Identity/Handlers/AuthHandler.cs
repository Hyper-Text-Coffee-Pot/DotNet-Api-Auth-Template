namespace Bookmarx.Shared.v1.Identity.Handlers;

public class AuthHandler : AuthorizationHandler<AccessTokenRequirement>
{
	private readonly AppSettings _appSettings;

	private readonly IHttpContextAccessor _httpContextAccessor;

	private readonly ITokenValidatorService _tokenValidatorService;

	public AuthHandler(
		IOptions<AppSettings> appSettings,
		IHttpContextAccessor httpContextAccessor,
		ITokenValidatorService tokenValidatorService)
	{
		this._appSettings = appSettings.Value;
		this._httpContextAccessor = httpContextAccessor;
		this._tokenValidatorService = tokenValidatorService;
	}

	/// <summary>
	/// Get the authentication credentials and process them
	/// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0
	/// https://stackoverflow.com/questions/25855698/how-can-i-retrieve-basic-authentication-credentials-from-the-header
	/// https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
	/// https://stackoverflow.com/questions/25855698/how-can-i-retrieve-basic-authentication-credentials-from-the-header
	/// </summary>
	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessTokenRequirement requirement)
	{
		var authorizationHeader = this._httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
		var xApiKeyHeader = this._httpContextAccessor.HttpContext.Request.Headers[this._appSettings.CustomHeaders.AuthUserIDHeader].ToString();

		if (!string.IsNullOrEmpty(authorizationHeader)
			&& authorizationHeader.StartsWith("Bearer")
			&& !string.IsNullOrEmpty(xApiKeyHeader))
		{
			var bearerToken = authorizationHeader.Substring("Bearer ".Length).Trim();

			// This is obfuscated on the client, thus using the X-Api-Key
			// It is actually the ID given by the auth provider
			var authProviderUID = xApiKeyHeader.Trim();

			if (!string.IsNullOrEmpty(bearerToken))
			{
				requirement.AccessTokenIsValid = await this._tokenValidatorService.CheckTokenIsValidAndSetIdentityUser(bearerToken, authProviderUID);
			}
		}
		else
		{
			throw new Exception("Request failed.");
		}

		if (requirement.AccessTokenIsValid)
		{
			context.Succeed(requirement);
		}
	}
}