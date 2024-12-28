namespace PropagatingKindness.Domain.Models;

public class Chat
{
    public int Id { get; set; }
    public User FromUser { get; set; }
    public User ToUser { get; set; }
    public Advert Advert { get; set; }
    public DateTime LastUpdate { get; set; }
    public List<Message> Messages { get; set; }

    public void AddMessage(User user, string message)
    {
        if (FromUser.Id != user.Id && ToUser.Id != user.Id)
            return;

        var chatMessage = new Message();
        if (FromUser.Id == user.Id)
        {
            chatMessage.From = user;
            chatMessage.To = Advert.User;
        }
        else
        {
            chatMessage.To = user;
            chatMessage.From = Advert.User;
        }

        chatMessage.Chat = this;
        chatMessage.Status = MessageStatus.Delivered;
        chatMessage.Text = message;
        chatMessage.Date = DateTime.UtcNow;
        Messages.Add(chatMessage);
    }
}
