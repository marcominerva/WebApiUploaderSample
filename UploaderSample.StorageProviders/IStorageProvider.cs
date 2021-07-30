using System.IO;
using System.Threading.Tasks;

namespace UploaderSample.StorageProviders
{
    public interface IStorageProvider
    {
        Task SaveAsync(string path, Stream stream);

        Task<Stream> ReadAsync(string path);

        Task DeleteAsync(string path);
    }
}
