using System.Threading.Tasks;

namespace UploaderSample.BusinessLayer.Services
{
    public class TravelService
    {
        private readonly IImageService imageService;

        public TravelService(IImageService imageService)
        {
            this.imageService = imageService;
        }

        public Task SaveAsync()
        {
            //var pdfStream = ....;

            //var fileContent = new StreamFileContent(pdfStream, MediaTypeNames.Application.Pdf, "Frullino.pdf", 42);
            //await imageService.UploadAsync(fileContent);

            return Task.CompletedTask;
        }
    }
}
