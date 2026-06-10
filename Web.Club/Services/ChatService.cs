using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IChatService
{
}

public class ChatService(ApiHttpClient api) : IChatService
{
    
}