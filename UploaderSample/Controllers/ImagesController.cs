using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploaderSample.BusinessLayer.Services;
using UploaderSample.Extensions;
using UploaderSample.Models;

namespace UploaderSample.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Upload([FromForm] UploadImageRequest request)
        {
            await imageService.UploadAsync(request.ToStreamFileContent());

            return NoContent();
        }
    }
}
