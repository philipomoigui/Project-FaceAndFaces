using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.InterfacesConstant.Events
{
    public class IOrderDispatchedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime DispatchDateTime { get; set; }
    }
}
