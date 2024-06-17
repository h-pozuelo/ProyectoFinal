using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Shared.Services;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly IAlquilerService _alquilerService;

        public AlquilerController(IAlquilerService alquilerService)
        {
            _alquilerService = alquilerService;
        }

        [HttpGet("GetAlquiler/{id}")]
        public async Task<IActionResult> GetAlquiler(int id)
        {
            var result = await _alquilerService.GetAlquiler(id);
            return Ok(result);
        }

        [HttpGet("GetAllAlquileres")]
        public async Task<IActionResult> GetAllAlquileres()
        {
            var result = await _alquilerService.GetAllAlquileres();
            return Ok(result);
        }


        [HttpPost("CreateAlquiler")]
        public async Task<IActionResult> CreateAlquiler([FromBody] AlquilerDto alquiler)
        {
            if (alquiler == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _alquilerService.CreateAlquiler(alquiler);
            return Ok(result);
        }

        [HttpPut("UpdateAlquiler")]
        public async Task<IActionResult> UpdateAlquiler([FromBody] AlquilerDto alquiler)
        {
            if (alquiler == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _alquilerService.UpdateAlquiler(alquiler);
            return Ok(result);
        }

        [HttpDelete("DeleteAlquiler/{id}")]
        public async Task<IActionResult> DeleteAlquiler(int id)
        {
            await _alquilerService.DeleteAlquiler(id);
            return Ok();
        }

        [HttpGet("GetAlquileresByAlojamiento/{id}")]
        public async Task<IActionResult> GetAlquileresByAlojamiento(int id)
        {
            var result = await _alquilerService.GetAlquileresByAlojamiento(id);
            return Ok(result);
        }

        [HttpGet("GetAlquileresByUser/{id}")]
        public async Task<IActionResult> GetAlquileresByUser(string id)
        {
            var result = await _alquilerService.GetAlquileresByUser(id);
            return Ok(result);
        }

    }
}
