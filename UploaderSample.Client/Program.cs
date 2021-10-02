using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

var uploaderApi = RestService.For<IUploaderApi>("https://localhost:5001/api");
var response = await uploaderApi.UploadImageAsync(new StreamPart(File.OpenRead(@"D:\Iceberg.jpg"), "Iceberg.jpg"), "Foto di un iceberg");

Console.ReadLine();

internal interface IUploaderApi
{
    [Multipart]
    [Post("/Images")]
    Task<HttpResponseMessage> UploadImageAsync([AliasAs("File")] StreamPart stream, [AliasAs("Description")] string description = null);
}