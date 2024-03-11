namespace Bookmarx.API.v1.Controllers.Membership;

[ApiController]
[Route("v{version:apiVersion}/membership-auth")]
public class MembershipAuthController : ControllerBase
{
	private readonly IMembershipAuthAppService _authAppService;

	private readonly IMapper _mapper;

	private readonly IReCAPTCHAService _reCAPTCHAService;

	private readonly ISubscriptionValidationService _subscriptionValidationService;

	private readonly ITokenValidatorService _tokenValidatorService;

	public MembershipAuthController(
	IMembershipAuthAppService authAppService,
	IMapper mapper,
	ITokenValidatorService tokenValidatorService,
	IReCAPTCHAService reCAPTCHAService,
	ISubscriptionValidationService subscriptionValidationService
	)
	{
		this._authAppService = authAppService;
		this._mapper = mapper;
		this._tokenValidatorService = tokenValidatorService;
		this._reCAPTCHAService = reCAPTCHAService;
		this._subscriptionValidationService = subscriptionValidationService;
	}

	[HttpPost]
	[Route("signup-with-email-and-password")]
	public async Task<IdentityActionResponseDto> CreateNewMemberAccount(MemberAccountCreateRequest memberAccountCreateRequest)
	{
		var response = new IdentityActionResponseDto();

		try
		{
			// Do something with this at a later point
			// Wrapping it in a try catch cuz I don't want the rest to fail
			var siteVerifyResponse = await this._reCAPTCHAService.VerifyReCAPTCHAToken(memberAccountCreateRequest.ReCAPTCHAToken);
		}
		catch (Exception ex)
		{
			// Eat it.
		}

		if (!string.IsNullOrEmpty(memberAccountCreateRequest?.APID)
			&& !string.IsNullOrEmpty(memberAccountCreateRequest?.EmailAddress))
		{
			// Sanitize some stuff
			memberAccountCreateRequest.EmailAddress = memberAccountCreateRequest.EmailAddress.Trim();

			var newMember = this._mapper.Map<MemberAccountDto>(memberAccountCreateRequest);

			if (await this._tokenValidatorService.CheckTokenIsValidAndSetIdentityUser(memberAccountCreateRequest.AccessToken, memberAccountCreateRequest.APID))
			{
				// Finally, create the account
				var newMemberAccount = await this._authAppService.CreateNewMemberAccountMember(newMember, memberAccountCreateRequest.IG);
				response.OGID = newMemberAccount.AccountGuid.ToString();

				// To start every user will have 30 days before they will be asked to select a subscription.
				response.IsSubscriptionValid = true;
			}
		}

		return response;
	}

	[HttpPost]
	[Route("sign-in-with-email-and-password")]
	public async Task<IdentityActionResponseDto> SignInWithEmailAndPassword(string authToken, string authProviderUID, string reCAPTCHAToken)
	{
		IdentityActionResponseDto identityActionResponseDto = new IdentityActionResponseDto();

		try
		{
			// Do something with this at a later point
			// Wrapping it in a try catch cuz I don't want the rest to fail
			var siteVerifyResponse = await this._reCAPTCHAService.VerifyReCAPTCHAToken(reCAPTCHAToken);
		}
		catch (Exception ex)
		{
			// Do nothing for now
		}

		if (!string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(authProviderUID))
		{
			// Validate the token before updating the last login details
			if (await this._tokenValidatorService.CheckTokenIsValidAndSetIdentityUser(authToken, authProviderUID))
			{
				var signedInMemberAccount = this._authAppService.SignInWithEmailAndPassword(authProviderUID);

				if (signedInMemberAccount != null
					&& signedInMemberAccount?.MemberAccountID > 0)
				{
					identityActionResponseDto.OGID = signedInMemberAccount.AccountGuid.ToString();

					// TODO: Swap this out for the new identity user values.
					identityActionResponseDto.IsSubscriptionValid = this._subscriptionValidationService.ValidateSubscription(signedInMemberAccount);
				}
			}
		}

		return identityActionResponseDto;
	}

	[HttpPost]
	[Route("sign-in-with-google")]
	public async Task<IdentityActionResponseDto> SignInWithGoogle(MemberAccountCreateRequest memberAccountCreateRequest)
	{
		IdentityActionResponseDto identityActionResponseDto = new IdentityActionResponseDto();

		try
		{
			// Do something with this at a later point
			// Wrapping it in a try catch cuz I don't want the rest to fail
			var siteVerifyResponse = await this._reCAPTCHAService.VerifyReCAPTCHAToken(memberAccountCreateRequest.ReCAPTCHAToken);
		}
		catch (Exception ex)
		{
			// Do nothing for now
		}

		if (!string.IsNullOrEmpty(memberAccountCreateRequest?.APID)
			&& !string.IsNullOrEmpty(memberAccountCreateRequest?.EmailAddress))
		{
			var newMember = this._mapper.Map<MemberAccountDto>(memberAccountCreateRequest);

			if (await this._tokenValidatorService.CheckTokenIsValidAndSetIdentityUser(memberAccountCreateRequest.AccessToken, memberAccountCreateRequest.APID))
			{
				// Finally, create the account or just update some information if it already exists.
				var signedInMemberAccount = await this._authAppService.SignInWithGoogle(newMember);
				if (signedInMemberAccount != null
					&& signedInMemberAccount?.MemberAccountID > 0)
				{
					identityActionResponseDto.OGID = signedInMemberAccount.AccountGuid.ToString();
					identityActionResponseDto.IsSubscriptionValid = this._subscriptionValidationService.ValidateSubscription(signedInMemberAccount);
				}
			}
		}

		return identityActionResponseDto;
	}
}