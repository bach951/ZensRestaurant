using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.DTOs.Accounts;
using Service.DTOs.JWTs;
using Service.DTOs.Users;
using Service.Services.Interfaces;

namespace ZensRestaurant.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        #region Register API
        /// <summary>
        /// Register new account for user
        /// </summary>
        /// <param name="account">
        /// Account object contains Email property and Password property. 
        /// Notice that the password must be hashed with MD5 algorithm before sending to Login API.
        /// </param>
        /// <returns>
        /// An Object with a json format that contains Account Id, Email, Role name, and a pair token (access token, refresh token).
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
        /// <response code="404">Some Error about request data not found.</response>
        /// <response code="500">Some Error about the system.</response>
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]

        [HttpPost("/api/v1/users/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest userRegisterRequest)
        {
            try
            {
                await this._userService.RegisterAnAccount(userRegisterRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Get User Information
        /// <summary>
        /// Get User Information
        /// </summary>
        /// <param name="id">
        /// Account object contains Email property and Password property. 
        /// Notice that the password must be hashed with MD5 algorithm before sending to Login API.
        /// </param>
        /// <returns>
        /// An Object with a json format that contains Account Id, Email, Role name, and a pair token (access token, refresh token).
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
        /// <response code="404">Some Error about request data not found.</response>
        /// <response code="500">Some Error about the system.</response>
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]

        [HttpGet("/api/v1/user/{id}")]
        public async Task<IActionResult> GetUserInformationAsync([FromRoute] int id)
        {
            try
            {
                var userInfor = await this._userService.GetUserInformation(id);
                return Ok(userInfor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
    }
}
