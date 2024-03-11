namespace Bookmarx.Shared.v1.Membership.Entities;

public class MemberThirdPartyProviderAccount
{
	public MemberThirdPartyProviderAccount(
		int memberAccountID,
		string thirdPartyProviderCustomerID,
		ThirdPartyProviderType thirdPartyProviderTypeID)
	{
		this.MemberAccountID = memberAccountID;
		this.ThirdPartyProviderCustomerID = thirdPartyProviderCustomerID;
		this.ThirdPartyProviderTypeID = (int)thirdPartyProviderTypeID;
	}

	private MemberThirdPartyProviderAccount()
	{
		// Needed for EF
	}

	public MemberAccount MemberAccount { get; private set; }

	public int MemberAccountID { get; private set; }

	public int MemberThirdPartyProviderAccountID { get; private set; }

	public string ThirdPartyProviderCustomerID { get; private set; }

	public int ThirdPartyProviderTypeID { get; private set; }
}