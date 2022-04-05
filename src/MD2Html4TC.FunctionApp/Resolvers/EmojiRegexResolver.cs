using System.Text.RegularExpressions;

namespace MD2Html4TC.FunctionApp.Resolvers
{
    /// <summary>
    /// This represents the <see cref="Regex"/> instance resolver for emoji.
    /// </summary>
    public class EmojiRegexResolver : IRegexResolver
    {
        private static readonly Regex regex = new Regex(":(\\w+):", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <inheritdoc />
        public string Name { get; } = "Emoji";

        /// <inheritdoc />
        public MatchCollection Matches(string input)
        {
            return regex.Matches(input);
        }

        /// <inheritdoc />
        public string Replace(string input, string replacement)
        {
            return regex.Replace(input, replacement);
        }
    }
}