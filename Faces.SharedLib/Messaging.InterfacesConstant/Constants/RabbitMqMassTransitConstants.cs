using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.InterfacesConstant.Constants
{
    public class RabbitMqMassTransitConstants
    {
        public const string RabbitMqUri = "rabbitmq://rabbitmq:5672";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderServiceQueue = "register.order.command";
    }
}
