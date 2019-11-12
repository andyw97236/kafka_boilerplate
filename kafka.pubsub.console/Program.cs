using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace kafka.pubsub.console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool cancel = false;

            IConfiguration config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();

            var host = new HostBuilder()
                  .ConfigureLogging(logging => logging.AddConsole())
                  .ConfigureServices(services =>
                  {
                      services.AddKafka(config);
                  })
                  .UseConsoleLifetime()
                  .Build()
                  .RunAsync();

            Console.ReadLine();
        }


    }
}