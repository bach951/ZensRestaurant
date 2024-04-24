﻿using Microsoft.AspNetCore.Http;
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
        [HttpPost("/api/v1/authentications/login")]
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
    }
}
