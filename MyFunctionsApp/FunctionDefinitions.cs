using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Data.SqlClient;
using MyFunctionsApp.ServiceInterfaces;

namespace MyFunctionsApp
{
    public class FunctionDefinitions
    {
        private IBlobService _blobService;
        private ISqlService _sqlService;
        public FunctionDefinitions(IBlobService blobService, ISqlService sqlService)
        {
            _blobService = blobService;
            _sqlService = sqlService;
        }
        [FunctionName("ImageUploadFuncion")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string imageUrl = req.Query["imageUrl"];


            if (string.IsNullOrEmpty(imageUrl))
            {
                return new BadRequestResult();
            }

            var extension = !string.IsNullOrEmpty(Path.GetExtension(imageUrl)) ? Path.GetExtension(imageUrl) : ".jpg";

            try
            {
                //Check in db if url already uploaded
                var count = await _sqlService.GetByUrlCountAsync(imageUrl);
                if (count > 0)
                {
                    log.LogError($"Image already uploaded");
                    return new BadRequestResult();
                }

                //upload file to blob storage
                var fileName = $"{Guid.NewGuid().ToString()}.{extension}";
                await _blobService.UploadImageAsync(fileName, imageUrl);


                //after successfull upload, save in db url and file name
                await _sqlService.InsertToDbAsync(imageUrl, fileName);
                await _sqlService.InsertToDbAsync(imageUrl, fileName);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}
