using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AccountResponse> LoginAsync(AccountRequest accountRequest, JWTAuth jwtAuth);
        public Task<AccountResponse> GenerateTokensAsync(AccountResponse accountResponse, JWTAuth jwtAuth);
        public Task<AccountTokenResponse> ReGenerateTokensAsync(AccountTokenRequest accountTokenRequest, JWTAuth jwtAuth);
        public Task ForgetPassword(string email);
        public Task ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}
