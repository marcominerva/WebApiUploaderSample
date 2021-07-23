using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UploaderSample.Filters;

namespace UploaderSample.Models
{
    public class UploadDocumentRequest
    {
        [BindRequired]
        [AllowedExtensions("doc", "docx", "pdf")]
        public IFormFile File { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }
    }
}
