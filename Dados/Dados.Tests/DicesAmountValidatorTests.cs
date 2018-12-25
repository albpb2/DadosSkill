using Dados.Exceptions;
using FluentAssertions;
using System;
using Xunit;

namespace Dados.Tests
{
    public class DicesAmountValidatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(20)]
        public void NotThrowException_WhenTheAmountOfDicesIsCorrect(int numberOfDices)
        {
            Action functionUnderTest = () => DicesAmountValidator.Validate(numberOfDices);

            functionUnderTest.Should().NotThrow();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(21)]
        public void ThrowException_WhenTheAmountOfDicesIsNotCorrect(int numberOfDices)
        {
            Action functionUnderTest = () => DicesAmountValidator.Validate(numberOfDices);

            functionUnderTest.Should().Throw<WrongNumberOfDicesException>();
        }
    }
}
