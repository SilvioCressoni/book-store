using System;
using System.Net;
using System.Threading.Tasks;
using Gateway.API.Admin.Web.Contracts.Request;
using Gateway.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.API.Admin.Web.Controllers
{
    [Route("api/v1/users")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute]string id)
        {
            _logger.LogInformation("Getting user");

            try
            {
                var result = await _service.GetAsync(id)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }

        [HttpPost]
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]string id, [FromBody] UpdateUserRequest user)
        {
            _logger.LogInformation("Getting user");

            try
            {
                var result = await _service.UpdateUserAsync(id, user.FirstName, user.LastNames, user.BirthDate)
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

        [HttpGet("{id}/phones")]
        public async Task<IActionResult> GetPhones([FromRoute]string id)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.GetPhonesAsync(id)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        [HttpPost("{id}/phones")]
        public async Task<IActionResult> AddPhone([FromRoute]string id, [FromBody]AddPhoneRequest phone)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.AddPhoneAsync(id, phone.Number)
                    .ConfigureAwait(false);
                return StatusCode(result.StatusCode, result.Value);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error to get user phone");
                return StatusCode(HttpStatusCode.ServiceUnavailable, "UserService down");
            }
        }
        
        [HttpDelete("{id}/phones/{number}")]
        public async Task<IActionResult> RemovePhone([FromRoute]string id, [FromRoute]string number)
        {
            _logger.LogInformation("Getting user phone");

            try
            {
                var result = await _service.RemovePhoneAsync(id, number)
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
