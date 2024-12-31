using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAdvertRepository _advertRepository;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IAdvertRepository advertRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _advertRepository = advertRepository;
    }
    public async Task<Result> CreateChat(int userId, int advertId)
    {
        var existing = await _chatRepository.GetByUserAdvert(userId, advertId);
        if (existing != null)
        {
            // User has a chat with this advert already, just update it and return
            existing.LastUpdate = DateTime.UtcNow;
            await _chatRepository.Update(existing);
            return Result.OK;
        }

        var advert = await _advertRepository.GetById(advertId);
        if (advert is null)
        {
            return Result.Fail("Advert does not exists");
        }

        List<AdvertStatus> allowedStatus = [AdvertStatus.Available, AdvertStatus.Promissed];
        if (!allowedStatus.Contains(advert.Status))
        {
            return Result.Fail("Advert status does not allow conversation.");
        }

        if (advert.User.Id == userId)
        {
            return Result.Fail("User can't start a conversation with himself");
        }

        var user = await _userRepository.GetById(userId);
        Chat chat = new() 
        {
            FromUser = user,
            ToUser = advert.User,
            Advert = advert,
            LastUpdate = DateTime.UtcNow,
            Messages = []
        };
        await _chatRepository.Add(chat);
        return Result.OK;   
    }

    public async Task<Result<Chat>> GetChat(int chatId)
    {
        var chat = await _chatRepository.GetChatWithMessages(chatId);
        if (chat is null)
            return new Result<Chat>(false, "Chat not found");

        return new Result<Chat>(chat);
    }

    public async Task<List<Chat>> GetChats(int userId)
    {
        return await _chatRepository.GetChats(userId);
    }

    public async Task<Result<Message>> SendMessage(int userId, int chatId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return Result<Message>.Fail("Message is empty");

        var chat = await _chatRepository.GetChatWithMessages(chatId);
        if (userId != chat.FromUser.Id && userId != chat.ToUser.Id)
        {
            return Result<Message>.Fail("Chat does not belog to the user");
        }

        var user = await _userRepository.GetById(userId);
        if (user is null)
            return Result<Message>.Fail("User not found");

        chat.AddMessage(user, message);
        await _chatRepository.Update(chat);
        return new Result<Message>(chat.Messages.Last());
    }
}
