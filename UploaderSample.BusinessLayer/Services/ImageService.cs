using System.Threading.Tasks;
using UploaderSample.BusinessLayer.Models;

namespace UploaderSample.BusinessLayer.Services
{
    public class ImageService : IImageService
    {
        public Task UploadAsync(StreamFileContent content)
        {
            return Task.CompletedTask;
        }
    }
}
