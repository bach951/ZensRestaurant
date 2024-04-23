using Repository.DBContext;
using Repository.Repositories;
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

        public UnitOfWork(ZRDbContext dbContext)
        {
            if(this._dbContext == null)
            {
                this._dbContext = dbContext;
            }
            
        }
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
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
    }
}
