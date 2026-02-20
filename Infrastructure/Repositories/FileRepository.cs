using UploadArquivosAzure.Application.Interfaces;

namespace UploadArquivosAzure.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        public IEnumerable<string> ObterArquivos(string caminho)
        {
            return Directory.GetFiles(caminho);
        }
        /// <summary>
        /// Não é para deletar o caminho do arquivo, mais o arquivo que está dentro do caminho.
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        public void Deletar(string caminhoArquivo)
        {
            File.Delete(caminhoArquivo);
        }

        public bool DiretorioExiste(string caminho)
        {
            return Directory.Exists(caminho);
        }
    }
}
