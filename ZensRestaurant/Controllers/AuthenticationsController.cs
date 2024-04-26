using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ZensRestaurant.Controllers
{

    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private IOptions<JWTAuth> _jwtAuthOptions;
        public AuthenticationsController(IAuthenticationService authenticationService, IOptions<JWTAuth> jwtAuthOptions)
        {
            _authenticationService = authenticationService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        #region Login API
        /// <summary>
        /// Login to access into the system by your account.
        /// </summary>
        /// <param name="account">
        /// Account object contains Email property and Password property. 
        /// Notice that the password must be hashed with MD5 algorithm before sending to Login API.
        /// </param>
        /// <returns>
        /// An Object with a json format that contains Account Id, Email, Role name, acesstoken
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         POST 
        ///         {
        ///             "email": "abc@gmail.com"
        ///             "password": "********"
        ///         }
        /// </remarks>
        /// <response code="200">Login Successfully.</response>
        /// <response code="400">Some Error about request data and logic data.</response>
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [HttpPost("/api/v1/auth/login")]
        public async Task<IActionResult> PostLoginAsync([FromBody] AccountRequest account)
        {
            try
            {
                AccountResponse accountResponse = await this._authenticationService.LoginAsync(account, this._jwtAuthOptions.Value);
                return Ok(accountResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        #endregion

        #region Re-GenerateTokens API
        /// <summary>
        /// Re-generate pair token from the old pair token that are provided by the ZenRestaurant system before.
        /// </summary>
        /// <param name="accountToken">
        /// AccountTokenRequest Object contains access token property and refresh token property.
        /// </param>
        /// <returns>
        /// The new pair token (Access token, Refresh token) to continue access into the ZenRestaurant system.
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         POST 
        ///         {
        ///             "accessToken": "abcxyz"
        ///             "refreshToken": "klmnopq"
        ///         }
        /// </remarks>
        /// <response code="200">Re-Generate Token Successfully.</response>
        /// <response code="400">Some Error about request data and logic data.</response>
        [ProducesResponseType(typeof(AccountTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("/api/v1/auth/regenerate-token")]
        public async Task<IActionResult> PostReGenerateTokensAsync([FromBody] AccountTokenRequest accountToken)
        {
            try
            {
                AccountTokenResponse accountTokenResponse = await this._authenticationService.ReGenerateTokensAsync(accountToken, this._jwtAuthOptions.Value);
                return Ok(accountTokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Forgot password API
        /// <summary>
        /// Get password from email of user when user forgot email.
        /// </summary>
        /// <param name="forgotPasswordRequest">
        /// email of user
        /// </param>
        /// <returns>
        /// Message: Password already send to your email.
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         POST 
        ///         {
        ///             "email": "lexuanbach952001@gmail.com"
        ///         }
        /// </remarks>
        /// <response code="200"> Password already send to your email.</response>
        /// <response code="400">Some Error about request data and logic data.</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("/api/v1/auth/forgot-password")]
        public async Task<IActionResult> PostForgotPasswordAsync([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                await _authenticationService.ForgetPassword(forgotPasswordRequest.Email);
                return Ok("Password already send to your email");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
    }
}
