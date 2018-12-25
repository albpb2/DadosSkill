using NSubstitute;
using Xunit;

namespace Dados.Tests
{
    public class DiceRollerTests
    {
        private DiceRoller _diceRoller;
        private IDiceFactory _diceFactory;
        private IDice _dice;

        public DiceRollerTests()
        {
            _diceFactory = NSubstitute.Substitute.For<IDiceFactory>();
            _dice = Substitute.For<IDice>();
            _diceFactory.CreateDice(Arg.Any<int>()).Returns(_dice);

            _diceRoller = new DiceRoller(_diceFactory);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(3)]
        [InlineData(1)]
        public void RollSpecifiedNumberOfDices(int numberOfDices)
        {
            _diceRoller.RollDices(numberOfDices, 6);

            _dice.Received(numberOfDices).Roll();
        }

        [Theory]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(20)]
        public void CreateDiceWithSpecifiedNumberOfSides(int numberOfSides)
        {
            _diceRoller.RollDices(2, numberOfSides);

            _diceFactory.Received(1).CreateDice(numberOfSides);
        }
    }
}
