using Azure.Storage.Blobs;
using MyFunctionsApp.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyFunctionsApp.Services
{
    public class BlobService : IBlobService
    {
        public async Task UploadImageAsync(string fileName, string imageUrl)
        {
            var container = "images";
            var blobConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var _blobServiceClient = new BlobServiceClient(blobConnectionString);
            var _blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

            var request = System.Net.WebRequest.Create(imageUrl);
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                await blobClient.UploadAsync(stream, true);
                stream.Close();
            }
        }
    }
}
