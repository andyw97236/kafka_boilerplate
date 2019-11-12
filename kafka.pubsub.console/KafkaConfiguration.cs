using System;
using System.Collections.Generic;
using System.Text;

namespace kafka.pubsub.console
{
    public class KafkaConfiguration
    {
        public bool Enabled { get; set; } = false;

        public string Endpoint { get; set; } = String.Empty;

        public string Topic { get; set; } = String.Empty;

        public string Port { get; set; } = String.Empty;
    }
}
