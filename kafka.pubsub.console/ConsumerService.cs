using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kafka.pubsub.console
{
    public class ConsumerService : BackgroundService
    {
        string _topic;
        Consumer<Null, string> _consumer;

        ILogger<ConsumerService> _logger;

        public ConsumerService(ILogger<ConsumerService> logger
            , KafkaConfiguration config)
        {
            _logger = logger;

            _topic = config.Topic;
            _consumer = new Consumer<Null, string>(
                new Dictionary<string, object>
            {
                { "group.id", "myconsumer" }
                ,{ "bootstrap.servers", config.Endpoint }
                ,{ "auto.offset.reset", "earliest" } // if you want to read from the beginning of time
            },
            null,
            new StringDeserializer(Encoding.UTF8));

            // Subscribe to the OnMessage event
            _consumer.OnMessage += OnMessage;            
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            //Do your preparation (e.g. Start code) here
            _consumer.Subscribe(new List<string>() { _topic });
            _logger.LogInformation("Consumer: listening for msgs....");

            //Async loop
            while (!stopToken.IsCancellationRequested)
            {
                await ListenForMsgs();
            }

            //Do your cleanup (e.g. Stop code) here
            _consumer.Unsubscribe();
        }

        public override void Dispose()
        {
            _logger.LogInformation("Consumer: disposing....");
            _consumer.OnMessage -= OnMessage;
            _consumer.Dispose();
            base.Dispose();
        }

        private async Task<bool> ListenForMsgs()
        {
            await Task.Run(() =>_consumer.Poll());
            return true;
        }

        private void OnMessage(object obj, Message<Null, String> msg)
        {
            _logger.LogInformation($"Received: Offset: {msg.Offset} Msg: {msg.Value}");
        }


    }
}