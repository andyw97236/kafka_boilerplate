using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kafka.pubsub.console
{
    public class ProducerService : BackgroundService
    {
        
        ILogger<ProducerService> _logger;
        Producer<Null, string> _producer;
        string _topic;

        public ProducerService(ILogger<ProducerService> logger
            , KafkaConfiguration config)
        {
            _logger = logger;

            _topic = config.Topic;

            _producer = new Producer<Null, string>(
                new Dictionary<string, object> { { "bootstrap.servers", config.Endpoint } },
                null,
                new StringSerializer(Encoding.UTF8));
        }

        public override void Dispose()
        {
            Console.WriteLine("Producer: disposing....");
            _producer.Flush(500);
            _producer.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            _logger.LogInformation("Producer: awaiting msg....");
            //Async loop
            while (!stopToken.IsCancellationRequested)
            {
                var line = Console.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    await SendMsg(line);
                }  
            }
        }

        private async Task SendMsg(string message)
        {
            try
            {
                await _producer.ProduceAsync(_topic, null, message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,$"{nameof(SendMsg)} failed.");
            }
                       
        }
    }
}