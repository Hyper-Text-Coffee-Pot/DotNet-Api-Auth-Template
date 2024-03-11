namespace Bookmarx.Shared.v1.ThirdParty.Google.Models;

public class SiteVerifyResponse
{
	[JsonProperty("action")]
	public string Action { get; set; }

	/// <summary>
	/// Timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
	/// </summary>
	[JsonProperty("challenge_ts")]
	public DateTime ChallengeTS { get; set; }

	/// <summary>
	/// missing-input-secret	The secret parameter is missing.
	/// invalid-input-secret	The secret parameter is invalid or malformed.
	/// missing-input-response	The response parameter is missing.
	/// invalid-input-response	The response parameter is invalid or malformed.
	/// bad-request	The request is invalid or malformed.
	/// timeout-or-duplicate	The response is no longer valid: either is too old or has been used previously.
	/// </summary>
	[JsonProperty("error-codes")]
	public List<string> ErrorCodes { get; set; }

	/// <summary>
	/// The hostname of the site where the reCAPTCHA was solved
	/// </summary>
	[JsonProperty("hostname")]
	public string HostName { get; set; }

	[JsonProperty("score")]
	public decimal Score { get; set; }

	[JsonProperty("success")]
	public bool Success { get; set; }
}