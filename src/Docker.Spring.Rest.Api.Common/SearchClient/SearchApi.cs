using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.Spring.Rest.Api.Common.SearchClient.Model;
using Rest.Client;

namespace Docker.Spring.Rest.Api.Common
{
    public class SearchApi
    {
        private RestClient _client;
        private string uri = "product";

        public SearchApi(RestClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<List<Product>>> PostAsync(Search search, Dictionary<string, string> headers = null, string queryParameters = null)
        {
            string endpoint = $"{TestEnvironment.BaseUrl}/{uri}/{queryParameters}";
            ApiResponse<List<Product>> response = await _client.PostAsync<List<Product>>(new Uri(endpoint), search, headers);

            return response;
        }
    }
}
