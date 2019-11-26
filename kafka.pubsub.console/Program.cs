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
using System.Diagnostics;

namespace kafka.pubsub.console
{
    class Program
    {

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();


            using (var _host = new HostBuilder()
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureServices(services =>
                {
                    services.AddKafka(config);
                })
                .Build())
            {
                _host.Run();
            }
        }
    }
}
