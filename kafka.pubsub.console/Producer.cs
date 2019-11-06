using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace kafka.pubsub.console
{
    public class Producer : IDisposable
    {
        string _topic;

        Producer<Null, string> _producer;

        public Producer(string endpoint, string topic)
        {
            _topic = topic;
            _producer = new Producer<Null, string>(
                new Dictionary<string, object> { { "bootstrap.servers", endpoint } },
                null,
                new StringSerializer(Encoding.UTF8));
        }

        public void Dispose()
        {
            Console.WriteLine("Producer: disposing....");
            _producer = null;
        }

        public void SendMsg(string message)
        {
            _producer.ProduceAsync(_topic, null, message);
        }
    }
}