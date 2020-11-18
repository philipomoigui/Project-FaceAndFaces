using MassTransit;
using Messaging.InterfacesConstant.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Messages.Consumer
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        public Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            throw new NotImplementedException();
        }
    }
}
