namespace Bookmarx.Shared.v1.Identity.Services;

public class GoogleFirebaseTokenValidatorService : ITokenValidatorService
{
	private readonly AppSettings _appSettings;

	private readonly IHttpContextAccessor _httpContextAccessor;

	//private readonly PictyrsDbContext _pictyrsDbContext;

	private readonly ISubscriptionValidationService _subscriptionValidationService;

	public GoogleFirebaseTokenValidatorService(
		IOptions<AppSettings> appSettings,

		//PictyrsDbContext pictyrsDbContext,
		IHttpContextAccessor httpContextAccessor,
		ISubscriptionValidationService subscriptionValidationService)
	{
		this._appSettings = appSettings.Value;

		//this._pictyrsDbContext = pictyrsDbContext;
		this._httpContextAccessor = httpContextAccessor;
		this._subscriptionValidationService = subscriptionValidationService;
	}

	public async Task<bool> CheckTokenIsValidAndMemberExists(string accessToken, string authProviderUID, Guid accountGuid)
	{
		bool isValid = false;

		if (!string.IsNullOrEmpty(accessToken))
		{
			try
			{
				// https://firebase.google.com/docs/auth/admin/verify-id-tokens#verify_id_tokens_using_the_firebase_admin_sdk
				FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(accessToken);
				if (decodedToken != null)
				{
					string uid = decodedToken.Uid;

					// Make sure that both the Uid are set and that it matches the given authProviderUID
					// If the AuthProviderUID doesn't match then there's no sense running the second check, so it's nested
					if (decodedToken?.Uid != null && decodedToken?.Uid == authProviderUID)
					{
						// TODO: Wire this up
						//var memberExists = this._pictyrsDbContext.MemberAccounts.Any(m => m.AuthProviderUID == authProviderUID && m.AccountGuid == accountGuid);
						var memberExists = false;

						if (memberExists)
						{
							isValid = true;
						}
					}
				}
				else
				{
					throw new ArgumentNullException($"{nameof(decodedToken)} failed to set a value, authentication fails.");
				}
			}
			catch (Exception ex)
			{
			}
		}

		return isValid;
	}

	public async Task<bool> CheckTokenIsValidAndSetIdentityUser(string bearerToken, string authProviderUID)
	{
		bool isValid = false;

		if (!string.IsNullOrEmpty(bearerToken))
		{
			try
			{
				// https://firebase.google.com/docs/auth/admin/verify-id-tokens#verify_id_tokens_using_the_firebase_admin_sdk
				FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(bearerToken);

				if (decodedToken != null)
				{
					// TODO: Set all of the dotnet user claims equal to the list of user claims that came from google.
					// Then use these values later on to check against, like in the Tus API endpoints.
					string uid = decodedToken.Uid;

					MemberAccount? member = null;

					// TODO: Wire this up
					//var member = this._pictyrsDbContext.MemberAccounts
					//	.AsNoTracking()
					//	.FirstOrDefault(m => m.AuthProviderUID == authProviderUID);

					if (member != null)
					{
						// Making sure that the Uid value is set from the token and that it matches the X-Api-Key value (actually the auth provider UID)
						// By validating the token Uid against the X-Api-Key (auth provider uid) we can then extract the x-api-key value to be used
						// later on in the CurrentMemberService knowing for certain that the user is who they say they are and not needing
						// to read out the token values again using the VerifyIdTokenAsync, which saves calls to Google Firebase
						// TODO: Important, some day move all this junk out of here and down in to the calling AuthHandler class that's using it most.
						if (decodedToken?.Uid != null && decodedToken?.Uid == authProviderUID)
						{
							// Need to actually set a claim properly so we can check the httpcontext User property anywhere we need.
							// https://learn.microsoft.com/en-us/dotnet/api/system.security.claims?view=net-6.0
							// Create the claims from Google and map them to the HttpContext User Claims in .NET 6.
							// Setting any one of the claims here will set the identity user is authenticated property to true, which we can check later in Tus endpoints.
							// https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claim?view=net-6.0
							var claims = new List<Claim>();

							// Email
							if (decodedToken.Claims.TryGetValue("email", out object email))
							{
								// https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes?view=net-6.0
								claims.Add(new Claim(ClaimTypes.Email, Convert.ToString(email), ClaimValueTypes.String, decodedToken.Issuer));
							}

							// Email Verified
							if (decodedToken.Claims.TryGetValue("email_verified", out object emailVerified))
							{
								claims.Add(new Claim("EmailVerified", Convert.ToString(emailVerified), ClaimValueTypes.Boolean, decodedToken.Issuer));
							}

							// Google User ID - AKA AuthProviderUID
							if (decodedToken.Claims.TryGetValue("user_id", out object userId))
							{
								claims.Add(new Claim("AuthProviderUID", Convert.ToString(userId), ClaimValueTypes.String, decodedToken.Issuer));
							}

							// Name
							if (decodedToken.Claims.TryGetValue("name", out object name))
							{
								claims.Add(new Claim(ClaimTypes.Name, Convert.ToString(name), ClaimValueTypes.String, decodedToken.Issuer));
							}

							// Pictyrs AccountGuid
							claims.Add(new Claim("AccountGuid", member.AccountGuid.ToString(), ClaimValueTypes.String, "https://pictyrs.app"));

							// Pictyrs Has Valid Subscription
							var hasActiveSubscription = this._subscriptionValidationService.ValidateSubscription(member);
							claims.Add(new Claim("SubscriptionIsActive", hasActiveSubscription.ToString(), ClaimValueTypes.Boolean, "https://pictyrs.app"));

							// Set up the newly minted identity for the HttpContext identity user.
							// https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsidentity?view=net-6.0
							var identity = new ClaimsIdentity(claims, ApiAuthPolicy.ActiveSessionAuthorization);

							// Set the HttpContext User to our new Principal claim. Whew.
							// https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal?view=net-6.0
							var principal = new ClaimsPrincipal(identity);

							// Finally, set the HttpContext User to our new principal.
							// The Identity User can now be accessed in the current context anywhere in the app. Neat.
							this._httpContextAccessor.HttpContext.User = principal;
						}
						else
						{
							throw new ArgumentNullException($"No corresponding user account exists with the given Google auth UID. Auth setup fails.");
						}
					}

					// If all the junk above has issues it's fine, we just need to know that the token was valid.
					isValid = true;
				}
			}
			catch (Exception ex)
			{
			}
		}

		return isValid;
	}
}