using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserRepository
    {
        private ZRDbContext _dbContext;
        public UserRepository(ZRDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<User> GetAccountAsync(string email)
        {
            try
            {
                var user = await this._dbContext.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await this._dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RegisterAnAccount(User user)
        {
            try
            {
                await this._dbContext.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateUserAccount(User user)
        {
            try
            {
                this._dbContext.Users.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
