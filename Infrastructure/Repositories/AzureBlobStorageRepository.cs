using Azure.Storage;
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
            
            accountName = accountName.Trim();
            accountKey = accountKey.Trim();
            containerName = containerName.Trim();

            if (accountKey.IndexOfAny(new[] { '\r', '\n' }) >= 0)
                throw new FormatException("AccountKey contém quebras de linha. Remova quebras de linha do segredo antes de usar.");

            // Valida se accountKey é base64
            try
            {
                Convert.FromBase64String(accountKey);
            }
            catch (FormatException)
            {
                throw new FormatException("AccountKey não está no formato base64 esperado para uma Storage Account Key.");
            }

            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");
            var blobServiceClient = new BlobServiceClient(serviceUri, credential);

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

            await using var stream = File.OpenRead(filePath);
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }
}
