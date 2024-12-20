using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<Result> CreateAccount(UserDTO user);

        public Task<LoginResult> Authenticate(string login, string password);
        public Task<User> GetById(int id);
        public Task<Result> Update(UserDTO userDTO);
        public Task<Result> UpdatePassword(UserDTO userDTO, string newPassword, string confirmationPassword);
    }
}
