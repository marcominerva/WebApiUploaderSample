using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UploaderSample.BusinessLayer.Models;
using Dto = UploaderSample.Shared.Models;

namespace UploaderSample.BusinessLayer.Services
{
    public interface IImageService
    {
        Task<IEnumerable<Dto.Image>> GetAsync();

        Task<(Stream Stream, string ContentType)?> GetAsync(Guid id);

        Task UploadAsync(StreamFileContent content);

        Task DeleteAsync(Guid id);
    }
}
