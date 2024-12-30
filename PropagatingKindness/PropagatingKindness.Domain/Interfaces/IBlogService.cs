using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropagatingKindness.Domain.DTO;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IBlogService
    {
        public Task<Result> CreatePost(BlogDTO post, int userId);
    }
}
