using System;

namespace UploaderSample.Shared.Models
{
    public class Image
    {
        public Guid Id { get; set; }

        public string Path { get; set; }

        public int Length { get; set; }

        public string ContentType { get; set; }
    }
}
