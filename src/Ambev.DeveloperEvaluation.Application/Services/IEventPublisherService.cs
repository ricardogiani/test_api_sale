using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public interface IEventPublisherService
    {                
        Task PublishAsync<TEvent>(TEvent @event, EventPublisherType eventType) where TEvent : class;
    }
}
