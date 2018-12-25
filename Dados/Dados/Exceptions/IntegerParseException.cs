using System;
using System.Runtime.Serialization;

namespace Dados.Exceptions
{
    public class IntegerParseException : Exception
    {
        public IntegerParseException(string numberToParse) : base(CreateMessage(numberToParse))
        {
        }

        protected IntegerParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string CreateMessage(string numberToParse)
        {
            return $"It was not possible to parse {numberToParse} into an integer.";
        }
    }
}
