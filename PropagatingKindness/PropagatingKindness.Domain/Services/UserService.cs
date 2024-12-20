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

        public async Task<LoginResult> Authenticate(string login, string password)
        {
            const string errorMessage = "Invalid username/password combination. Please try again.";

            var user = await GetByEmail(login);
            if (user == null)
                return new LoginResult(errorMessage);

            if (!HashingHelper.ValidateHashWithSalt(user.Password, password))
                return new LoginResult(errorMessage);

            return new LoginResult(UserDTO.FromUser(user));
        }

        private async Task<Result> ValidateFields(UserDTO userDTO)
        {
            // Usuario colocou uma data de nascimento no futuro?
            if (userDTO.Birthday > DateOnly.FromDateTime(DateTime.Today))
                return new Result(false, "Birthday cannot be in the future.");

            // Usuario nao pode ter mais de 120 anos
            if ((DateTime.Today.Year - userDTO.Birthday.Year) > 120)
                return new Result(false, "User is more than 120 years old, please verify.");

            // Usuario tem que ser maior de 18
            if ((DateTime.Today.Year - userDTO.Birthday.Year) < 18)
                return new Result(false, "For safety reasons, users must be 18 years old or more.");

            return new Result(true, string.Empty);
        }

        public async Task<Result> CreateAccount(UserDTO userDTO)
        {
            var result = await ValidateFields(userDTO);
            if (!result.Success)
            {
                return result;
            }

            if (await GetByEmail(userDTO.Email) is not null)
                return new Result(false, "This email is already registered.");

            // Calcular um Hash
            string hash = HashingHelper.CalculateHashWithSalt(userDTO.Password);

            // Convert the UserDTO to User
            User user = new()
            {
                Name = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email.ToLower(),
                Password = hash,
                Birthday = userDTO.Birthday,
                PostCode = userDTO.PostCode,
                Photo = userDTO.Photo,
            };

            // Save the User
            await _userRepository.Insert(user);

            return new Result(true, string.Empty);
        }

        private async Task<User> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<Result> Update(UserDTO userDTO)
        {
            var result = await ValidateFields(userDTO);
            if (!result.Success)
            {
                return result;
            }

            // Check if email hasn't changed
            var userByEmail = await GetByEmail(userDTO.Email);
            if (userByEmail != null)
            {
                // Check if the userID is the same.
                if (userByEmail.Id != userDTO.Id)
                { 
                    return new Result(false, "This email is already in use by other account.");
                }
            }

            User toUpdate = await _userRepository.GetById(userDTO.Id);

            toUpdate.Name = userDTO.FirstName;
            toUpdate.LastName = userDTO.LastName;
            toUpdate.Birthday = userDTO.Birthday;
            toUpdate.Email = userDTO.Email;

            await _userRepository.Update(toUpdate);

            return result;
        }
        public async Task<Result> UpdatePassword(UserDTO userDTO, string newPassword, string confirmationPassword)
        {
            User toUpdate = await _userRepository.GetById(userDTO.Id);

            if (!HashingHelper.ValidateHashWithSalt(toUpdate.Password, userDTO.Password))
            {
                return new Result(false, "Invalid password.");
            }

            if (newPassword != confirmationPassword) 
            {
                return new Result(false, "The new password and the password confirmation(repeat) aren't the same.");
            }

            // Calcular um Hash
            string hash = HashingHelper.CalculateHashWithSalt(newPassword);

            toUpdate.Password = hash;

            await _userRepository.Update(toUpdate);

            return new Result(true, string.Empty);
        }
    }
}
