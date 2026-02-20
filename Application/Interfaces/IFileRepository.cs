namespace UploadArquivosAzure.Application.Interfaces
{
    public interface IFileRepository
    {
        IEnumerable<string> ObterArquivos(string caminho);
        void Deletar(string caminhoArquivo);
        bool DiretorioExiste(string caminho);
    }
}
