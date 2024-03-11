namespace Bookmarx.Shared.v1.Configuration.Models;

/// <summary>
/// Globally accessible object to access anything inside appsettings.{Env}.json from projects
/// outside of the root startup project. So cool.
/// https://stackoverflow.com/questions/48948905/how-to-access-appsettings-from-another-project/48951371#answer-48949231
/// </summary>
public class AppSettings
{
	public ConnectionString ConnectionStrings { get; set; }

	public CustomHeaders CustomHeaders { get; set; }

	public string Environment { get; set; }

	public GoogleAPIs GoogleAPIs { get; set; }

	public Payments Payments { get; set; }

	public PostMarkApp PostmarkApp { get; set; }

	public Products Products { get; set; }
}