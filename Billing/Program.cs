using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            //This is our Endpoint Targeting to Billing
            var endpointConfiguration = new EndpointConfiguration("Billing");

           
            endpointConfiguration.EnableInstallers();
            // this will run the installers
            //await Endpoint.Start(endpointConfiguration)
            //    .ConfigureAwait(false);

            //Transport we are using.
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host=localhost");

            //This starts the endpoint
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);


            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);

        }
    }
}
