using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dados.Exceptions;
using System;

namespace Dados
{
    public class ThrowDiceIntentProcessor : IIntentProcessor
    {
        private const string NumberOfDicesSlotName = "NumberOfDices";
        private const string NumberOfSidesSlotName = "NumberOfSides";

        private IDiceRoller _diceRoller;
        private IIntegerNumberParser _integerNumberParser;

        public ThrowDiceIntentProcessor()
        {
            _diceRoller = new DiceRoller(new DiceFactory());
            _integerNumberParser = new IntegerNumberParser();
        }

        public IOutputSpeech ProcessIntent(IntentRequest intentRequest, ILambdaLogger logger)
        {
            var putputSpeech = new PlainTextOutputSpeech();

            try
            {
                var numberOfDices = GetNumberOfDices(intentRequest);
                var numberOfSides = GetNumberOfSides(intentRequest);

                var totalPoints = _diceRoller.RollDices(numberOfDices, numberOfSides);

                (putputSpeech as PlainTextOutputSpeech).Text = totalPoints.ToString();
            }
            catch (IntegerParseException ex)
            {
                logger.Log(ex.Message);
                (putputSpeech as PlainTextOutputSpeech).Text = "Por favor, indica números enteros";
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                (putputSpeech as PlainTextOutputSpeech).Text = "Se ha producido un error. Por favor, inténtalo de nuevo";
            }

            return putputSpeech;
        }

        private int GetNumberOfDices(IntentRequest intentRequest)
        {
            return int.Parse(intentRequest.Intent.Slots[NumberOfDicesSlotName].Value);
        }

        private int GetNumberOfSides(IntentRequest intentRequest)
        {
            if (intentRequest.Intent.Slots.ContainsKey(NumberOfSidesSlotName)
                && intentRequest.Intent.Slots[NumberOfSidesSlotName].Value != null)
            {
                var numberOfSidesText = intentRequest.Intent.Slots[NumberOfSidesSlotName].Value;
                if (!string.IsNullOrEmpty(numberOfSidesText))
                {
                    return int.Parse(numberOfSidesText);
                }
            }

            return Dice.DefaultSides;
        }
    }
}
