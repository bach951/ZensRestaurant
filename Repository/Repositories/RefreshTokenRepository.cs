using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RefreshTokenRepository
    {
        private ZRDbContext _dbContext;
        public RefreshTokenRepository(ZRDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await this._dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token.Equals(refreshToken));
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await this._dbContext.RefreshTokens.AddAsync(refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
