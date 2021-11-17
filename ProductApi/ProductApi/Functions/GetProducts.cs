using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.WindowsAzure.Storage.Table;
using ProductApi.Dal;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ProductApi.Functions
{
    public static class GetProducts
    {
        [FunctionName("GetProducts")]
        [OpenApiOperation("Run", "products")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<ProductEntity>), Description = "All products")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
            [Table("Products")] CloudTable cloudTable)
        {
            var productEntities = new List<ProductEntity>();
            var query = new TableQuery<ProductEntity>();

            TableContinuationToken continuationToken = null;
            do
            {
                var page = await cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = page.ContinuationToken;
                productEntities.AddRange(page.Results);
            }
            while (continuationToken != null);

            return new OkObjectResult(productEntities);
        }
    }
}

