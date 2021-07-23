using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

var testApi = RestService.For<IUploaderApi>("https://localhost:5001");
var response = await testApi.UploadImageAsync(new StreamPart(File.OpenRead(@"D:\Iceberg.jpg"), "Iceberg.jpg"),
        "Foto di un iceberg");

Console.ReadLine();

internal interface IUploaderApi
{
    [Multipart]
    [Post("/Images")]
    Task<HttpResponseMessage> UploadImageAsync([AliasAs("File")] StreamPart stream,
        [AliasAs("Description")] string description = null);
}