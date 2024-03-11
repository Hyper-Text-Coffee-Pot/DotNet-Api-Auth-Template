namespace Bookmarx.Shared.v1.ThirdParty.Google.Interfaces;

public interface IReCAPTCHAService
{
	Task<SiteVerifyResponse> VerifyReCAPTCHAToken(string token);
}