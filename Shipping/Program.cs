using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Shipping
{
    class Program
    {
        static async Task  Main()
        {
            Console.Title = "Shipping";

            var endpointConfiguration = new EndpointConfiguration("Shipping");
            endpointConfiguration.EnableInstallers();
            // this will run the installers
            //await Endpoint.Start(endpointConfiguration)
            //    .ConfigureAwait(false);


            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host=localhost");

            //This will create Saga data persisted
            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
            
            //Starting the Endpoint.
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            //Stopping the Endpoint.
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
