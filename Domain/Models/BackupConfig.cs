namespace UploadArquivosAzure.Domain.Models
{
    public class BackupConfig
    {
        public string CaminhoLocal { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountKey { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
    }
}
