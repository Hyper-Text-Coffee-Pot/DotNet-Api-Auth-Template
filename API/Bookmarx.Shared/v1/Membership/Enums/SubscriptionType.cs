namespace Bookmarx.Shared.v1.Membership.Enums;

public enum SubscriptionType
{
	// This should NOT be an actual option.
	// If this shows up (see probe) you should check what is wrong and fix the plan type.
	None = 0,

	TwentyFiveGBPlan = 1,

	FiftyGBPlan = 2,

	OneHundredGBPlan = 3,

	TwoGBPlan = 4,

	TwoHundredFiftyGBPlan = 5,

	FiveHundredGBPlan = 6
}