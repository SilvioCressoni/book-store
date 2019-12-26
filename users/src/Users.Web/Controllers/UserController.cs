using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Contracts;
using Users.Application.Contracts.Request;
using Users.Application.Operations;
using Users.Domain;

namespace Users.Web.Controllers
{
    [Route("/api/v1/user")]
    public class UserController : ControllerBase
    {
        #region User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] User user, [FromServices] UserCreateOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserAdd
            {
                FirstName = user.FirstName,
                LastNames = user.LastNames,
                BirthDate = user.BirthDay,
                Email = user.Email
            });

            if (result is OkResult<User> ok)
            {
                return Created("",ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid id, [FromBody] User user, [FromServices] UserUpdateOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserUpdate
            {
                Id = id,
                FirstName = user.FirstName,
                LastNames = user.LastNames,
                BirthDay = user.BirthDay
            });

            if (result is OkResult<User> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUser([FromRoute]Guid id, [FromServices] UserGetOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserGet
            {
                Id = id
            });

            if (result is OkResult<User> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAllUser([FromQuery]int? skip, [FromQuery]int? take, [FromServices] UserGetAllOperation operation)
        {
            var result = await operation.ExecuteAsync(new UserGetAll
            {
                Skip = skip ?? 0,
                Take =  take ?? 0
            });

            if (result is OkResult<IEnumerable<User>> ok)
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
        
        [HttpGet("{id}/phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPhone([FromRoute]Guid id, [FromServices] PhoneGetOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneGet()
            {
                UserId = id,
            });

            if (result is OkResult<IEnumerable<Phone>> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        
        [HttpPost("{id}/phone")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPhone([FromRoute]Guid id, [FromBody] Phone phone, [FromServices] PhoneAddOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneAdd
            {
                UserId = id,
                Number = phone.Number
            });

            if (result is OkResult<Phone> ok)
            {
                return Ok(new Phone
                {
                    Number = ok.Value.Number
                });
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}/phone/{number}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeletePhone([FromRoute]Guid id, [FromRoute] string number, [FromServices] PhoneRemoveOperation operation)
        {
            var result = await operation.ExecuteAsync(new PhoneRemove
            {
                UserId = id,
                Number = number
            });

            if (result is Domain.OkResult)
            {
                return NoContent();
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        #endregion

        #region Address
        [HttpGet("{id}/address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAddress([FromRoute]Guid id, [FromServices] AddressGetOperation operation)
        {
            var result = await operation.ExecuteAsync(new AddressGet
            {
                UserId = id,
            });

            if (result is OkResult<IEnumerable<Address>> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        
        [HttpPost("{id}/address")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddAddress([FromRoute]Guid id, [FromBody] Address address, [FromServices] AddressAddOperation operation)
        {
            var result = await operation.ExecuteAsync(new AddressAdd
            {
                UserId = id,
                Number = address.Number,
                Line = address.Line,
                PostCode = address.PostCode
            });

            if (result is OkResult<Address> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}/address/{addressId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteAddress([FromRoute]Guid id, [FromBody] Guid addressId, [FromServices] AddressRemoveOperation operation)
        {
            var result = await operation.ExecuteAsync(new AddressRemove
            {
                UserId = id,
                Id = addressId
            });
            
            if (result is Domain.OkResult)
            {
                return NoContent();
            }

            if (result is ErrorResult error && error.ErrorCode.StartsWith("USR"))
            {
                if (error.ErrorCode == DomainError.UserError.UserNotFound.ErrorCode)
                {
                    return NotFound(error);
                }
                
                return UnprocessableEntity(error);
            }

            return BadRequest(result);
        }
        #endregion
    }
}
