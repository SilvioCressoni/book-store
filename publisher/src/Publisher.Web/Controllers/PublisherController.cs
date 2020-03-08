using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Contracts.Request;
using Publisher.Application.Operations;
using Publisher.Domain;
using Publisher.Domain.Common;

namespace Publisher.Web.Controllers
{
    [ApiController]
    [Route("api/publisher")]
    public class PublisherController : ControllerBase  
    {
        [HttpGet]
        public void Get()
        {

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id, [FromServices]PublisherGetByIdOperation operation)
        {
            var result = await operation.ExecuteAsync(new PublisherGetById(id));

            if(result is OkResult<Publish> ok)
            {
                return Ok(ok.Value);
            }

            if (result is ErrorResult error && error == DomainError.PublisherError.NotFound)
            {

                return NotFound();
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult>  AddAsync([FromBody]PublisherAdd publisher, [FromServices]PublisherAddOperation operation)
        {
            var result = await operation.ExecuteAsync(publisher);

            if (result is OkResult<Publish> ok)
            {
                return Ok(ok.Value);
            }

            return UnprocessableEntity(result);
        }

        [HttpPut("{id}")]
        public void Edit([FromRoute]Guid id)
        {

        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute]Guid id)
        {

        }
    }
}
