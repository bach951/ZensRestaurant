using Repository.Infrastructures;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<RefreshToken> getRefreshToken(string refreshToken);

    }
}
