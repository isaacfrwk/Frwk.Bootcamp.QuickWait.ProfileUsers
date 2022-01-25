using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.FindAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _service.AddAsync(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            await _service.UpdateAsync(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] User user)
        {
            await _service.DeleteAsync(user);
            return Ok();
        }
    }
}
