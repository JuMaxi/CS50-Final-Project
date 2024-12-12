using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagatingKindness.Domain.Models
{
    public enum AdvertStatus
    {
        Available = 1, 
        Promissed = 2,
        Donated = 3,
        Inactive = 4
    }

    public class Advert
    {
        public int Id {  get; set; }
        public User UserId { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public AdvertStatus Status { get; set; }
        
    }
}
