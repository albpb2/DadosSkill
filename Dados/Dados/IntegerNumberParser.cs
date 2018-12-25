using Dados.Exceptions;

namespace Dados
{
    public class IntegerNumberParser : IIntegerNumberParser
    {
        public int Parse(string number)
        {
            if (!int.TryParse(number, out var parsedNumber))
            {
                throw new IntegerParseException(number);
            }

            return parsedNumber;
        }
    }
}
