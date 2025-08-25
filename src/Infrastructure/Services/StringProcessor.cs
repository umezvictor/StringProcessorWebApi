using Application.Abstractions.Services;
using System.Text;

namespace Infrastructure.Services
{
    public class StringProcessor : IStringProcessor
    {
        public string ProcessString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            string generatedString = string.Empty;
            string sorted = new string(input.OrderBy(c => c).ToArray());

            Dictionary<char, int> result = new Dictionary<char, int>();

            foreach (char c in sorted)
            {
                if (!result.ContainsKey(c))
                {
                    result.Add(c, 1);
                }
                else
                {
                    result[c]++;
                }
            }

            foreach (var keyValuePair in result)
            {
                generatedString += $"{keyValuePair.Key}{keyValuePair.Value}";
            }

            return $"{generatedString}/{Convert.ToBase64String(Encoding.UTF8.GetBytes(input))}";
        }
    }
}
