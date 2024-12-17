using PropagatingKindness.Domain.DTO;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<Result> CreateAccount(UserDTO user);

        public Task<LoginResult> Authenticate(string login, string password);
    }
}
