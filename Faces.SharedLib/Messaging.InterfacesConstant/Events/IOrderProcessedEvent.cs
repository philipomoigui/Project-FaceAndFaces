using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.InterfacesConstant.Events
{
    public class IOrderProcessedEvent
    {
        public Guid OrderId { get; set; }
        public string ImageUrl { get; set; }
        public string UserEmail { get; set; }
        public byte[] ImageData { get; set; }
    }
}
