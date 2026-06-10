using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IMemberService
{
}

public class MemberService(ApiHttpClient api) : IMemberService
{
    
}