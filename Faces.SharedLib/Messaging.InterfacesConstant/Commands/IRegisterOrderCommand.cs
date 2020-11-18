using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.InterfacesConstant.Commands
{
    public interface IRegisterOrderCommand
    {
        public string pictureUrl { get; set; }
        public string UserEmail { get; set; }
        public byte[] ImageData { get; set; }
    }
}
