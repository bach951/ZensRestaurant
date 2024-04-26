using NUnit.Framework;
using Repository.Infrastructures;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZensRestaurantUnitTest.ServiceUnitTest
{
    [TestFixture]
    public class LoginTest
    {
        private UnitOfWork _unitOfWork;
        private IAuthenticationService _authenticationService;
        [SetUp]
        public void Setup(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _authenticationService = authenticationService;
        }

        AccountRequest accountRequest = new AccountRequest
        {
            Email = "bachle@gmail.com",
            Password = "c4ca4238a0b923820dcc509a6f75849b"
        };
        JWTAuth jWTAuth = new JWTAuth
        {
            Key = "ZensRestaurant2024LeXuanBach2001"
        };
        [Test]
        public void LoginSuccess()
        {
            _authenticationService.LoginAsync(accountRequest, jWTAuth);
            Assert.Pass();
        }
    }
}
