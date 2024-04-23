using Service.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task RegisterAnAccount(UserRegisterRequest userRegisterRequest);
        public Task<UserResponse> GetUserInformation(int id);
    }
}
