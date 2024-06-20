using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.DataTransferObjects;
using Shared.Services;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/Alquiler")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        private readonly IAlquileresService _alquileresService;

        public AlquileresController(IAlquileresService alquileresService)
        {
            _alquileresService = alquileresService;
        }

        [HttpGet("GetAllAlquileres")]
        public async Task<IActionResult> GetAllAlquileres()
        {
            var result = await _alquileresService.GetAllAlquileres();

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpGet("GetAlquiler/{id}")]
        public async Task<IActionResult> GetAlquiler(int id)
        {
            var result = await _alquileresService.GetAlquiler(id);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpPost("CreateAlquiler")]
        public async Task<IActionResult> CreateAlquiler([FromBody] AlquilerDto alquiler)
        {
            var result = await _alquileresService.CreateAlquiler(alquiler);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpPut("UpdateAlquiler")]
        public async Task<IActionResult> UpdateAlquiler([FromBody] AlquilerDto alquiler)
        {
            var result = await _alquileresService.UpdateAlquiler(alquiler);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpDelete("DeleteAlquiler/{id}")]
        public async Task<IActionResult> DeleteAlquiler(int id)
        {
            var result = await _alquileresService.DeleteAlquiler(id);

            return StatusCode((int)result.StatusCode);
        }

        [HttpGet("GetAlquileresByAlojamiento/{id}")]
        public async Task<IActionResult> GetAllAlquileresAlojamiento(int id)
        {
            var result = await _alquileresService.GetAllAlquileresAlojamiento(id);

            return StatusCode((int)result.StatusCode, result.Element);
        }

        [HttpGet("GetAlquileresByUser/{id}")]
        public async Task<IActionResult> GetAllAlquileresInquilino(string id)
        {
            var result = await _alquileresService.GetAllAlquileresInquilino(id);

            return StatusCode((int)result.StatusCode, result.Element);
        }
    }
}
