using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using Shared.Services;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlojamientosController : ControllerBase
    {
        private readonly IAlojamientosService _alojamientosService;

        public AlojamientosController(IAlojamientosService alojamientosService)
        {
            _alojamientosService = alojamientosService;
        }

        [HttpGet("GetAllAlojamientos")]
        public async Task<IActionResult> GetAllAlojamientos()
        {
            var result = await _alojamientosService.GetAllAlojamientos();

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpGet("GetAlojamiento/{id}")]
        public async Task<IActionResult> GetAlojamiento(int id)
        {
            var result = await _alojamientosService.GetAlojamiento(id);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpPost("CreateAlojamiento")]
        public async Task<IActionResult> CreateAlojamiento([FromBody] AlojamientoDto alojamiento)
        {
            var result = await _alojamientosService.CreateAlojamiento(alojamiento);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpPut("UpdateAlojamiento")]
        public async Task<IActionResult> UpdateAlojamiento([FromBody] AlojamientoDto alojamiento)
        {
            var result = await _alojamientosService.UpdateAlojamiento(alojamiento);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpDelete("DeleteAlojamiento/{id}")]
        public async Task<IActionResult> DeleteAlojamiento(int id)
        {
            var result = await _alojamientosService.DeleteAlojamiento(id);

            return StatusCode((int)result.StatusCode);
        }

        [HttpGet("GetAlojamientosByUser/{id}")]
        public async Task<IActionResult> GetAllAlojamientosPropietario(string id)
        {
            var result = await _alojamientosService.GetAllAlojamientosPropietario(id);

            return StatusCode((int)result.StatusCode, result.Element);
        }
    }
}
