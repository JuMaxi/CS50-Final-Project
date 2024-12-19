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

            User to_Update = await _userRepository.GetById(userDTO.Id);

            to_Update.Name = userDTO.FirstName;
            to_Update.LastName = userDTO.LastName;
            to_Update.Photo = userDTO.Photo;
            to_Update.Birthday = userDTO.Birthday;
            to_Update.Email = userDTO.Email;

            await _userRepository.Update(to_Update);

            return result;
        }
    }
}
