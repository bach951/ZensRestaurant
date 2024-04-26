using Microsoft.IdentityModel.Tokens;
using Repository.Infrastructures;
using Repository.Models;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.DTOs.Users;
using Service.Services.Interfaces;
using Service.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
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
                string patternGmail = @"@gmail\.com$";


                if (!Regex.IsMatch(accountRequest.Email, patternGmail))
                {
                    throw new Exception("Email need end with @gmail.com");
                }

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
                accountResponse = await GenerateTokensAsync(accountResponse, jwtAuth);
                return accountResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<AccountResponse> GenerateTokensAsync(AccountResponse accountResponse, JWTAuth jwtAuth)
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
                    Expires = DateTime.UtcNow.AddSeconds(10),
                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                string accessToken = jwtTokenHandler.WriteToken(token);
                string refreshToken = GenerateRefreshToken();
                accountResponse.AccessToken = accessToken;
                accountResponse.RefreshToken = refreshToken;

                RefreshToken refreshTokenDB = new RefreshToken
                {
                    JWTId = token.Id,
                    Token = refreshToken,
                    ExpiredDate = DateTime.UtcNow.AddDays(5),
                    User = await _unitOfWork.UserRepository.GetUserByIdAsync(accountResponse.AccountId)
                };
                await this._unitOfWork.RefreshTokenRepository.AddRefreshTokenAsync(refreshTokenDB);
                await this._unitOfWork.CommitAsync();
                return accountResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AccountTokenResponse> ReGenerateTokensAsync(AccountTokenRequest accountTokenRequest, JWTAuth jwtAuth)
        {

            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyBytes = Encoding.UTF8.GetBytes(jwtAuth.Key);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };


                //Check 1: Access token is valid format
                var tokenVerification = jwtTokenHandler.ValidateToken(accountTokenRequest.AccessToken, tokenValidationParameters, out var validatedToken);

                //Check 2: Check Alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        throw new Exception("Invalid accesstoken");
                    }
                }

                //Check 3: check accessToken expried?
                var utcExpiredDate = long.Parse(tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiredDate = DateUtil.ConvertUnixTimeToDateTime(utcExpiredDate);
                if (expiredDate > DateTime.UtcNow)
                {
                    throw new Exception("accesstoken not expired yet");
                }

                //Check 4: Check refresh token exist in Redis Db
                string accountId = tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
                var refreshToken = await this._unitOfWork.RefreshTokenRepository.GetRefreshTokenAsync(accountTokenRequest.RefreshToken);
                if (refreshToken == null)
                {
                    throw new Exception("Token doesn't not exist in the database");
                }

                if (refreshToken.Token.Equals(accountTokenRequest.RefreshToken) == false)
                {
                    throw new Exception("Refresh Token doesn't exist in the database");
                }

                //Check 5: Id of refresh token == id of access token
                var jwtId = tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (refreshToken.JWTId.Equals(jwtId) == false)
                {
                    throw new Exception("accesstoken doesn't match");
                }

                //Check 6: refresh token is expired
                if (refreshToken.ExpiredDate < DateTime.UtcNow)
                {
                    throw new Exception("Refresh token expired");
                }
                User existedUser = await this._unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(accountId));
                AccountResponse accountResponse = new AccountResponse
                {
                    AccountId = existedUser.Id,
                    Email = existedUser.Email,
                    RoleName = existedUser.Role
                };
                accountResponse = await GenerateTokensAsync(accountResponse, jwtAuth);
                return new AccountTokenResponse
                {
                    AccessToken = accountResponse.AccessToken,
                    RefreshToken = accountResponse.RefreshToken,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task ForgetPassword(string email)
        {
            try
            {
                string patternGmail = @"@gmail\.com$";
                if (!Regex.IsMatch(email, patternGmail))
                {
                    throw new Exception("Email need end with @gmail.com");
                }

                User user = await this._unitOfWork.UserRepository.GetAccountAsync(email);
                if (user == null)
                {
                    throw new Exception("Email doesn't exist in the database"); ;
                }
                string messageBody = "This is your password";
                string password = Ramdom6Util.GenerateRandomString();
                string encrypPassword = StringToMD5.GetMD5Hash(password);
                user.Password = encrypPassword;
                string message = this._unitOfWork.EmailRepository.GetMessageForgotAccount(email, password, messageBody);
                await this._unitOfWork.EmailRepository.SendAccountToEmailAsync(email, message);
                this._unitOfWork.UserRepository.UpdateUserAccount(user);
                this._unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
