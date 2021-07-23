using System.IO;

namespace UploaderSample.BusinessLayer.Models
{
    public record StreamFileContent(Stream Content, string ContentType, string FileName,
        int Length, string Description = null);

}
