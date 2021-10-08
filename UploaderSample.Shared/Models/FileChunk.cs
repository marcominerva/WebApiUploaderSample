namespace UploaderSample.Shared.Models
{
    public class FileChunk
    {
        public string FileHandle { get; set; }

        public string Data { get; set; }

        public long StartAt { get; set; }
    }
}
