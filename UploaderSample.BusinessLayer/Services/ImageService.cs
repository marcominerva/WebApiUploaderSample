using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UploaderSample.BusinessLayer.Models;
using UploaderSample.DataAccessLayer;
using UploaderSample.StorageProviders;
using Dto = UploaderSample.Shared.Models;
using Entities = UploaderSample.DataAccessLayer.Entities;

namespace UploaderSample.BusinessLayer.Services
{
    public class ImageService : IImageService
    {
        private readonly DataContext dataContext;
        private readonly IStorageProvider storageProvider;

        public ImageService(DataContext dataContext, IStorageProvider storageProvider)
        {
            this.dataContext = dataContext;
            this.storageProvider = storageProvider;
        }

        public async Task<IEnumerable<Dto.Image>> GetAsync()
        {
            var images = await dataContext.Images.Select(i => new Dto.Image
            {
                Id = i.Id,
                Path = i.Path,
                Length = i.Length,
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(i.Path)
            })
            .ToListAsync();

            return images;
        }

        public async Task<(Stream Stream, string ContentType)?> GetAsync(Guid id)
        {
            var image = await dataContext.Images.FindAsync(id);
            if (image != null)
            {
                var stream = await storageProvider.ReadAsync(image.Path);
                return (stream, MimeMapping.MimeUtility.GetMimeMapping(image.Path));
            }

            return null;
        }

        public async Task UploadAsync(StreamFileContent content)
        {
            var path = GetFullPath(content.FileName);
            await storageProvider.SaveAsync(path, content.Content);

            var image = new Entities.Image
            {
                Path = path,
                Length = content.Length
            };

            dataContext.Images.Add(image);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var image = await dataContext.Images.FindAsync(id);
            if (image != null)
            {
                dataContext.Remove(image);
                await dataContext.SaveChangesAsync();

                await storageProvider.DeleteAsync(image.Path);
            }
        }

        public static string GetFullPath(string fileName)
            => Path.Combine(DateTime.UtcNow.Year.ToString(), DateTime.UtcNow.Month.ToString("00"), fileName);
    }
}
