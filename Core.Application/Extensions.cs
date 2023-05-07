using System.Text.RegularExpressions;

namespace Core.Application
{
    public static class Extensions
    {

        public static string GenerateUnique(this string originalFileName)
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var tokenAsName = new string(Enumerable.Repeat(chars, 10).Select(x => x[random.Next(x.Length)]).ToArray());

            var regx = new Regex("\\.[A-z]{3,4}$");
            var matchs = regx.Matches(originalFileName);

            if (matchs.Count < 1) throw new Exception("A Extensão do Ficheiro não se encontra no formato correcto");
            var ext = matchs[0].ToString();

            var finalFileName = $"file_{tokenAsName.ToLower()}{ext}";
            return finalFileName;
        }
    }
}
