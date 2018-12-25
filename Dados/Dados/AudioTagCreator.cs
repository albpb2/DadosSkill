namespace Dados
{
    public class AudioTagCreator
    {
        public static string Create(string sourceUrl) => $@"<audio src=""{sourceUrl}""/>";
    }
}
