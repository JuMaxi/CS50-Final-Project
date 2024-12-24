using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using PropagatingKindness.Configuration;
using SkiaSharp;

namespace PropagatingKindness.Services;

public interface IPhotosManagerService
{
    Task<string> ResizeAndUpload(IFormFile file, int maxWidth, int maxHeight, string blobContainer, string fileName = "");
}

// Thanks ChatGPT for this code
public class PhotosManagerService : IPhotosManagerService
{
    private IOptions<AzureConfiguration> _configuration;

    public PhotosManagerService(IOptions<AzureConfiguration> configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> ResizeAndUpload(IFormFile file, int maxWidth, int maxHeight, string blobContainer, string fileName = "")
    {
        ArgumentNullException.ThrowIfNull(nameof(file));

        var resizedStream = Resize(file.OpenReadStream(), maxWidth, maxHeight);
        resizedStream.Position = 0;
        string uploadedFile = await Upload(resizedStream, blobContainer, fileName);
        return uploadedFile;
    }

    private async Task<string> Upload(Stream stream, string blobContainer, string fileName)
    {
        string storageAccountUrl = _configuration.Value.StorageAccountURL;
        string blobName = string.IsNullOrEmpty(fileName) ? GetRandomFileName() : fileName;

        var clientSecretCredential = new ClientSecretCredential(_configuration.Value.tenantId, _configuration.Value.clientId, _configuration.Value.clientSecret);

        var blobServiceClient = new BlobServiceClient(new Uri(storageAccountUrl), clientSecretCredential);

        var containerClient = blobServiceClient.GetBlobContainerClient(blobContainer);
        var blobClient = containerClient.GetBlobClient(blobName);
        var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg", ContentDisposition = "inline" };

        using (var fileStream = stream)
        {
            var result = await blobClient.UploadAsync(fileStream, new BlobUploadOptions() 
            {
                HttpHeaders = blobHttpHeader
            });
        }

        return blobClient.Uri.AbsoluteUri;
    }

    private Stream Resize(Stream fileStream, int maxWidth, int maxHeight)
    {
        using (var inputStream = fileStream)
        using (var original = SKBitmap.Decode(inputStream))
        {
            var ratioX = (double)maxWidth / original.Width;
            var ratioY = (double)maxHeight / original.Height;
            var ratio = Math.Min(ratioX, ratioY);

            int finalWidth = (int)(original.Width * ratio);
            int finalHeight = (int)(original.Height * ratio);

            // Resize the image
            using (var resizedBitmap = original.Resize(new SKImageInfo(finalWidth, finalHeight), SKFilterQuality.High))
            {
                if (resizedBitmap == null)
                {
                    throw new Exception("Image resizing failed.");
                }

                // Save the resized image
                using (var image = SKImage.FromBitmap(resizedBitmap))
                {
                    var outputStream = new MemoryStream();
                    image.Encode(SKEncodedImageFormat.Jpeg, 85).SaveTo(outputStream);
                    return outputStream;
                }
            }
        }
    }

    private string GetRandomFileName() => Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
}
