using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Repository.Infrastructures;
using Repository.Models;
using Service.DTOs.Users;
using Service.Services.Interfaces;
using Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class UserService : IUserService
    {
        private UnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task<UserResponse> GetUserInformation(int id, IEnumerable<Claim> claims)
        {
            try
            {
                Claim registeredIdClaim = claims.FirstOrDefault(x => x.Type.Equals("sid"));
                int idClaim = int.Parse(registeredIdClaim.Value);
                if (idClaim != id)
                {
                    throw new Exception("You can't acess to orther id");
                }
                if (id <= 0)
                {
                    throw new Exception("Id need to greater than 0");
                }
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    throw new Exception("Id doesn't exist in the system");
                }
                var userResponse = new UserResponse();
                if (user.Gender == true)
                {
                    userResponse.Gender = "Male";
                }
                else
                {
                    userResponse.Gender = "Female";
                }

                if (user.Status == 1)
                {
                    userResponse.Status = "Active";
                }
                else
                {
                    userResponse.Status = "Inactive";
                }

                userResponse.DOB = user.DOB;
                userResponse.Avatar = user.Avatar;
                userResponse.Address = user.Address;
                userResponse.Email = user.Email;
                userResponse.Id = user.Id;
                userResponse.Role = user.Role;
                return userResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RegisterAnAccount(UserRegisterRequest userRegisterRequest)
        {
            try
            {
                var userEmail = await _unitOfWork.UserRepository.GetAccountAsync(userRegisterRequest.Email);
                if (userEmail != null)
                {
                    throw new Exception("Email already exist in the database");
                }
                string patternGmail = @"@gmail\.com$";

                if (userRegisterRequest.UserName.Length > 100)
                {
                    throw new Exception("UserName must be less than 100 character");
                }

                if (userRegisterRequest.Email.Length > 100)
                {
                    throw new Exception("Email must be less than 100 character");

                }

                if (!Regex.IsMatch(userRegisterRequest.Email, patternGmail))
                {
                    throw new Exception("Email need end with @gmail.com");
                }

                if (userRegisterRequest.Password.Length > 50)
                {
                    throw new Exception("Password must be less than 50 character");
                }
                // change normal password to md5 hash
                string md5Password = "";
                if (userRegisterRequest != null && userRegisterRequest.Password != null)
                {
                    md5Password = StringToMD5.GetMD5Hash(userRegisterRequest.Password);
                }
                User user = new User
                {
                    UserName = userRegisterRequest.UserName,
                    DOB = DateTime.Now.Date,
                    Email = userRegisterRequest.Email,
                    Gender = true,
                    Password = md5Password,
                    Role = "Customer",
                    Status = 1
                };
                await _unitOfWork.UserRepository.RegisterAnAccount(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
