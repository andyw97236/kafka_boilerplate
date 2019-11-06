using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace kafka.pubsub.console
{
    public class Consumer : IDisposable
    {
        string _topic;
        Consumer<Null, string> _consumer;

        bool _cancel;

        public Consumer(string endpoint, string topic, ref bool cancel)
        {
            _topic = topic;
            _cancel = cancel;
            _consumer = new Consumer<Null, string>(
                new Dictionary<string, object>
            {
                { "group.id", "myconsumer" }
                ,{ "bootstrap.servers", endpoint }
                ,{ "auto.offset.reset", "earliest" } // if you want to read from the beginning of time
            },
            null,
            new StringDeserializer(Encoding.UTF8));
        }

        public void Dispose()
        {
            Console.WriteLine("Consumer: disposing....");
            _consumer = null;
        }

        public void ReceiveMsgs()
        {
            Console.WriteLine("Consumer: listening for msgs....");

            // Subscribe to the OnMessage event
            _consumer.OnMessage += (obj, msg) =>
            {
                Console.WriteLine($"Received: Offset: {msg.Offset} Msg: {msg.Value}");
            };

            // Subscribe to the Kafka topic
            _consumer.Subscribe(new List<string>() { _topic });

            // Poll for messages
            while (!_cancel)
            {
                _consumer.Poll();
            }

            Console.WriteLine("Consumer: no longer listening for msgs....");
        }
    }
}