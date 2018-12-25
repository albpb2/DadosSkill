using Dados.Exceptions;
using FluentAssertions;
using System;
using Xunit;

namespace Dados.Tests
{
    public class DiceSidesValidatorTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(20)]
        [InlineData(100)]
        public void NotThrowException_WhenTheNumberOfSidesIsCorrect(int numberOfSides)
        {
            Action functionUnderTest = () => DiceSidesValidator.Validate(numberOfSides);

            functionUnderTest.Should().NotThrow();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(101)]
        public void ThrowException_WhenTheNumberOfSidesIsNotCorrect(int numberOfSides)
        {
            Action functionUnderTest = () => DiceSidesValidator.Validate(numberOfSides);

            functionUnderTest.Should().Throw<WrongDiceSidesException>();
        }
    }
}
