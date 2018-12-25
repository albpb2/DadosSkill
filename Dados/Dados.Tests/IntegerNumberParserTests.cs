using Dados.Exceptions;
using FluentAssertions;
using System;
using Xunit;

namespace Dados.Tests
{
    public class IntegerNumberParserTests
    {
        private readonly IntegerNumberParser _integerNumberParser;

        public IntegerNumberParserTests()
        {
            _integerNumberParser = new IntegerNumberParser();
        }

        [Theory]
        [InlineData("3", 3)]
        [InlineData("20", 20)]
        public void ParseIntegerNumbers(string number, int expectedResult)
        {
            var result = _integerNumberParser.Parse(number);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("3.5")]
        [InlineData("NAN")]
        public void ThrowIntegerParseException_WhenTheNumberIsNotInteger(string number)
        {
            Action functionUnderTest = () => _integerNumberParser.Parse(number);

            functionUnderTest.Should().Throw<IntegerParseException>().Which.Message.Should().Be(
                $"It was not possible to parse {number} into an integer.");
        }
    }
}
