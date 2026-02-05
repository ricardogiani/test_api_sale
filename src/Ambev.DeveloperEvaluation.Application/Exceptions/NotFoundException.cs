using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Exceptions
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}