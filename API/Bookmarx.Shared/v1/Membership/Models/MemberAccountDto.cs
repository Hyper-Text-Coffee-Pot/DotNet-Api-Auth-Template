namespace Bookmarx.Shared.v1.Membership.Models;

public class MemberAccountDto
{
	public Guid? AccountGuid { get; set; }

	public string AuthProviderUID { get; set; }

	public string EmailAddress { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }
}