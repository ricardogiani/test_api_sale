using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.EventBus.Producers
{
    public class KafkaProducer<TValue> : IGenericEventProducer<TValue>
    {
        private readonly IProducer<Null, TValue> _producer;
        private readonly string _topic;
        private ILogger<KafkaProducer<TValue>> _logger;

        public KafkaProducer(ILogger<KafkaProducer<TValue>> logger, IConfiguration configuration)
        {
            _logger = logger;
            string kafkaBootstrapServers = configuration["Kafka:BootstrapServers"];
            _topic = configuration["Kafka:Topic"];

            var config = new ProducerConfig { BootstrapServers = kafkaBootstrapServers };
            _producer = new ProducerBuilder<Null, TValue>(config).Build();
        }

        public async Task ProduceAsync(TValue message)
        {
            try
            {
                _logger.LogInformation($"[ProduceAsync]: try send data: {message}");
                var result = await _producer.ProduceAsync(_topic, new Message<Null, TValue> { Value = message });
                _logger.LogInformation($"Mensagem enviada para o Kafka: {result.TopicPartitionOffset}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro ao enviar mensagem para o Kafka: {ex.Message} - {ex.StackTrace}");
            }
        }

    }

}