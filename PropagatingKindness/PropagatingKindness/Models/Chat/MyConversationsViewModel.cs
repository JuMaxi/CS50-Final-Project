namespace PropagatingKindness.Models.Chat;

public class MyConversationsViewModel
{
    public List<Converstion> Conversations { get; set; }

    public static MyConversationsViewModel FromChats(List<Domain.Models.Chat> chats, int userId)
    {
        return new MyConversationsViewModel()
        {
            Conversations = chats.Select(c => Converstion.FromChat(c, userId)).ToList()
        };
    }
}

public class Converstion
{
    public int Id { get; set; }
    public DateTime LastUpdate { get; set; }
    public string UserPhoto { get; set; }
    public string UserName { get; set; }

    public static Converstion FromChat(Domain.Models.Chat chat, int userId)
    {
        var result = new Converstion
        {
            Id = chat.Id,
            LastUpdate = chat.LastUpdate,
        };

        if (chat.FromUser.Id == userId)
        {
            result.UserPhoto = chat.ToUser.Photo;
            result.UserName = chat.ToUser.Name;
        }
        else
        {
            result.UserPhoto = chat.FromUser.Photo;
            result.UserName = chat.FromUser.Name;
        }
        return result;
    }
}
