using System.Text.Json;
using UploadArquivosAzure.Application.Interfaces;
using UploadArquivosAzure.Domain.Models;
using UploadArquivosAzure.Infrastructure.Settings;

namespace UploadArquivosAzure.Application.Services
{
    public class BackupService : IBackupService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IAzureStorageRepository _azureRepository;
        private readonly BackupSettings _settings;

        public BackupService(IFileRepository fileRepository, IAzureStorageRepository azureRepository, BackupSettings settings)
        {
            _fileRepository = fileRepository;
            _azureRepository = azureRepository;
            _settings = settings;
        }

        public async Task ExecutarAsync()
        {
            
            var json = await File.ReadAllTextAsync(_settings.ConfigFilePath);

            List<BackupConfig> configs;

            if (string.IsNullOrWhiteSpace(json))
            {
                configs = new List<BackupConfig>();
            }
            else
            {
                try
                {
                    configs = JsonSerializer.Deserialize<List<BackupConfig>>(json)
                              ?? new List<BackupConfig>();
                }
                catch (JsonException)
                {
                    // JSON inválido (mal formatado)
                    configs = new List<BackupConfig>();
                    // aqui você pode logar o erro se quiser
                }
            }
            //var configs = JsonSerializer.Deserialize<List<BackupConfig>>(json)
            //              ?? new List<BackupConfig>();

            foreach (var config in configs)
            {
                if (!_fileRepository.DiretorioExiste(config.CaminhoLocal))
                    continue;

                var arquivos = _fileRepository.ObterArquivos(config.CaminhoLocal);

                foreach (var arquivo in arquivos)
                {
                    await _azureRepository.UploadAsync(
                        arquivo,
                        config.AccountName,
                        config.AccountKey,
                        config.ContainerName);

                    _fileRepository.Deletar(arquivo);
                }
            }
        }
    }
}
