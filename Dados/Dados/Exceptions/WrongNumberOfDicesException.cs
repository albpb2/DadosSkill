using System;
using System.Runtime.Serialization;

namespace Dados.Exceptions
{
    public class WrongNumberOfDicesException : Exception
    {
        public WrongNumberOfDicesException()
        {
        }

        public WrongNumberOfDicesException(string message) : base(message)
        {
        }

        public WrongNumberOfDicesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongNumberOfDicesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
