namespace Dados
{
    public class SpeechSsmlGenerator
    {
        private const string OpeningTag = "<speak>";
        private const string ClosingTag = "</speak>";

        public static string Generate(string innerText) => $"{OpeningTag}{innerText}{ClosingTag}";
    }
}
