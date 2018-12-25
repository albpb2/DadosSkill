using System;

namespace Dados
{
    public class Dice : IDice
    {
        public const int DefaultSides = 6;
        private const int MinValue = 1;

        private readonly int _sides;
        private readonly Random _random;

        public Dice(int sides)
        {
            _sides = sides;
            _random = new Random();
        }

        public Dice() : this(DefaultSides)
        {
        }

        public int Roll()
        {
            return _random.Next(MinValue, 6);
        }
    }
}
