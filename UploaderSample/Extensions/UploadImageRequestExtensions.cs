using UploaderSample.BusinessLayer.Models;
using UploaderSample.Models;

namespace UploaderSample.Extensions
{
    public static class UploadImageRequestExtensions
    {
        public static StreamFileContent ToStreamFileContent(this UploadImageRequest request)
            => new(request.File.OpenReadStream(), request.File.ContentType,
                request.File.FileName, (int)request.File.Length, request.Description);
    }
}
