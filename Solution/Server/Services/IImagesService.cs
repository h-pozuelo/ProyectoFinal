using Shared.DataTransferObjects;

namespace Server.Services
{
    public interface IImagesService
    {
        public Task<ResponseDto<string>> UploadImage(IFormFile image, string containerName, string id);
        public Task<ResponseDto<IEnumerable<string>>> GetImages(string containerName, string id);
    }
}
