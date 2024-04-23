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
        Task<AccountResponse> LoginAsync(AccountRequest accountRequest, JWTAuth jwtAuth);
        AccountResponse GenerateToken(AccountResponse accountResponse, JWTAuth jwtAuth);

    }
}
