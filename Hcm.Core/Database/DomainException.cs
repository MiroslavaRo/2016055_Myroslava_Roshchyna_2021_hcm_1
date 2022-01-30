using System;

namespace Hcm.Core.Database
{
    public class DomainException : Exception
    {
        public DomainException(string message) 
            : base(message)
        {
        }
    }
}
