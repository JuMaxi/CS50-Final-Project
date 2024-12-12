using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagatingKindness.Domain.Models
{
    public enum MessageStatus
    {
        Sent = 1,
        Delivered = 2,
        Read = 3,
    }
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public MessageStatus Status { get; set; }
        public User From {  get; set; }
        public User To { get; set; }
    }
}
