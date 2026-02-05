using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Producers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class EventPublisherService : IEventPublisherService
    {
        private readonly ILogger<EventPublisherService> _logger;

        private readonly IGenericEventProducer<string> _eventProducer;


        public EventPublisherService(ILogger<EventPublisherService> logger, IGenericEventProducer<string> eventProducer)
        {
            _logger = logger;
            _eventProducer = eventProducer;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, EventPublisherType eventType) where TEvent : class
        {
            try
            {
                _logger.LogInformation($"Publish EventType:{eventType}, eventBody: {@event}");

                var paclageContent = JsonConvert.SerializeObject(@event);

                await _eventProducer.ProduceAsync(paclageContent).ConfigureAwait(false);
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, $"Fail on PublishAsync Error: {ex.Message}");
                
            }
         
        }
    }
}