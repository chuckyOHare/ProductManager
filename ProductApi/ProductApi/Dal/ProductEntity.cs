using Microsoft.WindowsAzure.Storage.Table;
using ProductApi.Functions;
using System;

namespace ProductApi.Dal
{
    public class ProductEntity : TableEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public static ProductEntity Create(string category,
            string name,
            string description,
            double price)
        {
            return new ProductEntity
            {
                Category = category,
                Description = description,
                Name = name,
                Price = price,
                PartitionKey = category,
                RowKey = Guid.NewGuid().ToString()
            };
        }

        public void UpdateFrom(UpsertProductRequestModel requestModel)
        {
            Name = requestModel.Name;
            Description = requestModel.Description;
            Price = requestModel.Price.GetValueOrDefault();
        }
    }
}