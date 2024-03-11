using AutoMapper;
using Bookmarx.API.v1.Controllers.Membership.Models;
using Bookmarx.Shared.v1.Membership.Models;

namespace Bookmarx.API.v1.Controllers.Membership;

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
		this.CreateMap<MemberAccountCreateRequest, MemberAccountDto>()
			.ForMember(mad => mad.AuthProviderUID, macr => macr.MapFrom(m => m.APID));
	}
}