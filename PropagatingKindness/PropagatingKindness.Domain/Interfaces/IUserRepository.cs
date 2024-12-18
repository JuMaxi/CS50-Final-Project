using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task Insert(User user);
        public Task<User?> GetByEmail(string email);
        public Task<User> GetById(int id);
    }
}
