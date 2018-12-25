using Dados.Exceptions;

namespace Dados
{
    public class DicesAmountValidator
    {
        private const int MinDices = 1;
        private const int MaxDices = 20;

        public static void Validate(int numberOfDices)
        {
            if (numberOfDices < MinDices || numberOfDices > MaxDices)
            {
                throw new WrongNumberOfDicesException($"Tira entre {MinDices} y {MaxDices} dados");
            }
        }
    }
}
