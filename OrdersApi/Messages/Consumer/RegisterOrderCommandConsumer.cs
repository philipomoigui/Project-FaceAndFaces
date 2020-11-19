using MassTransit;
using Messaging.InterfacesConstant.Commands;
using Newtonsoft.Json;
using OrdersApi.Models;
using OrdersApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OrdersApi.Messages.Consumer
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IHttpClientFactory _clientFactory;

        public RegisterOrderCommandConsumer(IOrderRepository repository, IHttpClientFactory clientFactory)
        {
            _repository = repository;
            _clientFactory = clientFactory;
        }

        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var result = context.Message;

            if(result.ImageData != null && result.ImageUrl != null && result.OrderId != null && result.UserEmail != null)
            {
                SaveOrder(result);
                var clientFactory = _clientFactory.CreateClient();
                Tuple<List<byte[]>, Guid> orderDetailData = await GetFacesFromApiAsync(clientFactory, result);
                List<byte[]> faces = orderDetailData.Item1;
                Guid orderId = orderDetailData.Item2;
                await SaveOrderDetail(orderId, faces);
            }
        }

        private async Task SaveOrderDetail(Guid orderId, List<byte[]> faces)
        {
            var order = await _repository.GetOrderAsync(orderId);
            if (order != null)
            {
                order.Status = Status.Processing;

                foreach(var face in faces)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        FaceData = face
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                _repository.UpdateOrder(order);
            }
        }

        private async Task<Tuple<List<byte[]>, Guid>> GetFacesFromApiAsync(HttpClient clientFactory, IRegisterOrderCommand result)
        {
            var byteContent = new ByteArrayContent(result.ImageData);
            Tuple<List<byte[]>, Guid> orderDetailData = null;

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using(var response = await clientFactory.PostAsync("http://localhost:6000/api/faces?orderId="+ result.OrderId, byteContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                orderDetailData = JsonConvert.DeserializeObject < Tuple<List<byte[]>, Guid>>(apiResponse);
            }

            return orderDetailData;
        }

        private void SaveOrder(IRegisterOrderCommand result)
        {
            Order order = new Order()
            {
                OrderId = result.OrderId,
                ImageData = result.ImageData,
                ImageUrl = result.ImageUrl,
                UserEmail = result.UserEmail
            };
            _repository.RegisterOrder(order);
        }
    }
}
