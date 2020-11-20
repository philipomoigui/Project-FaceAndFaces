using Faces.WebMVC.ViewModel;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faces.WebMVC.RestClients
{
    public class OrderManagementApi : IOrderManagementApi
    {
        private IOrderManagementApi _restClient;
        public OrderManagementApi(IConfiguration configuration, HttpClient httpClient)
        {
            string hostAndPort = configuration.GetSection("ApiServiceLocation").GetValue<string>("OrdersApiLocation");
            httpClient.BaseAddress = new Uri($"{hostAndPort}/api");
            _restClient = RestService.For<IOrderManagementApi>(httpClient);
        }
        public async Task<OrderViewModel> GetOrderByIDAsync(Guid id)
        {
            try
            {
               return await _restClient.GetOrderByIDAsync(id);
            }
            catch (ApiException ex)
            {
               if(ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                } 
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<OrderViewModel>> GetOrdersAsync()
        {
           return await _restClient.GetOrdersAsync();
        }
    }
}
