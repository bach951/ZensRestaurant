﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Infrastructures
{
    public interface IUnitOfWork
    {
        public void Commit();
        public Task CommitAsync();
    }
}
