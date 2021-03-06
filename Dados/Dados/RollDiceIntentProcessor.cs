﻿using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dados.Exceptions;
using System;
using System.Collections.Generic;

namespace Dados
{
    public class RollDiceIntentProcessor : IIntentProcessor
    {
        private const string NumberOfDicesSlotName = "NumberOfDices";
        private const string NumberOfSidesSlotName = "NumberOfSides";
        private const string DiceRollAudioUrl = @"https://s3-us-west-2.amazonaws.com/albpb2-alexasounds/Dice/Dice.mp3";

        private readonly List<string> _responseFormats = new List<string>()
        {
            "El resultado es un {0}",
            "{0}",
            "Has sacado un {0}"
        };

        private IDiceRoller _diceRoller;
        private IIntegerNumberParser _integerNumberParser;

        public RollDiceIntentProcessor()
        {
            _diceRoller = new DiceRoller(new DiceFactory());
            _integerNumberParser = new IntegerNumberParser();
        }

        public RollDiceIntentProcessor(IDiceRoller diceRoller, IIntegerNumberParser integerNumberParser)
        {
            _diceRoller = diceRoller;
            _integerNumberParser = integerNumberParser;
        }

        public SkillResponse ProcessIntent(IntentRequest intentRequest, ILambdaLogger logger)
        {
            SkillResponse response = new SkillResponse();
            var outputSpeech = new PlainTextOutputSpeech();
            response.Response = new ResponseBody();

            try
            {
                var numberOfDices = GetNumberOfDices(intentRequest);
                DicesAmountValidator.Validate(numberOfDices);
                var numberOfSides = GetNumberOfSides(intentRequest);
                DiceSidesValidator.Validate(numberOfSides);

                var totalPoints = _diceRoller.RollDices(numberOfDices, numberOfSides);

                var speech = new SsmlOutputSpeech();
                var speechInnerText = AudioTagCreator.Create(DiceRollAudioUrl) + FormatRollResult(totalPoints.ToString());
                speech.Ssml = SpeechSsmlGenerator.Generate(speechInnerText);

                response = ResponseBuilder.Tell(speech);
                response.Response.ShouldEndSession = true;
            }
            catch (IntegerParseException ex)
            {
                logger.Log(ex.Message);
                (outputSpeech as PlainTextOutputSpeech).Text = "Por favor, indica números enteros";
                response.Response.OutputSpeech = outputSpeech;
                response.Response.ShouldEndSession = false;
            }
            catch (WrongDiceSidesException ex)
            {
                logger.Log(ex.Message);
                (outputSpeech as PlainTextOutputSpeech).Text = ex.Message;
                response.Response.OutputSpeech = outputSpeech;
                response.Response.ShouldEndSession = false;
            }
            catch (WrongNumberOfDicesException ex)
            {
                logger.Log(ex.Message);
                (outputSpeech as PlainTextOutputSpeech).Text = ex.Message;
                response.Response.OutputSpeech = outputSpeech;
                response.Response.ShouldEndSession = false;
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                (outputSpeech as PlainTextOutputSpeech).Text = "Se ha producido un error. Por favor, inténtalo de nuevo";
                response.Response.OutputSpeech = outputSpeech;
                response.Response.ShouldEndSession = false;
            }

            response.Version = "1.0";

            return response;
        }

        private int GetNumberOfDices(IntentRequest intentRequest)
        {
            return _integerNumberParser.Parse(intentRequest.Intent.Slots[NumberOfDicesSlotName].Value);
        }

        private int GetNumberOfSides(IntentRequest intentRequest)
        {
            if (intentRequest.Intent.Slots.ContainsKey(NumberOfSidesSlotName)
                && intentRequest.Intent.Slots[NumberOfSidesSlotName].Value != null)
            {
                var numberOfSidesText = intentRequest.Intent.Slots[NumberOfSidesSlotName].Value;
                if (!string.IsNullOrEmpty(numberOfSidesText))
                {
                    return _integerNumberParser.Parse(numberOfSidesText);
                }
            }

            return Dice.DefaultSides;
        }

        private string FormatRollResult(string response)
        {
            return string.Format(_responseFormats.GetRandomElement(), response);
        }
    }
}
