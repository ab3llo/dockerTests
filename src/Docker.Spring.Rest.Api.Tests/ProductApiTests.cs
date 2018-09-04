using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Docker.Spring.Rest.Api.Common;
using Rest.Client;
using Xunit;

namespace Docker.Spring.Rest.Api.Tests
{
    public class ProductApiTests
    {
        RestClient _client;

        public ProductApiTests()
        {
            _client = new RestClient(TestEnvironment.BaseUrl);
        }

        [Theory]
        [InlineData("1","World-Check Online", "It's an application of some sort that does something sometimes")]
        [InlineData("2", "Connected Risk", "It's an application of some sort that does something sometimes maybe")]
        [InlineData("3", "ONE Source", "We don't really know what this does")]
        public async Task CreateProduct(string id,string title, string description)
        {
            Product product = new Product
            {
                Title = title,
                Description = description
            };

            ProductApi api = new ProductApi("");
            ApiResponse<Product> response = await api.PostAsync(product);
            Product data = response.Data;

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(data);
            
            Assert.Equal("","");
            Assert.Equal("", "");
            Assert.Equal("", "");

            await api.DeleteAsync(id, new Product { Id = id });
        }

        [Fact]
        public async Task GetProducts()
        {
            ProductApi api = new ProductApi("");

            List<Product> products = new List<Product>()
            {
                new Product(){Title="World-Check Online",Description="It's an application of some sort that does something sometimes"},
                new Product(){Title="Connected Risk",Description="It's an application of some sort that does something sometimes maybe"},
                new Product(){Title="ONE Source",Description="We don't really know what this does"}
            };

            foreach (Product product in products)
            {
                await api.PostAsync(product);
            }

            ApiResponse <List<Product>>response = await api.GetAllAsync();

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotEmpty(response.Data);
            Assert.Equal(products.Count, response.Data.Count);

            products.ForEach(async p => await api.DeleteAsync(p.Id, p));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task GetProduct(string id)
        {
            ProductApi api = new ProductApi("");

            List<Product> products = new List<Product>()
            {
                new Product(){Title="World-Check Online",Description="It's an application of some sort that does something sometimes"},
                new Product(){Title="Connected Risk",Description="It's an application of some sort that does something sometimes maybe"},
                new Product(){Title="ONE Source",Description="We don't really know what this does"}
            };

            foreach (Product product in products)
            {
                await api.PostAsync(product);
            }

            ApiResponse<Product> response = await api.GetAsync(id);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Data);

            Product expected = products[Int32.Parse(id) - 1];

            Assert.Equal(expected.Id,response.Data.Id);
            Assert.Equal(expected.Title, response.Data.Title);
            Assert.Equal(expected.Description, response.Data.Description);
        }

        [Fact]
        public async Task UpdateProduct()
        {
            Product product = new Product
            { 
                Title = "ONE Source", 
                Description = "It's an application of some sort that does something sometimes" 
            };

            ProductApi api = new ProductApi("");
            ApiResponse<Product> response = await api.PostAsync(product);

            Product update = new Product
            {
                Title = "Eikon",
                Description = @"Highly visual and intuitive to use, Eikon is the ultimate set of financial analysis tools. Integrate multiple workflows, 
                co-create applications and securely connect to other financial professionals.",
            };
            response = await api.PutAsync("1", update);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Data);

            Assert.Equal(update.Description, response.Data.Description);
            Assert.Equal(update.Title, response.Data.Title);

            await api.DeleteAsync("1", new Product { Id = "1" });
        }

        [Fact]
        public async Task DeleteProduct()
        {
            Product product = new Product
            {
                Title = "ONE Source",
                Description = "It's an application of some sort that does something sometimes"
            };

            ProductApi api = new ProductApi("");
            ApiResponse<Product> response = await api.PostAsync(product);

            Product deletedProduct = new Product
            {
               Id= "1"
            };
            response = await api.DeleteAsync(deletedProduct.Id, deletedProduct);
            Assert.True(response.IsSuccessStatusCode);

            response = await api.GetAsync(deletedProduct.Id);
            Assert.Equal(HttpStatusCode.NotFound, response.Status);
        }
    }
}
