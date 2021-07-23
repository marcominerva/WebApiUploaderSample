using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UploaderSample.Filters;

namespace UploaderSample.Models
{
    public class UploadImageRequest
    {
        [BindRequired]
        [AllowedExtensions("jpeg", "*.jpg", "*.png")]
        public IFormFile File { get; set; }

        public string Description { get; set; }
    }
}
