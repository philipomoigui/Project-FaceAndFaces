using MassTransit;
using Messaging.InterfacesConstant.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Messages.Consumer
{
    public class OrderProcessedEventConsumer : IConsumer<IOrderProcessedEvent>
    {
        public Task Consume(ConsumeContext<IOrderProcessedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
