using System.IO;
using System.Threading.Tasks;

namespace UploaderSample.StorageProviders
{
    internal class FileSystemStorageProvider : IStorageProvider
    {
        private readonly FileSystemStorageSettings settings;

        public FileSystemStorageProvider(FileSystemStorageSettings settings)
        {
            this.settings = settings;
        }

        public async Task SaveAsync(string path, Stream stream)
        {
            var fullPath = Path.Combine(settings.StorageFolder, path);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

            stream.Position = 0;
            await stream.CopyToAsync(outputStream);

            outputStream.Close();
        }

        public Task<Stream> ReadAsync(string path)
        {
            var fullPath = Path.Combine(settings.StorageFolder, path);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            var stream = File.OpenRead(fullPath);
            return Task.FromResult(stream as Stream);
        }

        public Task DeleteAsync(string path)
        {
            var fullPath = Path.Combine(settings.StorageFolder, path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
