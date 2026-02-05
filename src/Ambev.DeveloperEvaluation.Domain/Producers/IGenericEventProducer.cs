using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Producers
{
    public interface IGenericEventProducer<TValue>
    {
        Task ProduceAsync(TValue message);
    }
}