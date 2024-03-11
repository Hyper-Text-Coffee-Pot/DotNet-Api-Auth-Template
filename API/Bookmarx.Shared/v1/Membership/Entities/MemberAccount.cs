using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmarx.Shared.v1.Membership.Entities;

public class MemberAccount
{
	public Guid AccountGuid { get; set; }

	public string AuthProviderUID { get; set; }

	public DateTime CreatedDateTimeUTC { get; set; }

	public string EmailAddress { get; set; }

	public string? FirstName { get; set; }

	[NotMapped]
	public string FullName
	{
		get
		{
			return $"{this.FirstName} {this.LastName}";
		}

		private set { }
	}

	public DateTime? LastLoginDateTimeUTC { get; set; }

	public string? LastName { get; set; }

	public int MemberAccountID { get; set; }

	public List<MemberThirdPartyProviderAccount> MemberThirdPartyProviders { get; set; } = new List<MemberThirdPartyProviderAccount>();

	public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}