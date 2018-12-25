namespace Dados
{
    public class DiceFactory : IDiceFactory
    {
        public IDice CreateDice(int numberOfSides)
        {
            return new Dice(numberOfSides);
        }
    }
}
