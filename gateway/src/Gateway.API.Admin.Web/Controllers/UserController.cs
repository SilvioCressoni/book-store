using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Gateway.API.Admin.Web.Contracts.Request;
using Gateway.API.Admin.Web.Contracts.Response;
using Gateway.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.API.Admin.Web.Controllers
{
    [Route("api/v1/users")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : AbstractController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Users
        /// <summary>
        /// GET User by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Found User</returns>
        /// <response code="200">Return user</response>
        /// <response code="400">Invalid id</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute]Guid id)
        {
            _logger.LogInformation("Getting user");

            try
            {
                var result = await _service.GetAsync(id.ToString())
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A news user created</returns>
        /// <response code="201">Return user</response>
        /// <response code="400">Invalid user parameters</response>
        /// <response code="422">When there is some business error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest user)
        {
            _logger.LogInformation("Getting user");

            try
            {
                var result = await _service.AddUserAsync(user.Email, user.FirstName, user.LastNames, user.BirthDate)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        /// <summary>
        /// Update a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A user updated</returns>
        /// <response code="204">User updated</response>
        /// <response code="400">Invalid user parameters</response>
        /// <response code="422">When there is some bussiness error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<User>),StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid id, [FromBody] UpdateUserRequest user)
        {
            _logger.LogInformation("Getting user");

            try
            {
                var result = await _service.UpdateUserAsync(id.ToString(), user.FirstName, user.LastNames, user.BirthDate)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        #endregion

        #region Phone

        /// <summary>
        /// GET Phone by user Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Found User</returns>
        /// <response code="200">Return user phones</response>
        /// <response code="400">Invalid id</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}/phones")]
        [ProducesResponseType(typeof(Result<IEnumerable<Phone>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<Phone>>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<IEnumerable<Phone>>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPhones([FromRoute]Guid id)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.GetPhonesAsync(id.ToString())
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        /// <summary>
        /// Create a new user phone
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="phone">Phone</param>
        /// <returns>A news phone created</returns>
        /// <response code="201">Return phone</response>
        /// <response code="400">Invalid user phone parameters</response>
        /// <response code="404">User not found</response>
        /// <response code="422">When there is some business error</response>
        [HttpPost("{id}/phones")]
        [ProducesResponseType(typeof(Result<Phone>),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<Phone>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<Phone>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<Phone>),StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPhone([FromRoute]Guid id, [FromBody]AddPhoneRequest phone)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.AddPhoneAsync(id.ToString(), phone.Number)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        /// <summary>
        /// Delete user phone
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="number">Phone number</param>
        /// <returns>Delete phone</returns>
        /// 
        [HttpDelete("{id}/phones/{number}")]
        [ProducesResponseType(typeof(Result),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Result),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result),StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemovePhone([FromRoute]Guid id, [FromRoute]string number)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.RemovePhoneAsync(id.ToString(), number)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }

        #endregion
        
        #region Address

        [HttpGet("{id}/addresses")]
        public async Task<IActionResult> GetAddresses([FromRoute]string id)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.GetAddressesAsync(id)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        [HttpPost("{id}/addresses")]
        public async Task<IActionResult> AddAddresses([FromRoute]string id, [FromBody]AddAddressRequest address)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.AddAddressAsync(id, address.Line, address.Number, address.PostCode)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        [HttpDelete("{id}/addresses/{addressId}")]
        public async Task<IActionResult> RemoveAddress([FromRoute]string id, [FromRoute]string addressId)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.RemoveAddressAsync(id, addressId)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }

        #endregion
    }
}
