using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Amazon.Lambda.Core;
using Dados.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dados.Tests
{
    public class RollDiceIntentProcessorTests
    {
        private readonly RollDiceIntentProcessor _rollDiceIntentProcessor;
        private readonly IIntegerNumberParser _integerNumberParser;
        private readonly IDiceRoller _diceRoller;

        public RollDiceIntentProcessorTests()
        {
            _integerNumberParser = Substitute.For<IIntegerNumberParser>();   
            _diceRoller = Substitute.For<IDiceRoller>();
            _rollDiceIntentProcessor = new RollDiceIntentProcessor(_diceRoller, _integerNumberParser);
        }

        [Fact]
        public void EndSession_WhenTheRequestIsSuccessful()
        {
            var intentRequest = new IntentRequest();
            intentRequest.Intent = new Intent();
            intentRequest.Intent.Slots = new Dictionary<string, Slot>();
            intentRequest.Intent.Slots["NumberOfDices"] = new Slot();
            intentRequest.Intent.Slots["NumberOfDices"].Value = "1";
            intentRequest.Intent.Slots["NumberOfSides"] = new Slot();
            intentRequest.Intent.Slots["NumberOfSides"].Value = "6";

            var logger = Substitute.For<ILambdaLogger>();

            _integerNumberParser.Parse(Arg.Any<string>()).Returns(6);

            var response = _rollDiceIntentProcessor.ProcessIntent(intentRequest, logger);

            response.Response.ShouldEndSession.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(Exceptions))]
        public void NotEndSession_WhenTheRequestIsNotSuccessful(Exception exception)
        {
            var intentRequest = new IntentRequest();
            intentRequest.Intent = new Intent();
            intentRequest.Intent.Slots = new Dictionary<string, Slot>();
            intentRequest.Intent.Slots["NumberOfDices"] = new Slot();
            intentRequest.Intent.Slots["NumberOfDices"].Value = "1";
            intentRequest.Intent.Slots["NumberOfSides"] = new Slot();
            intentRequest.Intent.Slots["NumberOfSides"].Value = "6";

            var logger = Substitute.For<ILambdaLogger>();

            _integerNumberParser.Parse(Arg.Any<string>()).Throws(exception);

            var response = _rollDiceIntentProcessor.ProcessIntent(intentRequest, logger);

            response.Response.ShouldEndSession.Should().BeFalse();
        }


        public static IEnumerable<object[]> Exceptions =>
        new List<object[]>
        {
            new object[] { new IntegerParseException("1") },
            new object[] { new WrongDiceSidesException() },
            new object[] { new WrongNumberOfDicesException() },
            new object[] { new InvalidOperationException() },
            new object[] { new Exception() },
        };
    }
}
