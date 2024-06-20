using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {
        private readonly IUbicacionesService _ubicacionesService;

        public UbicacionesController(IUbicacionesService ubicacionesService)
        {
            _ubicacionesService = ubicacionesService;
        }

        [HttpGet("GetComunidades")]
        public async Task<IActionResult> GetComunidades()
        {
            var result = await _ubicacionesService.GetComunidades();

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetProvincias")]
        public async Task<IActionResult> GetProvincias()
        {
            var result = await _ubicacionesService.GetProvincias();

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetCiudades")]
        public async Task<IActionResult> GetCiudades()
        {
            var result = await _ubicacionesService.GetCiudades();

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetProvinciasComunidad")]
        public async Task<IActionResult> GetProvinciasComunidad([FromHeader] string? idComunidad)
        {
            if (string.IsNullOrEmpty(idComunidad)) return RedirectToAction(nameof(GetProvincias));

            var result = await _ubicacionesService.GetProvinciasComunidad(idComunidad);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetCiudadesProvincia")]
        public async Task<IActionResult> GetCiudadesProvincia([FromHeader] string? idProvincia)
        {
            if (string.IsNullOrEmpty(idProvincia)) return RedirectToAction(nameof(GetCiudades));

            var result = await _ubicacionesService.GetCiudadesProvincia(idProvincia);

            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
