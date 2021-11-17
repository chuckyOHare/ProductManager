using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.WindowsAzure.Storage.Table;
using ProductApi.Dal;
using ProductApi.ResponseModels;
using System.Net;
using System.Threading.Tasks;

namespace ProductApi.Functions
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        [OpenApiOperation("Run", "product")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **RowKey** parameter")]
        [OpenApiParameter("partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **PartitionKey** parameter")]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{partitionKey}/{rowKey}")] HttpRequest req,
            [Table("Products")] CloudTable cloudTable,
            string rowKey, string partitionKey)
        { 
            var retrieveOperation = TableOperation.Retrieve<ProductEntity>(partitionKey, rowKey);

            if (!((await cloudTable.ExecuteAsync(retrieveOperation)).Result is ProductEntity result))
            {
                return ProductApiResponses.NotFoundError();
            }

            var deleteOp = TableOperation.Delete(result);
            await cloudTable.ExecuteAsync(deleteOp);

            return new OkResult();
        }
    }
}

