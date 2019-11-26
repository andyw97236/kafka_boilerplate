using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace kafka.pubsub.console
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaConfiguration = new KafkaConfiguration();
            configuration.GetSection("Kafka").Bind(kafkaConfiguration);

            if (!kafkaConfiguration.Enabled)
            {
                return services;
            }

            services.AddSingleton(kafkaConfiguration);
            services.AddHostedService<ConsumerService>();
            services.AddHostedService<ProducerService>();
            return services;
        }
    }
}
