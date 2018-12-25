using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace Dados
{
    public interface IIntentProcessor
    {
        IOutputSpeech ProcessIntent(IntentRequest intentRequest, ILambdaLogger logger);
    }
}
