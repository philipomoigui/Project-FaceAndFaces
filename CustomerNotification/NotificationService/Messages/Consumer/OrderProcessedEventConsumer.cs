using EmailService;
using MassTransit;
using Messaging.InterfacesConstant.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Messages.Consumer
{
    public class OrderProcessedEventConsumer : IConsumer<IOrderProcessedEvent>
    {
        private readonly IEmailSender _emailSender;

        public OrderProcessedEventConsumer(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task Consume(ConsumeContext<IOrderProcessedEvent> context)
        {
            var rootFolder = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            var result = context.Message;
            var faceData = result.Faces;

            if(faceData.Count < 1)
            {
                await Console.Out.WriteLineAsync("No Faces Detected");
            } else
            {
                var j = 0;
                foreach(var face in faceData)
                {
                    MemoryStream ms = new MemoryStream(face);
                    var image = Image.FromStream(ms);
                    image.Save(rootFolder + "/Images/face" + j + ".jpg", ImageFormat.Jpeg);
                    j++;
                }

                //Send Email To Customer
               // string[] mailAddress = { result.UserEmail };
               // await _emailSender.SendMessageAsync(new Message(mailAddress, "Face And Faces - Images Found", "From Face And Faces", faceData));


                await context.Publish<IOrderDispatchedEvent>(new
                {
                    context.Message.OrderId,
                    DispatchDateTime = DateTime.UtcNow
                });
            }
        }
    }
}
