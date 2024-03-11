namespace Bookmarx.Shared.v1.ThirdParty.Google.Services;

public class ReCAPTCHAService : IReCAPTCHAService
{
	private readonly AppSettings _appSettings;

	private readonly IHttpClientFactory _httpClientFactory;

	public ReCAPTCHAService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
	{
		this._appSettings = appSettings.Value;
		this._httpClientFactory = httpClientFactory;
	}

	public async Task<SiteVerifyResponse> VerifyReCAPTCHAToken(string token)
	{
		SiteVerifyResponse response = new SiteVerifyResponse();
		var reCAPTCHAVerificationURL = $"{this._appSettings.GoogleAPIs.ReCAPTCHAVerificationURL}?secret={this._appSettings.GoogleAPIs.ReCAPTCHASecretKey}&response={token}";

		var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, reCAPTCHAVerificationURL)
		{
			//Headers =
			//{
			//    { HeaderNames.Accept, "application/vnd.github.v3+json" },
			//    { HeaderNames.UserAgent, "HttpRequestsSample" }
			//}
		};

		var httpClient = _httpClientFactory.CreateClient();
		var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

		if (httpResponseMessage.IsSuccessStatusCode)
		{
			var contentStream =
				await httpResponseMessage.Content.ReadAsStringAsync();

			response = JsonConvert.DeserializeObject<SiteVerifyResponse>(contentStream);
		}

		return response;
	}
}