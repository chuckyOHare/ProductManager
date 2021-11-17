using Microsoft.AspNetCore.Mvc;

namespace ProductApi.ResponseModels
{
    public class ProductApiResponses
    {
        public static NotFoundObjectResult NotFoundError()
        {
            return new NotFoundObjectResult("404 not found");
        }
    }
}
