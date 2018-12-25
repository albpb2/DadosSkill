using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dados.Exceptions;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Dados
{
    public class Function
    {
        private const string NumberOfDicesSlotName = "NumberOfDices";
        private const string NumberOfSidesSlotName = "NumberOfSides";

        private IDiceRoller _diceRoller;
        private IIntegerNumberParser _integerNumberParser;

        public Function()
        {
            _diceRoller = new DiceRoller(new DiceFactory());
            _integerNumberParser = new IntegerNumberParser();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, abre dados");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "¿Qué dado quieres tirar?";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Cerrando dados";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        log.LogLine($"AMAZON.StopIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Cerrando dados";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Puedes decir algo como 'Alexa, tira dos dados de diez caras";
                        break;
                    case "ThrowDiceIntent":
                        log.LogLine($"ThrowDiceIntent sent");
                        innerResponse = ProcessDiceRoll(intentRequest, log);
                        break;
                    default:
                        log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "No te he entendido, ¿puedes repetir?";
                        break;
                }
            }
            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            return response;
        }

        private PlainTextOutputSpeech ProcessDiceRoll(IntentRequest intentRequest, ILambdaLogger logger)
        {
            var innerResponse = new PlainTextOutputSpeech();

            try
            {
                var numberOfDices = GetNumberOfDices(intentRequest);
                var numberOfSides = GetNumberOfSides(intentRequest);

                var totalPoints = _diceRoller.RollDices(numberOfDices, numberOfSides);

                (innerResponse as PlainTextOutputSpeech).Text = totalPoints.ToString();
            }
            catch (IntegerParseException ex)
            {
                logger.Log(ex.Message);
                (innerResponse as PlainTextOutputSpeech).Text = "Por favor, indica números enteros";
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                (innerResponse as PlainTextOutputSpeech).Text = "Se ha producido un error. Por favor, inténtalo de nuevo";
            }

            return innerResponse;
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
