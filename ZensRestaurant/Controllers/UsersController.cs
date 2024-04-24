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
        /// <param name="userRegisterRequest">
        /// userRegisterRequest object contains username, email, password properties. 
        /// </param>
        /// <returns>
        /// Message "Register sucessfully".
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         POST 
        ///         {
        ///             "userName": "Le Xuan Bach"
        ///             "email": "lexuanbach952001@gmail.com"
        ///             "password": "68687979"
        ///         }
        /// </remarks>
        /// <response code="200">Register Successfully.</response>
        /// <response code="400">Some Error about request data and logic data.</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("/api/v1/users/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest userRegisterRequest)
        {
            try
            {
                await this._userService.RegisterAnAccount(userRegisterRequest);
                return Ok(new { Message = "Register sucessfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        #endregion

        #region Get User Information
        /// <summary>
        /// Get User Information
        /// </summary>
        /// <param name="id">
        /// id of user.
        /// </param>
        /// <returns>
        /// An Object with a json format that contains Id, Email, Status, Address, Gender, Role, DOB, Avatar.
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         POST 
        ///         id: 3
        /// </remarks>
        /// <response code="200">Login Successfully.</response>
        /// <response code="400">Some Error about request data and logic data.</response>
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                return BadRequest(new { Message = ex.Message });
            }

        }
        #endregion
    }
}
