using Microsoft.Extensions.DependencyInjection;
using UploadArquivosAzure.Application.Interfaces;
using UploadArquivosAzure.Application.Services;
using UploadArquivosAzure.Infrastructure.Repositories;
using UploadArquivosAzure.Infrastructure.Settings;

var services = new ServiceCollection();

services.AddSingleton(new BackupSettings
{
    ConfigFilePath = @"D:\RotinaBackupAzure\rotinaBackupAzure.json"
});

services.AddScoped<IFileRepository, FileRepository>();
services.AddScoped<IAzureStorageRepository, AzureBlobStorageRepository>();
services.AddScoped<IBackupService, BackupService>();

var provider = services.BuildServiceProvider();

var backupService = provider.GetRequiredService<IBackupService>();
await backupService.ExecutarAsync();
