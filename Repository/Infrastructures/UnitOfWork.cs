using Repository.DBContext;
using Repository.Repositories;
using Repository.SMTPs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private ZRDbContext _dbContext;
        private UserRepository _userRepository;
        private RefreshTokenRepository _refreshTokenRepository;
        private EmailRepository _emailRepository;



        public UnitOfWork(ZRDbContext dbContext)
        {
            if (this._dbContext == null)
            {
                this._dbContext = dbContext;
            }

        }
        public void Commit()
        {
            this._dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }

        public UserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(this._dbContext);
                }
                return this._userRepository;
            }
        }

        public RefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (this._refreshTokenRepository == null)
                {
                    this._refreshTokenRepository = new RefreshTokenRepository(this._dbContext);
                }
                return this._refreshTokenRepository;
            }
        }

        public EmailRepository EmailRepository
        {
            get
            {
                if (this._emailRepository == null)
                {
                    this._emailRepository = new EmailRepository(this._dbContext);
                }
                return this._emailRepository;
            }
        }
    }
}
