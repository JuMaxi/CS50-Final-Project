using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagatingKindness.Domain.Models
{
    public class Gardening_Help
    {
        public int Id {  get; set; }
        public User UserId { get; set; }
        public string PostCode { get; set; }
        public string Description { get; set; }

    }
}
