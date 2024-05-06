using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ZensRestaurantUnitTest.ServiceUnitTest
{
    public class UserServiceUnitTest
    {
        private IUserService _userService;
        private JWTAuth _jWTAuth;
        private DependencyInjection dependencyInjection = new DependencyInjection();
        private ServiceProvider _provider;
        private IServiceScope _scope;
        [OneTimeSetUp]
        public void SetUp()
        {
            _jWTAuth = new JWTAuth
            {
                Key = "ZensRestaurant2024LeXuanBach2001"
            };
            _provider = dependencyInjection.provider;
            _scope = _provider.CreateScope();
            _userService = _scope.ServiceProvider.GetService<IUserService>();
        }

        [Test]
        public async Task GetUserInformation_Success_ShouldReturnUserInformation()
        {
            // Arrange: prepare the data to test
            List<Claim> claims = new List<Claim>
        {
            new Claim("sid", "1"),
        };
            // Act: test
            var response = await _userService.GetUserInformation(1, claims);

            // Assert: compare the actual result and expected result
            ClassicAssert.IsNotNull(response);
        }

        [Test]
        public async Task GetUserInformation_AccessToOrtherId_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            List<Claim> claims = new List<Claim>
        {
            new Claim("sid", "1"),
        };
            // Act: test
            Exception exception = null;
            try
            {
                var response = await _userService.GetUserInformation(2, claims);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("You can't acess to orther id", exception.Message);
        }

        [Test]
        public async Task GetUserInformation_IdLessThan0_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            List<Claim> claims = new List<Claim>
        {
            new Claim("sid", "1"),
        };
            // Act: test
            Exception exception = null;
            try
            {
                var response = await _userService.GetUserInformation(-2, claims);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Id need to greater than 0", exception.Message);
        }

        [Test]
        public async Task GetUserInformation_IdDoNotExistInTheDatabase_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            List<Claim> claims = new List<Claim>
        {
            new Claim("sid", "1"),
        };
            // Act: test
            Exception exception = null;
            try
            {
                var response = await _userService.GetUserInformation(100, claims);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Id doesn't exist in the system", exception.Message);
        }
    }
}
