using Microsoft.Extensions.DependencyInjection;
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
            _authenticationService = _scope.ServiceProvider.GetService<IAuthenticationService>();
        }
        public AuthenticationServiceUnitTest()
        {
            // This constructor is necessary for NUnit to initialize the test fixture
        }

        #region Login
        [Test]
        public async Task Login_Success_ShouldReturnAccountResponse()
        {
            // Arrange: prepare the data to test
            AccountRequest accountRequest = new AccountRequest
            {
                Email = "ngokhong@gmail.com",
                Password = "4297f44b13955235245b2497399d7a93"
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
                Email = "ngokhong.com",
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
                Email = "tamtang@gmail.com",
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
                Email = "ngokhong@gmail.com",
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
        #endregion

        #region Regenerate Token
        [Test]
        public async Task RegenerateToken_Success_ShouldReturnAccountTokenResponse()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new AccountTokenRequest
            {
                AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5cBMaHqMMKJZwCpexB6jiYbHwB_tNK2n0wCOjK4CzxmtBIJ0ex0x-fdzUYB9jdMg26SQEDUPa1lSM6pPa5JVTuFG_T71rgZI2BK0hk7HmnykV7VHbu0Ow2SGNsHJnrRjQ",
                RefreshToken = "l9uoHp6x5ePaLeFO4OitWGmnozM/rVuJA37hZ8eFp44="
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
                AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9." +
                "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwi" +
                "aWF0IjoxNTE2MjM5MDIyfQ.ASsBwkXGQXNVlDz2kpE2_VoRpzoXsw" +
                "MfbJ_pEf9ZoTBmykDfTPyzCxBDBxR1znMzFi4p0pep5jptF9a0rPFXtUAA" +
                "8MtZheTI51N55_yYiFQnzAc8aXBlVMGz7G7nF5WJrzzUGSyG1dHcIuAvjC" +
                "mLjgjP2i_-lQbCcNhu3XEPE",
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
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                "eyJzdWIiOiJiYXRnaW9pQGdtYWlsLmNvbSIsImVtYWlsIjoiYmF0Z" +
                "2lvaUBnbWFpbC5jb20iLCJzaWQiOiI2IiwiUm9sZSI6IkN1c3" +
                "RvbWVyIiwianRpIjoiZDM2MTQxNzYtNTI5Zi00ZGE3LWEzY2MtZ" +
                "TY4YjlkMTI4OGM0IiwibmJmIjoxNzE0NzI5Nzc4LCJleHAiOjE3Mj" +
                "MzNjk3NzgsImlhdCI6MTcxNDcyOTc3OH0.it_E_bHAhuEoTlBr7X--" +
                "kX2Tsr1VpTjXYF4jejpkx90",
                RefreshToken = "sylg16rhZWi9ko3fpUaSsj/Iod9HLboBiygFiLK2EIM="
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
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                "eyJzdWIiOiJiYXRnaW9pQGdtYWlsLmNvbSIsImVtYWlsIjoiYmF0Z" +
                "2lvaUBnbWFpbC5jb20iLCJzaWQiOiI2IiwiUm9sZSI6IkN1c3" +
                "RvbWVyIiwianRpIjoiZDM2MTQxNzYtNTI5Zi00ZGE3LWEzY2MtZ" +
                "TY4YjlkMTI4OGM0IiwibmJmIjoxNzE0NzI5Nzc4LCJleHAiOjE3Mj" +
                "MzNjk3NzgsImlhdCI6MTcxNDcyOTc3OH0.it_E_bHAhuEoTlBr7X--" +
                "kX2Tsr1VpTjXYF4jejpkx90",
                RefreshToken = "sylg16rhZWi9ko3fpUaS"
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
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                ".eyJzdWIiOiJiYXRnaW9pQGdtYWlsLmNvbSIsImVtYWlsIjoiYmF" +
                "0Z2lvaUBnbWFpbC5jb20iLCJzaWQiOiI2IiwiUm9sZSI6IkN1c" +
                "3RvbWVyIiwianRpIjoiNmE1ZDdmZjEtMDExYy00ZWEzLThjNjMtZ" +
                "DhkNTM1NWY1YjUzIiwibmJmIjoxNzE0NzMxMDc1LCJleHAiOjE3" +
                "MTQ3MzEwODUsImlhdCI6MTcxNDczMTA3NX0.q-_o6_bojEjHwtNt-" +
                "cKueBUQr7tE0IUttGk4yADh-4c",
                RefreshToken = "l9uoHp6x5ePaLeFO4OitWGmnozM/rVuJA37hZ8eFp44="
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
            ClassicAssert.AreEqual("Accesstoken doesn't match", exception.Message);
        }

        [Test]
        public async Task ReGenerateToken_RefreshTokenExpired_ShouldReturnException()
        {
            // Arrange: prepare the data to test
            AccountTokenRequest accountTokenRequest = new()
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e" +
                "yJzdWIiOiJiYXRnaW9pQGdtYWlsLmNvbSIsImVtYWlsIjoiYmF0Z2l" +
                "vaUBnbWFpbC5jb20iLCJzaWQiOiI2IiwiUm9sZSI6IkN1c3RvbWVyI" +
                "iwianRpIjoiMzYxMDhmM2UtNzNhOC00ZDNjLWJhZDgtOThiZDdkODhh" +
                "MzQ3IiwibmJmIjoxNzE0NzMzMTQwLCJleHAiOjE3MTQ3MzMxNTAsImlhd" +
                "CI6MTcxNDczMzE0MH0.J3FRCKkbUjtcQR1Gdp_IU0HMBr__G8Tl-aYKYeux5mg",
                RefreshToken = "eL4pUln+km6uLylQrVya4yYtgfDBCMwGS9pGftmxqJ0="
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
        #endregion

        #region Forget password
        [Test]
        public async Task ForgetPassword_Success_ShouldReturnSuccessMessage()
        {
            // Arrange: prepare the data to test
            string email = "lexuanbach952001@gmail.com";
            // Act: test
            await _authenticationService.ForgetPassword(email);
            // Assert: compare the actual result and expected result
            Assert.Pass("Password already send to your email");
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
        #endregion

        #region Change password
        [Test]
        public async Task ChangePassword_Success_ShouldReturnSuccessMessage()
        {
            // Arrange: prepare the data to test

            ChangePasswordRequest changePasswordRequest = new()
            {
                Email = "lexuanbach952001@gmail.com",
                OldPassword = "682d0d3978cd616b2f61e03333cea11a",
                NewPassword = "bachle123",
                ConfirmPassword = "bachle123"
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
                ConfirmPassword = "bachle123"
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
                ConfirmPassword = "bachle123"
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
                ConfirmPassword = "bachle123"
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
                ConfirmPassword = ""
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
        #endregion
    }
}
