using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IUserService
    {
        public Task Insert(UserDTO user);
    }
}
