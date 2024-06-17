using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Authorize]
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

        [HttpGet("GetUsuario")]
        public async Task<IActionResult> GetUsuario([FromHeader] string id)
        {
            var result = await _usuariosService.GetUsuario(id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario([FromBody] UserForUpdateDto model, [FromHeader] string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdateUsuario(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordForUpdateDto model, [FromHeader] string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdatePassword(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetNombreUsuario/{idUsuario}")]
        public async Task<IActionResult> GetNombreUsuario(string idUsuario)
        {
            var result = await _usuariosService.GetNombreUsuario(idUsuario);

            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
