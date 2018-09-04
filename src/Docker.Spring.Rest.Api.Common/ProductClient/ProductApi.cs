using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rest.Client;

namespace Docker.Spring.Rest.Api.Common
{
    public class ProductApi : RestClient
    {
        private string uri = "product";

        public ProductApi(string basePath) : base(basePath)
        {

        }


        public async Task<ApiResponse<Product>> PostAsync(Product product, Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{BasePath}/{uri}/{queryParameters}";
            ApiResponse<Product> response = await PostAsync<Product>(new Uri(endpoint), product, headers);
            return response;
        }

        public async Task<ApiResponse<List<Product>>> GetAllAsync(Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{BasePath}/{uri}/{queryParameters}";
            ApiResponse<List<Product>> response = await GetAsync<List<Product>>(new Uri(endpoint), headers);
            return response;
        }


        public async Task<ApiResponse<Product>> GetAsync(string id, Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{BasePath}/{uri}/{id}/{queryParameters}";
            ApiResponse<Product> response = await GetAsync<Product>(new Uri(endpoint), headers);

            return response;
        }

        public async Task<ApiResponse<Product>> PutAsync(string id, Product product, Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{BasePath}/{uri}/{id}/{queryParameters}";
            ApiResponse<Product> response = await PutAsync<Product>(new Uri(endpoint), product, headers);
            return response;
        }


        public async Task<ApiResponse<object>> DeleteAsync(string id, Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{BasePath}/{uri}/{id}/{queryParameters}";
            ApiResponse<object> response = await DeleteAsync<object>(new Uri(endpoint), headers);
            return response;
        }
    }
}
