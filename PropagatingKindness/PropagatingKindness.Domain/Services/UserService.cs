using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;


namespace PropagatingKindness.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Insert(UserDTO user)
        {
            // Calcular um Hash
            string hash = CalculateHash(user.Password);

            // Verificar email existe
            if (GetByEmail(user.Email) is null) 
            {
                throw new Exception();
            }

            // Convert the UserDTO to User


            // Save the User
            await _userRepository.Insert(null);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public string CalculateHash(string password)
        {
            //byte[] data = Convert.FromBase64String(password);
            var data = Encoding.UTF8.GetBytes(password);
            byte[] hashedData = SHA512.HashData(data);
            return Convert.ToBase64String(hashedData);
        }
    }
}
