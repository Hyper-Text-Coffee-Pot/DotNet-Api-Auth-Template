using Bookmarx.Shared.v1.Configuration.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Bookmarx.API.v1.Filters;

/// <summary>
/// Amazing! Custom header interceptors/filters/middleware/atrributes.
/// Place the attribute [SubscriptionStatusHeader] on individual endpoints or on an entire controller to enforce the attribute rule.
/// This attribute retrieves a true or false value indicating whether or not their subscription has expired.
/// It is then intercepted on the client and used to update the PictyrsUser localStorage variable to toggle a payment modal.
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0#dependency-injection
/// The Service Filter Attribute type must be used if you want DI to be supported.
/// Decorate any controller or endpoint with the following to enforce this filter.
/// [ServiceFilter(typeof(SubscriptionHeaderFilterService))]
/// Also notice how this is registered on startup of the application to provide the instance to the ServiceFilter
/// builder.Services.AddScoped<\SubscriptionHeaderFilterService\>();
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0#servicefilterattribute
/// </summary>
public class SubscriptionHeaderFilterService : BaseMembershipService, IResultFilter
{
	private readonly AppSettings _appSettings;

	public SubscriptionHeaderFilterService(
		IOptions<AppSettings> appSettings,
		ICurrentMemberService currentMemberService,
		ISubscriptionValidationService subscriptionValidationService)
		: base(currentMemberService, subscriptionValidationService)
	{
		this._appSettings = appSettings.Value;
	}

	public void OnResultExecuted(ResultExecutedContext context)
	{
		// Do nothing
	}

	public void OnResultExecuting(ResultExecutingContext context)
	{
		// Header value must be a string.
		string subscriptionIsValid = "false";

		if (this._appSettings != null
			&& this.CurrentMemberAccount != null)
		{
			if (this.IsSubscriptionValid)
			{
				subscriptionIsValid = "true";
			}
		}

		// Add the subscription status to the response header.
		// This is then used on the client side to show a modal to select a subscription option.
		context.HttpContext.Response.Headers.Add(this._appSettings.CustomHeaders.SubscriptionStatusHeader, subscriptionIsValid);
	}
}