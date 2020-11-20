using Faces.WebMVC.ViewModel;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMVC.RestClients
{
    public interface IOrderManagementApi
    {
        [Get("/orders")]
        Task<List<OrderViewModel>> GetOrdersAsync();

        [Get("/orders/{orderId}")]
        Task<OrderViewModel>  GetOrderByIDAsync(Guid orderId);
    }
}
