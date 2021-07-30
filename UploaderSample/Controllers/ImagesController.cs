using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploaderSample.BusinessLayer.Services;
using UploaderSample.Extensions;
using UploaderSample.Models;
using Dto = UploaderSample.Shared.Models;

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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Dto.Image>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var images = await imageService.GetAsync();
            return Ok(images);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var image = await imageService.GetAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.Value.Stream, image.Value.ContentType);
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Upload([FromForm] UploadImageRequest request)
        {
            await imageService.UploadAsync(request.ToStreamFileContent());

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await imageService.DeleteAsync(id);

            return NoContent();
        }
    }
}
