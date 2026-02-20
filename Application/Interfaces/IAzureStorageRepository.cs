namespace UploadArquivosAzure.Application.Interfaces
{
    public interface IAzureStorageRepository
    {
        Task UploadAsync(string filePath, string accountName, string accountKey, string containerName);
    }
}
