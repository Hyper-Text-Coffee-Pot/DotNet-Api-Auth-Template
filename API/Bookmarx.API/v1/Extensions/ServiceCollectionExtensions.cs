namespace Bookmarx.API.v1.Extensions;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#code-try-4
	/// Register ALL custom DI dependencies here.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection RegisterCustomDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<ITokenValidatorService, GoogleFirebaseTokenValidatorService>();
		services.AddScoped<IReCAPTCHAService, ReCAPTCHAService>();
		services.AddScoped<ICurrentMemberService, CurrentMemberService>();
		services.AddScoped<ISubscriptionValidationService, SubscriptionValidationService>();
		services.AddScoped<IMembershipAuthAppService, MembershipAuthAppService>();
		services.AddScoped<IOrderService, OrderService>();
		services.AddScoped<ISubscriptionService, Shared.v1.Sales.Services.SubscriptionService>();

		return services;
	}
}