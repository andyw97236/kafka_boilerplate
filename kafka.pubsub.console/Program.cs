using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace kafka.pubsub.console
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";

            // The Kafka topic we'll be using
            string kafkaTopic = "testtopic";

            // Handle Cancel Keypress 
            bool cancelled = false;
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cancelled = true;
            };

            var producer = new Producer(kafkaEndpoint, kafkaTopic);
            var consumer = new Consumer(kafkaEndpoint, kafkaTopic, ref cancelled);

            Task.Run(() =>
                {
                    consumer.ReceiveMsgs();
                }
            );

            Console.WriteLine("Ctrl-C to exit.");

            while (!cancelled)
            {
                var line = Console.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    producer.SendMsg(line);
                }
                else
                {
                    cancelled = true;
                }
            }

            Console.WriteLine("Exiting....");

            producer.Dispose();
            consumer.Dispose();

            System.Environment.Exit(1);
        }
    }
}