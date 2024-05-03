using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.DBContext;
using Repository.Infrastructures;
using Service.DTOs.JWTs;
using Service.DTOs.Users;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZensRestaurantUnitTest
{
    public class DependencyInjection
    {
        private ServiceCollection _services;
        public ServiceProvider provider { get; private set; }
        public DependencyInjection()
        {
            _services = new ServiceCollection();

            string connectionString = $"Server=tcp:bachle-sqlserver.database.windows.net,1433;Database=zenstestdb;"
                                           + $"User ID=bachbeobo;"
                                           + $"Password=xuanbach2001*;";

            _services.AddDbContext<ZRDbContext>(options
          => options.UseSqlServer(connectionString));
            //DI UnitOfWork
            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            // DI Service
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<IAuthenticationService, AuthenticationService>();

            provider = _services.BuildServiceProvider();

            var context = provider.GetService<ZRDbContext>();

            context.Database.Migrate();
        }
    }

}
