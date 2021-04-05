using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Sales.Handlers
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrder>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId ={message.OrderId} ");

            //this is normally where som Business Logic occurs
            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };

            //publishing the Event.
            return context.Publish(orderPlaced);
        }
    }
}
