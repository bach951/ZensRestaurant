using Microsoft.IdentityModel.Tokens;
using Repository.Infrastructures;
using Repository.Models;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private UnitOfWork _unitOfWork;
        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }



        public async Task<AccountResponse> LoginAsync(AccountRequest accountRequest, JWTAuth jwtAuth)
        {
            try
            {
                User user = await this._unitOfWork.UserRepository.GetAccountAsync(accountRequest.Email);
                if (user == null)
                {
                    throw new Exception("Email doesn't exist in the database"); ;
                }
                if (user != null
                    && user.Status == 0)
                {
                    throw new Exception("Account is disabled"); ;
                }
                if (user != null && user.Password.Equals(accountRequest.Password) == false)
                {
                    throw new Exception("Password in valid"); ;
                }
                AccountResponse accountResponse = new AccountResponse
                {
                    AccountId = user.Id,
                    Email = user.Email,
                    RoleName = user.Role
                };
                accountResponse = GenerateToken(accountResponse, jwtAuth);
                return accountResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public AccountResponse GenerateToken(AccountResponse accountResponse, JWTAuth jwtAuth)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, accountResponse.Email),
                        new Claim(JwtRegisteredClaimNames.Email, accountResponse.Email),
                        new Claim(JwtRegisteredClaimNames.Sid, accountResponse.AccountId.ToString()),
                        new Claim("Role", accountResponse.RoleName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                string accessToken = jwtTokenHandler.WriteToken(token);
                accountResponse.AccessToken = accessToken;
                return accountResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
