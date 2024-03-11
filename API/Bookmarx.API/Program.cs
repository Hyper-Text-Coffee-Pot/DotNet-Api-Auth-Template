internal class Program
{
	private static void Main(string[] args)
	{
		// Serilog Logging instance added before anything else.
		// https://github.com/serilog/serilog-aspnetcore#two-stage-initialization
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.CreateBootstrapLogger();

		try
		{
			Log.Information("API is starting up.");

			string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

			var builder = WebApplication.CreateBuilder(args);

			// Wire up Serilog after appSettings are available.
			// https://github.com/serilog/serilog-sinks-file
			builder.Host.UseSerilog((context, services, configuration) => configuration
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services));

			// Default ASP.NET DI - Register all custom dependencies in a single location.
			builder.Services.RegisterCustomDependencies(builder.Configuration);

			// Register automapper.
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// Set the global Stripe payments Standard Secret API Key (from appsettings).
			StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Payments:StripeAPIKeys:StandardSecretKey");

			// Overriding the new camelCase setting in .NET Core 3.
			// https://stackoverflow.com/questions/38202039/json-properties-now-lower-case-on-swap-from-asp-net-core-1-0-0-rc2-final-to-1-0#answer-58187836
			builder.Services.AddControllers()
				   .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

			// Add services to the container.
			// TODO: Verify if this is needed
			//builder.Services.AddControllers();

			// Add versioning configuration.
			builder.Services.AddApiVersioning(options =>
			{
				// Reporting api versions will return the headers
				// "api-supported-versions" and "api-deprecated-versions."
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(1, 0);
			}).AddMvc(options =>
			{
				// Automatically applies an api version based on the name of
				// the defining controller's namespace.
				options.Conventions.Add(new VersionByNamespaceConvention());
			});

			RegisterCorsSettings(MyAllowSpecificOrigins, builder);

			// Add services to the container.
			// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0
			builder.Services.AddHttpClient();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen();

			// Map the appsettings.json file to the AppSettings class.
			builder.Services.Configure<AppSettings>(builder.Configuration);

			RegisterAuthMiddleware(builder);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// See RegisterCorsSettings for details.
			app.UseCors(MyAllowSpecificOrigins);

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, "Application terminated unexpectedly");
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static void RegisterAuthMiddleware(WebApplicationBuilder builder)
	{
		// Required so that we can access IHttpContextAccessor from any of our custom classes or libraries.
		// This allows us to then access the HttpContext from anywhere using DI.
		// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-7.0
		builder.Services.AddHttpContextAccessor();

		// Custom authorization handler
		// https://stackoverflow.com/questions/59328439/error-while-validating-the-service-descriptor-servicetype-inewsrepository-life
		builder.Services.AddTransient<IAuthorizationHandler, AuthHandler>();

		// Custom filter injection.
		// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0#servicefilterattribute
		builder.Services.AddScoped<SubscriptionHeaderFilterService>();

		// Authorization settings
		// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0
		// [Authorize(Policy = ApiAuthPolicy.ActiveSessionAuthorization)] - Add to any controller that should be protected by this policy
		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy(ApiAuthPolicy.ActiveSessionAuthorization, policy =>
			{
				policy.Requirements.Add(new AccessTokenRequirement());
			});
		});

		// Custom Claims with 3rd party auth providers (e.g. Google)
		// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims?view=aspnetcore-6.0
		// Using Firebase Auth for authentication provider.
		// https://firebase.google.com/docs/admin/setup
		FirebaseApp.Create(new AppOptions()
		{
			Credential = GoogleCredential.GetApplicationDefault(),
		});
	}

	private static void RegisterCorsSettings(string MyAllowSpecificOrigins, WebApplicationBuilder builder)
	{
		// Custom CORS settings
		// https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
		builder.Services.AddCors(options =>
		{
			options.AddPolicy(name: MyAllowSpecificOrigins, options =>
			{
				options.WithOrigins("http://localhost:4200",
									"https://localhost:4200",
									"https://a.stripecdn.com", // All potential stripe FQDNs
									"https://api.stripe.com",
									"https://atlas.stripe.com",
									"https://auth.stripe.com",
									"https://b.stripecdn.com",
									"https://billing.stripe.com",
									"https://buy.stripe.com",
									"https://c.stripecdn.com",
									"https://checkout.stripe.com",
									"https://climate.stripe.com",
									"https://connect.stripe.com",
									"https://dashboard.stripe.com",
									"https://express.stripe.com",
									"https://files.stripe.com",
									"https://hooks.stripe.com",
									"https://invoice.stripe.com",
									"https://invoicedata.stripe.com",
									"https://js.stripe.com",
									"https://m.stripe.com",
									"https://m.stripe.network",
									"https://manage.stripe.com",
									"https://pay.stripe.com",
									"https://payments.stripe.com",
									"https://q.stripe.com",
									"https://qr.stripe.com",
									"https://r.stripe.com",
									"https://verify.stripe.com",
									"https://stripe.com",
									"https://terminal.stripe.com",
									"https://uploads.stripe.com")
				.WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin")
				.WithHeaders(HeaderNames.Authorization, "Authorization")
				.WithHeaders(builder.Configuration.GetValue<string>("CustomHeaders:AuthUserIDHeader"))
				.WithHeaders(builder.Configuration.GetValue<string>("CustomHeaders:StripeWebhookHeader"))
				.WithExposedHeaders(builder.Configuration.GetValue<string>("CustomHeaders:SubscriptionStatusHeader"))
				.AllowAnyHeader()
				.WithMethods("POST", "PUT", "GET", "DELETE", "HEAD", "PATCH");
			});
		});
	}
}