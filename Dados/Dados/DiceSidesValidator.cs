using Dados.Exceptions;

namespace Dados
{
    public class DiceSidesValidator
    {
        private const int MinSides = 2;
        private const int MaxSides = 100;

        public static void Validate(int numberOfSides)
        {
            if (numberOfSides < 2 || numberOfSides > MaxSides)
            {
                throw new WrongDiceSidesException($"El dado tiene que tener entre {MinSides} y {MaxSides} caras");
            }
        }
    }
}
