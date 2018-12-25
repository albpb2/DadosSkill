using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace Dados
{
    public interface IIntentProcessor
    {
        SkillResponse ProcessIntent(IntentRequest intentRequest, ILambdaLogger logger);
    }
}
