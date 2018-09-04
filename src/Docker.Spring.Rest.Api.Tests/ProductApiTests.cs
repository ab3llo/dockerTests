using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Docker.Spring.Rest.Api.Common;
using Rest.Client;
using Xunit;

namespace Docker.Api.Tests
{
    public class ProductApiTests
    {
        string basePath = "http://192.168.1.99:8080";

        [Theory]
        [InlineData("World-Check Online", "It's an application of some sort that does something sometimes")]
        [InlineData("Connected Risk", "It's an application of some sort that does something sometimes maybe")]
        [InlineData("ONE Source", "We don't really know what this does")]
        public async Task CreateProduct(string title, string description)
        {
            Product product = new Product
            {
                Title = title,
                Description = description
            };

            ProductApi api = new ProductApi(basePath);
            ApiResponse<Product> response = await api.PostAsync(product);
            product.Id = response.Data.Id;
            Product data = response.Data;

            Assert.Equal(HttpStatusCode.OK, response.Status);
            Assert.NotNull(data);

            Assert.NotNull(response.Data.Id);
            Assert.Equal(product.Title, response.Data.Title);
            Assert.Equal(product.Description, response.Data.Description);

            List<Product> products = new List<Product> { product };
            await DeleteProducts(products);
        }

        [Fact]
        public async Task GetProducts()
        {
            ProductApi api = new ProductApi(basePath);

            List<Product> products = new List<Product>()
            {
                new Product(){Title="World-Check Online",Description="It's an application of some sort that does something sometimes"},
                new Product(){Title="Connected Risk",Description="It's an application of some sort that does something sometimes maybe"},
                new Product(){Title="ONE Source",Description="We don't really know what this does"}
            };

            foreach (Product product in products)
            {
                var createResponse = await api.PostAsync(product);
                product.Id = createResponse.Data.Id;
            }

            ApiResponse<List<Product>> response = await api.GetAllAsync();

            Assert.Equal(HttpStatusCode.OK, response.Status);
            Assert.NotEmpty(response.Data);
            Assert.Equal(products.Count, response.Data.Count);

            await DeleteProducts(products);
        }

        [Fact]
        public async Task GetProduct()
        {
            ProductApi api = new ProductApi(basePath);

            List<Product> products = new List<Product>()
            {
                new Product(){Title="World-Check Online",Description="It's an application of some sort that does something sometimes"},
                new Product(){Title="Connected Risk",Description="It's an application of some sort that does something sometimes maybe"},
                new Product(){Title="ONE Source",Description="We don't really know what this does"}
            };

            foreach (Product product in products)
            {
                var createResponse = await api.PostAsync(product);
                product.Id = createResponse.Data.Id;
            }

            ApiResponse<Product> response = await api.GetAsync(products[2].Id);
            Assert.Equal(HttpStatusCode.OK, response.Status);
            Assert.NotNull(response.Data);

            Product expected = products[2];

            Assert.Equal(expected.Id, response.Data.Id);
            Assert.Equal(expected.Title, response.Data.Title);
            Assert.Equal(expected.Description, response.Data.Description);

            await DeleteProducts(products);
        }

        [Fact]
        public async Task UpdateProduct()
        {
            Product product = new Product
            {
                Title = "ONE Source",
                Description = "It's an application of some sort that does something sometimes"
            };

            ProductApi api = new ProductApi(basePath);
            ApiResponse<Product> response = await api.PostAsync(product);
            product.Id = response.Data.Id;

            Product update = new Product
            {
                Title = "Eikon",
                Description = @"Highly visual and intuitive to use, Eikon is the ultimate set of financial analysis tools. Integrate multiple workflows, 
                co-create applications and securely connect to other financial professionals.",
            };
            response = await api.PutAsync(product.Id, update);

            Assert.Equal(HttpStatusCode.OK, response.Status);
            Assert.NotNull(response.Data);

            Assert.Equal(update.Description, response.Data.Description);
            Assert.Equal(update.Title, response.Data.Title);

            List<Product> products = new List<Product> { product };
            await DeleteProducts(products);
        }

        [Fact]
        public async Task DeleteProduct()
        {
            Product product = new Product
            {
                Title = "ONE Source",
                Description = "It's an application of some sort that does something sometimes"
            };

            ProductApi api = new ProductApi(basePath);
            ApiResponse<Product> response = await api.PostAsync(product);

            ApiResponse<object> deleteResponse = await api.DeleteAsync(response.Data.Id);
            Assert.True(response.IsSuccessStatusCode);

            response = await api.GetAsync(response.Data.Id);
            Assert.Equal(HttpStatusCode.OK, response.Status);
            Assert.Null(response.Data);
        }


        private async Task DeleteProducts(List<Product> products)
        {
            ProductApi api = new ProductApi(basePath);

            foreach (Product product in products)
            {
                var deleted = await api.DeleteAsync(product.Id);
            }
        }
    }
}
