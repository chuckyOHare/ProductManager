using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using ProductApi.Dal;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ProductApi.Functions
{
    public class PostProductRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double? Price { get; set; }
    }

    public static class PostProduct
    {
        [FunctionName("PostProduct")]
        [OpenApiOperation("Run", "product")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("json", typeof(PostProductRequestModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req,
            [Table("Products")] CloudTable cloudTable)
        {
            var reqBody = await new StreamReader(req.Body).ReadToEndAsync();

            PostProductRequestModel requestModel;
            try
            {
                requestModel = JsonConvert.DeserializeObject<PostProductRequestModel>(reqBody, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var entity = ProductEntity.Create(requestModel.Category,
                requestModel.Name,
                requestModel.Description,
                requestModel.Price.GetValueOrDefault());

            var addOp = TableOperation.Insert(entity);
            await cloudTable.ExecuteAsync(addOp); // todo handle errors

            return new OkResult();
        }
    }
}

