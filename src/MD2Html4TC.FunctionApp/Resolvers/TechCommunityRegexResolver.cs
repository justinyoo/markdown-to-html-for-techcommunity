using System.Text.RegularExpressions;

namespace MD2Html4TC.FunctionApp.Resolvers
{
    /// <summary>
    /// This represents the <see cref="Regex"/> instance resolver for Tech Community.
    /// </summary>
    public class TechCommunityRegexResolver : IRegexResolver
    {
        private static readonly Regex regex = new Regex("\\<pre\\>\\<code class=\"language\\-(.+)\"\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <inheritdoc />
        public string Name { get; } = "TechCommunity";

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