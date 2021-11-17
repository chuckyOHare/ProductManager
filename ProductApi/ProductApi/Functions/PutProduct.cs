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
    // TODO proper response models, validation, logging, unit tests, authentication, authorization

    public class UpsertProductRequestModel
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double? Price { get; set; }

        [JsonIgnore]
        public bool IsCategoryChanged => !Category.Equals(PartitionKey);
    }

    public static class PutProduct
    {
        [FunctionName("PutProduct")]
        [OpenApiOperation("Run", "product")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("json", typeof(UpsertProductRequestModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product")] HttpRequest req,
            [Table("Products")] CloudTable cloudTable)
        {
            var reqBody = await new StreamReader(req.Body).ReadToEndAsync();

            UpsertProductRequestModel requestModel;
            try
            {
                requestModel = JsonConvert.DeserializeObject<UpsertProductRequestModel>(reqBody, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var retrieveOperation = TableOperation.Retrieve<ProductEntity>(requestModel.Category, requestModel.RowKey);

            if (!((await cloudTable.ExecuteAsync(retrieveOperation)).Result is ProductEntity existing))
            {
                var newEntity = ProductEntity.Create(requestModel.Category,
                    requestModel.Name,
                    requestModel.Description,
                    requestModel.Price.GetValueOrDefault());

                var addOp = TableOperation.Insert(newEntity);
                await cloudTable.ExecuteAsync(addOp);
                //return ProductApiResponses.NotFoundError();
            }
            else
            {
                if (requestModel.IsCategoryChanged)
                {
                    var deleteOp = TableOperation.Delete(existing);
                    await cloudTable.ExecuteAsync(deleteOp);
                }

                existing.UpdateFrom(requestModel);
                var insertOrReplaceOp = TableOperation.InsertOrReplace(existing);
                await cloudTable.ExecuteAsync(insertOrReplaceOp);
            }

            return new OkResult();
        }
    }
}

