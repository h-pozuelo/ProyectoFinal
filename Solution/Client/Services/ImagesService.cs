using Microsoft.AspNetCore.Components.Forms;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;

namespace Client.Services
{
    public class ImagesService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string apiUri;

        public ImagesService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiUri = _configuration.GetSection("ApiSettings")["BaseUri"]!;
        }

        public async Task<ResponseDto<string>> UploadImage(IBrowserFile image, string containerName, string id)
        {
            using (var content = new MultipartFormDataContent())
            {
                using (var stream = new StreamContent(image.OpenReadStream(maxAllowedSize: long.MaxValue)))
                {
                    stream.Headers.ContentType = MediaTypeHeaderValue.Parse(image.ContentType);

                    content.Add(stream, "image", image.Name);

                    var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUri}Images/UploadImage");

                    request.Content = content;

                    request.Headers.Add("containerName", containerName);
                    request.Headers.Add("id", id);

                    var response = await _httpClient.SendAsync(request);

                    var result = await Deserialize<ResponseDto<string>>
                        .DeserializeJson(response);

                    return result;
                }
            }
        }

        public async Task<ResponseDto<IEnumerable<string>>> GetImages(string containerName, string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUri}Images/GetImages");

            request.Headers.Add("containerName", containerName);
            request.Headers.Add("id", id);

            var response = await _httpClient.SendAsync(request);

            var result = await Deserialize<ResponseDto<IEnumerable<string>>>
                .DeserializeJson(response);

            return result;
        }
    }
}
