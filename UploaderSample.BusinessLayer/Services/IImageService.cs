using System.Threading.Tasks;
using UploaderSample.BusinessLayer.Models;

namespace UploaderSample.BusinessLayer.Services
{
    public interface IImageService
    {
        Task UploadAsync(StreamFileContent content);
    }
}
