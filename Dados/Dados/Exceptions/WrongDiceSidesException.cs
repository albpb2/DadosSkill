using System;
using System.Runtime.Serialization;

namespace Dados.Exceptions
{
    public class WrongDiceSidesException : Exception
    {
        public WrongDiceSidesException()
        {
        }

        public WrongDiceSidesException(string message) : base(message)
        {
        }

        public WrongDiceSidesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDiceSidesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
