using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Shared.Services;
using Shared.DataTransferObjects;

namespace Server.Controllers
{

    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlojamientosController : ControllerBase
    {
        private readonly IAlojamientoService _alojamientoService;

        public AlojamientosController(IAlojamientoService alojamientoService)
        {
            _alojamientoService = alojamientoService;
        }


        [HttpGet("GetAlojamiento/{id}")]
        public async Task<IActionResult> GetAlojamiento(int id)
        {
            var result = await _alojamientoService.GetAlojamiento(id);
            return Ok(result);
        }


        [HttpGet("GetAllAlojamientos")]
        public async Task<IActionResult> GetAllAlojamientos()
        {
            var result = await _alojamientoService.GetAllAlojamientos();
            return Ok(result);
        }

        [HttpGet("GetAlojamientosByUser/{id}")]
        public async Task<IActionResult> GetAlojamientosByUser(string id)
        {
            var result = await _alojamientoService.GetAlojamientosByUser(id);
            return Ok(result);
        }

        [HttpPost("CreateAlojamiento")]
        public async Task<IActionResult> CreateAlojamiento([FromBody] AlojamientoDto alojamiento)
        {
            if (alojamiento == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _alojamientoService.CreateAlojamiento(alojamiento);
            return Ok(result);
        }


        [HttpPut("UpdateAlojamiento")]
        public async Task<IActionResult> UpdateAlojamiento([FromBody] AlojamientoDto alojamiento)
        {
            if (alojamiento == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _alojamientoService.UpdateAlojamiento(alojamiento);
            return Ok(result);
        }


        [HttpDelete("DeleteAlojamiento/{id}")]
        public async Task<IActionResult> DeleteAlojamiento(int id)
        {
            await _alojamientoService.DeleteAlojamiento(id);
            return Ok();
        }
    }
}
