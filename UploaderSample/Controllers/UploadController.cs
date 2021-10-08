using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploaderSample.Shared.Models;

namespace UploaderSample.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpPost("start")]
        [ProducesResponseType(typeof(StartUploadResult), StatusCodes.Status200OK)]
        public IActionResult BeginFileUpload([Required] string fileName)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "temp");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var tempFileName = $"{Guid.NewGuid()}_{fileName}";

            using var fileStream = new FileStream(Path.Combine(filePath, tempFileName), FileMode.CreateNew);
            fileStream.Close();

            var result = new StartUploadResult { FileHandle = tempFileName };
            return Ok(result);
        }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadChunk(FileChunk fileChunk)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "temp", fileChunk.FileHandle);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            if (fileInfo.Length != fileChunk.StartAt)
            {
                return BadRequest();
            }

            using var fileStream = new FileStream(filePath, FileMode.Append);

            var bytes = Convert.FromBase64String(fileChunk.Data);
            await fileStream.WriteAsync(bytes, 0, bytes.Length);

            return NoContent();
        }

        [HttpPost("end")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult EndFileUpload([Required] string fileHandle, bool cancel, long fileSize)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "temp", fileHandle);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            if (cancel)
            {
                fileInfo.Delete();
            }
            else
            {
                if (fileInfo.Length != fileSize)
                {
                    return Conflict();
                }

                var newDirectoryPath = Path.Combine(environment.ContentRootPath, "uploads");
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                var newFileName = fileHandle[37..];
                var newFileInfo = new FileInfo(Path.Combine(newDirectoryPath, newFileName));
                if (newFileInfo.Exists)
                {
                    newFileInfo.Delete();
                }

                fileInfo.MoveTo(newFileInfo.FullName);
            }

            return NoContent();
        }
    }
}
