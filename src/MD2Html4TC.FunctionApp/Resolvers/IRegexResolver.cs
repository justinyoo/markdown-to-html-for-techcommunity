using System.Text.RegularExpressions;

namespace MD2Html4TC.FunctionApp.Resolvers
{
    /// <summary>
    /// This provides interface to the <see cref="Regex"/> instance resolver class.
    /// </summary>
    public interface IRegexResolver
    {
        /// <summary>
        /// Gets the name of the resolver.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Searches the specified input string for all occurrences of a regular expression.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <returns>A collection of the <see cref="Match"/> objects found by the search. If no matches are found, the method returns an empty collection object.</returns>
        MatchCollection Matches(string input);

        /// <summary>
        /// In a specified input string, replaces all strings that match a regular expression pattern with a specified replacement string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <returns>A new string that is identical to the input string, except that the replacement string takes the place of each matched string. If the regular expression pattern is not matched in the current instance, the method returns the current instance unchanged.</returns>
        string Replace(string input, string replacement);
    }
}