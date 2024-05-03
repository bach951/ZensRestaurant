using NUnit.Framework;
using NUnit.Framework.Legacy;
using Repository.DBContext;
using Repository.Infrastructures;
using Repository.Repositories;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZensRestaurantUnitTest.ServiceUnitTest
{
    [TestFixture]
    public class AuthenticationServiceUnitTest
    {
        private IAuthenticationService _authenticationService;
        private JWTAuth _jWTAuth;
        private IUnitOfWork _unitOfWork;
        private ZRDbContext _zRDbContext;
        public AuthenticationServiceUnitTest()
        {
            // This constructor is necessary for NUnit to initialize the test fixture
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _jWTAuth = new JWTAuth
            {
                Key = "ZensRestaurant2024LeXuanBach2001"
            };
            _zRDbContext = new ZRDbContext();
            _unitOfWork = new UnitOfWork(_zRDbContext);
            // Initialize your authentication service here if needed
            _authenticationService = new AuthenticationService(_unitOfWork); // Replace YourAuthenticationService with your actual implementation
        }

        [Test]
        public async Task Login_Success_ShouldReturnAccountResponse()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "lexuanbach952001@gmail.com",
                Password = "72b0ff31df84edcbdc79ca74a8fed313"
            };
            // Act: test
            var response = await _authenticationService.LoginAsync(accountRequest, _jWTAuth);

            // Assert: compare the actual result and expected result
            ClassicAssert.IsNotNull(response);
        }

        [Test]
        public async Task Login_WithEmailNotValid_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "lexuanbach952001.com",
                Password = "72b0ff31df84edcbdc79ca74a8fed313"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.LoginAsync(accountRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Email need end with @gmail.com", exception.Message);
        }

        [Test]
        public async Task Login_WithEmailNotExistInDatabase_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "baothanhthien@gmail.com",
                Password = "72b0ff31df84edcbdc79ca74a8fed313"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.LoginAsync(accountRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Email doesn't exist in the database", exception.Message);
        }

        [Test]
        public async Task Login_WithEmailHaveStatusDisabled_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "yg@gmail.com",
                Password = "f5bb0c8de146c67b44babbf4e6584cc0"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.LoginAsync(accountRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Account is disabled", exception.Message);
        }

        [Test]
        public async Task Login_WithPasswordInvalid_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "xuanrin1412@gmail.com",
                Password = "123123123"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.LoginAsync(accountRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Password in valid", exception.Message);
        }

        [Test]
        public async Task RegenerateToken_Success_ShouldReturnAccountTokenResponse()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new AccountTokenRequest
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYWNobGU5NTIwMDFAZ21" +
                "haWwuY29tIiwiZW1haWwiOiJiYWNobGU5NTIwMDFAZ21haWwuY29tIiwic2lkIjoiNiIsIlJvbGUiOiJDdXN0b2" +
                "1lciIsImp0aSI6IjBmNzkxYjU0LWQ0MWYtNDUzYS1hMDhkLWExYTFlZjE4OWYxZiIsIm5iZiI6MTcxNDY0MTc" +
                "4MCwiZXhwIjoxNzE0NjQxNzkwLCJpYXQiOjE3MTQ2NDE3ODB9.756bEBgcN7pdnlmB19pNqL536oQnq_vRtGAb3oHerC8",
                RefreshToken = "hrDjuA0LbWKKGEDAadealxYWwapxcGSpgoXUlmIYuQs="
            };
            // Act & Assert: test
            var response = await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(response);
        }

        [Test]
        public async Task RegenerateToken_InvalidAccessToken_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new AccountTokenRequest
            {
                AccessToken = "invalidAcessToken",
                RefreshToken = "hrDjuA0LbWKKGEDAadealxYWwapxcGSpgoXUlmIYuQs="
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Invalid accesstoken", exception.Message);
        }

        [Test]
        public async Task ReGenerateToken_AccessTokenNotExpired_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new AccountTokenRequest
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYWNobGU5NTIwMDFAZ21" +
                "haWwuY29tIiwiZW1haWwiOiJiYWNobGU5NTIwMDFAZ21haWwuY29tIiwic2lkIjoiNiIsIlJvbGUiOiJDdXN0b2" +
                "1lciIsImp0aSI6IjBmNzkxYjU0LWQ0MWYtNDUzYS1hMDhkLWExYTFlZjE4OWYxZiIsIm5iZiI6MTcxNDY0MTc" +
                "4MCwiZXhwIjoxNzE0NjQxNzkwLCJpYXQiOjE3MTQ2NDE3ODB9.756bEBgcN7pdnlmB19pNqL536oQnq_vRtGAb3oHerC8",
                RefreshToken = "hrDjuA0LbWKKGEDAadealxYWwapxcGSpgoXUlmIYuQs="
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Accesstoken not expired yet", exception.Message);
        }

        [Test]
        public async Task ReGenerateToken_RefreshTokenNotExistInDatabase_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new AccountTokenRequest
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYWNobGU5NTIwMDFAZ21" +
                "haWwuY29tIiwiZW1haWwiOiJiYWNobGU5NTIwMDFAZ21haWwuY29tIiwic2lkIjoiNiIsIlJvbGUiOiJDdXN0b2" +
                "1lciIsImp0aSI6IjBmNzkxYjU0LWQ0MWYtNDUzYS1hMDhkLWExYTFlZjE4OWYxZiIsIm5iZiI6MTcxNDY0MTc" +
                "4MCwiZXhwIjoxNzE0NjQxNzkwLCJpYXQiOjE3MTQ2NDE3ODB9.756bEBgcN7pdnlmB19pNqL536oQnq_vRtGAb3oHerC8",
                RefreshToken = "hrDjuA0LbWKKGEDAadealx"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Refresh Token doesn't exist in the database", exception.Message);
        }

        [Test]
        public async Task ReGenerateToken_AccessTokenNotMatch_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new()
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYWNobGU5NTIwMDFAZ21" +
                "haWwuY29tIiwiZW1haWwiOiJiYWNobGU5NTIwMDFAZ21haWwuY29tIiwic2lkIjoiNiIsIlJvbGUiOiJDdXN0b2" +
                "1lciIsImp0aSI6IjBmNzkxYjU0LWQ0MWYtNDUzYS1hMDhkLWExYTFlZjE4OWYxZiIsIm5iZiI6MTcxNDY0MTc" +
                "4MCwiZXhwIjoxNzE0NjQxNzkwLCJpYXQiOjE3MTQ2NDE3ODB9.756bEBgcN7pdnlmB19pNqL536oQnq_vRtGAb3oHerC8",
                RefreshToken = "hrDjuA0LbWKKGEDAadealx"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Refresh Token doesn't exist in the database", exception.Message);
        }

        [Test]
        public async Task ReGenerateToken_AccessTokenExpired_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new()
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYWNobGU5NTIwMDFAZ21" +
                "haWwuY29tIiwiZW1haWwiOiJiYWNobGU5NTIwMDFAZ21haWwuY29tIiwic2lkIjoiNiIsIlJvbGUiOiJDdXN0b2" +
                "1lciIsImp0aSI6IjBmNzkxYjU0LWQ0MWYtNDUzYS1hMDhkLWExYTFlZjE4OWYxZiIsIm5iZiI6MTcxNDY0MTc" +
                "4MCwiZXhwIjoxNzE0NjQxNzkwLCJpYXQiOjE3MTQ2NDE3ODB9.756bEBgcN7pdnlmB19pNqL536oQnq_vRtGAb3oHerC8",
                RefreshToken = "hrDjuA0LbWKKGEDAadealx"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ReGenerateTokensAsync(accountTokenRequest, _jWTAuth);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Refresh token expired", exception.Message);
        }

        [Test]
        public async Task ForgetPassword_Success_ShouldReturnSuccessMessage()
        {
            // Arrange: prepare the data to test
            string email = "lexuanbach952001@gmail.com";
            // Act: test
            await _authenticationService.ForgetPassword(email);
            // Assert: compare the actual result and expected result
            Assert.Pass();
        }

        [Test]
        public async Task ForgetPassword_WrongFormatEmail_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            string email = "quanvantruong99.com";
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ForgetPassword(email);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Email need end with @gmail.com", exception.Message);
        }

        [Test]
        public async Task ForgetPassword_EmailDoNotExistInDatabase_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            string email = "quanvantruong99@gmail.com";
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ForgetPassword(email);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Email doesn't exist in the database", exception.Message);
        }

        [Test]
        public async Task ChangePassword_Success_ShouldReturnSuccessMessage()
        {
            // Arrange: prepare the data to test

            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "lexuanbach952001@gmail.com",
                OldPassword = "682d0d3978cd616b2f61e03333cea11a",
                NewPassword = "bachle123",
                ComfirmPassword = "bachle123"
            };
            // Act & Assert: test
            await _authenticationService.ChangePassword(changePasswordRequest);
            // Assert: check if the exception is thrown and if its message is as expected
            Assert.Pass("Change password successfully");
        }

        [Test]
        public async Task ChangePassword_EmailDoNotExistInDatabase_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "quanvantruong99@gmail.com",
                OldPassword = "682d0d3978cd616b2f61e03333cea11a",
                NewPassword = "bachle123",
                ComfirmPassword = "bachle123"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ChangePassword(changePasswordRequest);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Email doesn't exist in the database", exception.Message);
        }

        [Test]
        public async Task ChangePassword_InvalidPassword_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "lexuanbach952001@gmail.com",
                OldPassword = "123123123",
                NewPassword = "bachle123",
                ComfirmPassword = "bachle123"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ChangePassword(changePasswordRequest);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Invalid password", exception.Message);
        }

        [Test]
        public async Task ChangePassword_PasswordNotMatchEachOrther_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "lexuanbach952001@gmail.com",
                OldPassword = "682d0d3978cd616b2f61e03333cea11a",
                NewPassword = "bachle",
                ComfirmPassword = "bachle123"
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ChangePassword(changePasswordRequest);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("New password and comfirm password not match each orther", exception.Message);
        }

        [Test]
        public async Task ChangePassword_PasswordNeedGreaterThan1_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "lexuanbach952001@gmail.com",
                OldPassword = "682d0d3978cd616b2f61e03333cea11a",
                NewPassword = "",
                ComfirmPassword = ""
            };
            // Act & Assert: test
            Exception exception = null;
            try
            {
                await _authenticationService.ChangePassword(changePasswordRequest);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            // Assert: check if the exception is thrown and if its message is as expected
            ClassicAssert.IsNotNull(exception);
            ClassicAssert.AreEqual("Password need to greater than 1 character", exception.Message);
        }
    }
}
