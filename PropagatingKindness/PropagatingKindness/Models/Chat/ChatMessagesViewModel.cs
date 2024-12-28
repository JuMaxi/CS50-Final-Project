using PropagatingKindness.Domain.Models;
using PropagatingKindness.Models.Advert;

namespace PropagatingKindness.Models.Chat;

public class ChatMessagesViewModel
{
    public int Id { get; set; }
    public ViewAdvertViewModel Advert { get; set; }
    public List<ChatMessage> Messages { get; set; }

    public static ChatMessagesViewModel FromChat(Domain.Models.Chat chat, int userId)
    {
        return new ChatMessagesViewModel()
        {
            Id = chat.Id,
            Advert = ViewAdvertViewModel.FromAdvert(chat.Advert),
            Messages = chat.Messages.OrderBy(x => x.Date).Select(x => ChatMessage.FromMessage(x, userId)).ToList(),
        };
    }
}

public class ChatMessage
{
    public int Status { get; set; }
    public DateTime DateTime { get; set; }
    public string Message { get; set; }

    public bool IsSent { get; set; }
    public bool IsReceived => !IsSent;
    public string UserPhoto { get; set; }

    public static ChatMessage FromMessage(Message message, int userId)
    {
        return new ChatMessage() 
        { 
            DateTime = message.Date,
            IsSent = message.From.Id == userId,
            Message = message.Text,
            Status = (int)message.Status,
            UserPhoto = message.From.Photo,
        };
    }
}
