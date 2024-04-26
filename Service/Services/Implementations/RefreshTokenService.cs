using Repository.Infrastructures;
using Repository.Models;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        UnitOfWork _unitOfWork;
        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork?)unitOfWork;
        }

        public async Task<RefreshToken> getRefreshToken(string refreshToken)
        {
            return await _unitOfWork.RefreshTokenRepository.GetRefreshTokenAsync(refreshToken);
        }
    }
}
