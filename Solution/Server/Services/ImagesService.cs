using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shared.DataTransferObjects;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Services
{
    public class ImagesService : IImagesService
    {
        private readonly BlobServiceClient _serviceClient;

        private readonly List<string> allowedContentTypes = ["image/jpeg", "image/jpg", "image/png"];

        public ImagesService(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<ResponseDto<string>> UploadImage(IFormFile image, string containerName, string id)
        {
            ResponseDto<string> response = new ResponseDto<string>();

            try
            {
                // Comprobamos que la imagen adjuntada no sea nula ni se encuentre vacía
                if (image == null || image.Length == 0)
                {
                    response.Error = "La imagen adjuntada es nula o se encuentra vacía.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                // Si el tipo de la imagen adjuntada no se encuentra entre los tipos permitidos...
                else if (!allowedContentTypes.Contains(image.ContentType.ToLower()))
                {
                    response.Error = $"La imagen {image.FileName} no posee un formato de fichero válido: {string.Join(", ", allowedContentTypes)}.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    // Recuperamos el cliente del contenedor de "Azure Blob Storage"
                    var containerClient = _serviceClient.GetBlobContainerClient(containerName);
                    // Si el contenedor no existe lo crea con acceso público ("PublicAccessType.Blob")
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                    // Generamos un cliente del blob concatenando el id recibido como parámetro junto al nombre de la imagen
                    var client = containerClient.GetBlobClient($"{id}/{DateTimeOffset.Now.ToUnixTimeSeconds()}{image.FileName}");

                    // Subimos la imagen al "Azure Blob Storage"
                    using (var stream = image.OpenReadStream())
                    {
                        await client.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                    }

                    response.IsSuccessful = true;
                    response.Element = client.Uri.ToString();
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseDto<IEnumerable<string>>> GetImages(string containerName, string id)
        {
            ResponseDto<IEnumerable<string>> response = new ResponseDto<IEnumerable<string>> { };

            var images = new List<string>();

            try
            {
                // Recuperamos el cliente del contenedor de "Azure Blob Storage"
                var containerClient = _serviceClient.GetBlobContainerClient(containerName);

                // Por cada elemento dentro del contenedor que posea el prefijo indicado...
                // (En este caso el prefijo corresponde al directorio en donde se encuentra el elemento)
                await foreach (var item in containerClient.GetBlobsAsync(prefix: id))
                {
                    var client = containerClient.GetBlobClient(item.Name);
                    images.Add(client.Uri.ToString());
                }

                response.IsSuccessful = true;
                response.Element = images;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
