using Azure.Storage.Blobs;
using UploadArquivosAzure.Application.Interfaces;

namespace UploadArquivosAzure.Infrastructure.Repositories
{
    public class AzureBlobStorageRepository : IAzureStorageRepository
    {
        public async Task UploadAsync(
            string filePath,
            string accountName,
            string accountKey,
            string containerName)
        {
            var connectionString =
                $"DefaultEndpointsProtocol=https;" +
                $"AccountName={accountName};" +
                $"AccountKey={accountKey};" +
                $"EndpointSuffix=core.windows.net";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(
                Path.GetFileName(filePath));

            await blobClient.UploadAsync(filePath, overwrite: true);
        }
    }
}
