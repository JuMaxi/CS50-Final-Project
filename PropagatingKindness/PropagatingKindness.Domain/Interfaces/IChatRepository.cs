using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces;

public interface IChatRepository
{
    Task<List<Chat>> GetChats(int userId);
    Task<Chat> GetChatWithMessages(int chatId);
    Task<Chat> GetByUserAdvert(int userId, int advertId);
    Task Add(Chat chat);
    Task Update(Chat chat);

}
