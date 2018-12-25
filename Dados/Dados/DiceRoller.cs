namespace Dados
{
    public class DiceRoller : IDiceRoller
    {
        private IDiceFactory _diceFactory;

        public DiceRoller(IDiceFactory diceFactory)
        {
            _diceFactory = diceFactory;
        }

        public long RollDices(int numberOfDices, int numberOfSides)
        {
            var dice = _diceFactory.CreateDice(numberOfSides);

            var result = 0;
            
            for(var i = 0; i < numberOfDices; i++)
            {
                result += dice.Roll();
            }

            return result;
        }
    }
}
