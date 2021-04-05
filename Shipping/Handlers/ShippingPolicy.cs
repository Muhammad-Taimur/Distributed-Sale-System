using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using Shipping.SagaData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Handlers
{
    //We do not know what message comes first in NserviceBus so we use IAmStartedByMessages in both
    //so any message comes it will create saga instance.
    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmStartedByMessages<OrderPlaced>, IAmStartedByMessages<OrderBilled>
    {

        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId}");
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.OrderId}");
            Data.IsOrderBilled = true;
            return ProcessOrder(context); ;
        }

        //This method is used HowToFindSaga
        //In our porgram OrderID is unique and used in both message so this is how we MAP.
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            //mapping Saga Data
            mapper.ConfigureMapping<OrderPlaced>(message => message.OrderId)
                 .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }
        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
                MarkAsComplete(); // This will remvove the Saga instance creted for OrderPlaced and OrderBilled Message
            }
        }
    }
}
