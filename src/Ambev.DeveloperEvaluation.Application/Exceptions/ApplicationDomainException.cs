using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Exceptions
{
    public class ApplicationDomainException : Exception
    {
        public ApplicationDomainException()
        {
        }

        public ApplicationDomainException(string? message) : base(message)
        {
        }

        public ApplicationDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}