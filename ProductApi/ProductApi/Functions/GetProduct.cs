using System.Net;
using System.Threading.Tasks;
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

namespace ProductApi.Functions
{
    public static class GetProduct
    {
        [FunctionName("GetProduct")]
        [OpenApiOperation("Run", "product")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("rowKey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **RowKey** parameter")]
        [OpenApiParameter("partitionKey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **PartitionKey** parameter")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ProductEntity), Description = "The product which matches the provided partition key and row key")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req,
            [Table("Products")] CloudTable cloudTable)
        {
            var partitionKey = req.Query[nameof(TableEntity.PartitionKey)];
            var rowKey = req.Query[nameof(TableEntity.RowKey)];
            var retrieveOperation = TableOperation.Retrieve<ProductEntity>(partitionKey, rowKey);
            var result = await cloudTable.ExecuteAsync(retrieveOperation);

            if (result?.Result == null)
            {
                return ProductApiResponses.NotFoundError();
            }

            return new OkObjectResult(result.Result as ProductEntity);

        }
    }
}

