﻿using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<Result> CreateAccount(UserDTO user);

        public Task<LoginResult> Authenticate(string login, string password);
        public Task<User> GetById(int id);
    }
}
