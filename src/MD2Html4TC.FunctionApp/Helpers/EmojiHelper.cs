using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Resolvers;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Newtonsoft.Json;

namespace MD2Html4TC.FunctionApp.Helpers
{
    /// <summary>
    /// This provides interfaces to <see cref="EmojiHelper"/>.
    /// </summary>
    public interface IEmojiHelper
    {
        /// <summary>
        /// Converts the markdown document to HTML with extra treatment for TechCommunity specific tags.
        /// </summary>
        /// <param name="input">Input HTML document.</param>
        /// <returns>HTML document that emojis are converted.</returns>
        Task<string> ConvertEmojiAsync(string input);
    }

    public class EmojiHelper : IEmojiHelper
    {
        private readonly EmojiSettings _settings;
        private readonly HttpClient _http;
        private readonly IRegexResolver _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmojiHelper"/> class.
        /// </summary>
        /// <param name="settings"><see cref="EmojiSettings"/> instance.</param>
        /// <param name="http"><see cref="HttpClient"/> instance.</param>
        /// <param name="regexes">List of <see cref="IRegexResolver"/> isntances.</param>
        public EmojiHelper(EmojiSettings settings, HttpClient http, IEnumerable<IRegexResolver> regexes)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = http.ThrowIfNullOrDefault();
            this._regex = regexes.SingleOrDefault(p => p.Name == "Emoji").ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public async Task<string> ConvertEmojiAsync(string input)
        {
            var result = input;
            var emojis = await this.GetEmojisAsync();

            var matches = this._regex.Matches(input).ToList();
            foreach (var match in matches)
            {
                var key = match.Value.Trim(':');
                if (emojis.ContainsKey(key))
                {
                    result = result.Replace(match.Value, $"<img src=\"{emojis[key]}\" alt=\"{key}\" title=\"{key}\" style=\"display:inline;width:16px;height:16px;\">");
                }
            }

            return result;
        }

        private async Task<Dictionary<string, string>> GetEmojisAsync()
        {
            this._http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("md2html", "1.0"));
            this._http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            var json = await this._http.GetStringAsync(this._settings.ApiUrl).ConfigureAwait(false);

            var emojis = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return emojis;
        }
    }
}