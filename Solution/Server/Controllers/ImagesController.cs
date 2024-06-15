using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image, [FromHeader] string containerName, [FromHeader] string id)
        {
            var result = await _imagesService.UploadImage(image, containerName, id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpGet("GetImages")]
        public async Task<IActionResult> GetImages([FromHeader] string containerName, [FromHeader] string id)
        {
            var result = await _imagesService.GetImages(containerName, id);

            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
