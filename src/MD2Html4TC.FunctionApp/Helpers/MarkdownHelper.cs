using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Markdig;

using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Extensions;
using MD2Html4TC.FunctionApp.Resolvers;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace MD2Html4TC.FunctionApp.Helpers
{
    /// <summary>
    /// This provides interfaces to <see cref="MarkdownHelper"/>.
    /// </summary>
    public interface IMarkdownHelper
    {
        /// <summary>
        /// Converts the markdown document to HTML with extra treatment for TechCommunity specific tags.
        /// </summary>
        /// <param name="markdown">Markdown document.</param>
        /// <returns>HTML converted document.</returns>
        Task<string> GetHtmlAsync(string markdown);
    }

    /// <summary>
    /// This represents the helper entity to convert markdown to HTML.
    /// </summary>
    public class MarkdownHelper : IMarkdownHelper
    {
        private readonly ConverterSettings _settings;
        private readonly IRegexResolver _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownHelper"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="regexes">List of <see cref="IRegexResolver"/> isntances.</param>
        public MarkdownHelper(ConverterSettings settings, IEnumerable<IRegexResolver> regexes)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._regex = regexes.SingleOrDefault(p => p.Name == "TechCommunity").ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public async Task<string> GetHtmlAsync(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                   .UseAdvancedExtensions()
                   // .UseEmojiAndSmiley()
                   .UseYamlFrontMatter()
                   .Build();
            var html = Markdown.ToHtml(markdown, pipeline);

            html = this._regex.Replace(html, "<li-code lang=\"$1\">")
                              .Replace("</code></pre>", "</li-code>")

                              .AddEmptyParagraph(this._settings.HtmlTags, this._settings.HtmlTags)
                              ;

            return await Task.FromResult(html).ConfigureAwait(false);
        }
    }
}