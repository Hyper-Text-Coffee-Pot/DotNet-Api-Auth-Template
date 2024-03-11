namespace Bookmarx.Shared.v1.Membership;

/// <summary>
/// Super awesome automapper setup
/// https://code-maze.com/automapper-net-core/
/// Use the AppDomain.CurrentDomain.GetAssemblies() from the following link to work properly in Core 3.1
/// https://procodeguide.com/programming/automapper-in-aspnet-core/
/// </summary>
public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		// Domain object always on the left (think lower, or left side) UI or response object always on the right
		this.CreateMap<MemberAccountDto, MemberAccount>();
	}
}