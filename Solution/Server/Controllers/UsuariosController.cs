using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet("GetUsuario/{id}")]
        public async Task<IActionResult> GetUsuario(string id)
        {
            var result = await _usuariosService.GetUsuario(id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdateUsuario/{id}")]
        public async Task<IActionResult> UpdateUsuario([FromBody] UserForUpdateDto model, string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdateUsuario(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordForUpdateDto model, string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdatePassword(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
