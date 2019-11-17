using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Contracts.Request;
using Users.Application.Operations;
using Users.Domain;
using Users.Web.Contracts;


namespace Users.Web.Controllers
{
    [Route("/api/v1/user")]
    public class UserController : ControllerBase
    {
        #region User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user, [FromServices] UserCreateOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserAdd
            {
                FirstName = user.FirstName,
                LastNames = user.LastNames,
                BirthDay = user.BirthDay,
                Email = user.Email
            });

            if (result is OkResult<Application.Contracts.Response.User> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid id, [FromBody] User user, [FromServices] UserUpdateOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserUpdate
            {
                Id = id,
                FirstName = user.FirstName,
                LastNames = user.LastNames,
                BirthDay = user.BirthDay
            });

            if (result is OkResult<Application.Contracts.Response.User> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetUser([FromRoute]Guid id, [FromServices] UserGetOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserGet
            {
                Id = id
            });

            if (result is OkResult<Application.Contracts.Response.User> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery]int? skip, [FromQuery]int? take, [FromServices] UserGetAllOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserGetAll
            {
                Skip = skip ?? 0,
                Take =  take ?? 0
            });

            if (result is OkResult<IEnumerable<Application.Contracts.Response.User>> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        #endregion

        #region Phone
        [HttpPost("/{id}/phone")]
        public async Task<IActionResult> AddPhone([FromRoute]Guid id, [FromBody] Phone phone, [FromServices] PhoneAddOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneAdd
            {
                UserId = id,
                Number = phone.Number
            });

            if (result is OkResult<Application.Contracts.Response.Phone> ok)
            {
                return Ok(new Phone
                {
                    Number = ok.Value.Number
                });
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpDelete("/{id}/phone")]
        public async Task<IActionResult> DeletePhone([FromRoute]Guid id, [FromBody] Phone phone, [FromServices] PhoneRemoveOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneRemove
            {
                UserId = id,
                Number = phone.Number
            });

            if (result is Domain.OkResult)
            {
                return NoContent();
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        #endregion

        #region Address
        [HttpPost("/{id}/address")]
        public async Task<IActionResult> AddAddress([FromRoute]Guid id, [FromBody] Phone phone, [FromServices] PhoneAddOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneAdd
            {
                UserId = id,
                Number = phone.Number
            });

            if (result is OkResult<PhoneAdd> ok)
            {
                return Ok(new Phone
                {
                    Number = ok.Value.Number
                });
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpDelete("/{id}/address")]
        public async Task<IActionResult> DeleteAddress([FromRoute]Guid id, [FromBody] Phone phone, [FromServices] PhoneRemoveOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneRemove
            {
                UserId = id,
                Number = phone.Number
            });

            if (result is Domain.OkResult)
            {
                return NoContent();
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        #endregion
    }
}
