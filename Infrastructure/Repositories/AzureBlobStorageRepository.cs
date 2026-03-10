using Azure.Storage;
using Azure.Storage.Blobs;
using UploadArquivosAzure.Application.Interfaces;

namespace UploadArquivosAzure.Infrastructure.Repositories
{
    public class AzureBlobStorageRepository : IAzureStorageRepository
    {
        public async Task UploadAsync(string filePath, string accountName, string accountKey, string containerName)
        {
            accountName = accountName?.Trim() ?? throw new ArgumentNullException(nameof(accountName));
            accountKey = accountKey?.Trim() ?? throw new ArgumentNullException(nameof(accountKey));
            containerName = containerName?.Trim() ?? throw new ArgumentNullException(nameof(containerName));

            if (accountKey.IndexOfAny(new[] { '\r', '\n' }) >= 0)
                throw new FormatException("AccountKey contém quebras de linha. Remova quebras de linha do segredo antes de usar.");

            accountKey = NormalizeAndValidateBase64Key(accountKey);

            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");
            var blobServiceClient = new BlobServiceClient(serviceUri, credential);

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

            await using var stream = File.OpenRead(filePath);
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        private static string NormalizeAndValidateBase64Key(string key)
        {

            key = key.Trim();

            int mod = key.Length % 4;
            if (mod != 0)
                key = key.PadRight(key.Length + (4 - mod), '=');

            try
            {
                Convert.FromBase64String(key);
            }
            catch (FormatException ex)
            {
                throw new FormatException("AccountKey não está no formato base64 esperado para uma Storage Account Key.", ex);
            }

            return key;
        }
    }
}
