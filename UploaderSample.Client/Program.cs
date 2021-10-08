using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using UploaderSample.Shared.Models;

var uploaderApi = RestService.For<IUploaderApi>("https://localhost:5001/api");

//var response = await uploaderApi.UploadImageAsync(new StreamPart(File.OpenRead(@"D:\Iceberg.jpg"), "Iceberg.jpg"), "Foto di un iceberg");
await ChunkFileUploadAsync(@"D:\Taggia.jpg", uploaderApi);

Console.ReadLine();

async Task ChunkFileUploadAsync(string inputFileName, IUploaderApi uploaderApi)
{
    var startUploadResponse = await uploaderApi.StartUploadAsync(Path.GetFileName(inputFileName));
    var fileHandle = startUploadResponse.Content.FileHandle;

    var chunkSize = 65_536;
    var buffer = new byte[chunkSize];

    using var fileStream = File.OpenRead(inputFileName);
    var bytesRead = 0;
    var fileSize = fileStream.Length;

    do
    {
        var position = fileStream.Position;
        bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead > 0)
        {
            if (bytesRead < buffer.Length)
            {
                Array.Resize(ref buffer, bytesRead);
            }

            var fileChunk = new FileChunk
            {
                FileHandle = fileHandle,
                Data = Convert.ToBase64String(buffer),
                StartAt = position
            };

            await uploaderApi.UploadChunkAsync(fileChunk);
        }

    } while (bytesRead > 0);

    var uploadCompleteResponse = await uploaderApi.EndUploadAsync(fileHandle, false, fileSize);
}

internal interface IUploaderApi
{
    [Multipart]
    [Post("/Images")]
    Task<HttpResponseMessage> UploadImageAsync([AliasAs("File")] StreamPart stream, [AliasAs("Description")] string description = null);

    [Post("/Upload/start")]
    Task<ApiResponse<StartUploadResult>> StartUploadAsync(string fileName);

    [Post("/Upload/send")]
    Task<HttpResponseMessage> UploadChunkAsync(FileChunk fileChunk);

    [Post("/Upload/end")]
    Task<HttpResponseMessage> EndUploadAsync(string fileHandle, bool cancel, long fileSize);
}