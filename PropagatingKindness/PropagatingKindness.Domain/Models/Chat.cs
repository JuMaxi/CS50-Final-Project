namespace PropagatingKindness.Domain.Models;

public class Chat
{
    public int Id { get; set; }
    public User FromUser { get; set; }
    public User ToUser { get; set; }
    public Advert Advert { get; set; }
    public DateTime LastUpdate { get; set; }
    public List<Message> Messages { get; set; }
}
