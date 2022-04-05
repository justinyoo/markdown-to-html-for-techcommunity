using System.Threading.Tasks;

using MD2Html4TC.FunctionApp.Helpers;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace MD2Html4TC.FunctionApp.Services
{
    /// <summary>
    /// This provides interfaces to <see cref="MarkdownService"/>.
    /// </summary>
    public interface IMarkdownService
    {
        /// <summary>
        /// Converts the markdown document to HTML with extra treatment for TechCommunity specific tags.
        /// </summary>
        /// <param name="markdown">Markdown document.</param>
        /// <returns>HTML converted document.</returns>
        Task<string> ConvertToHtmlAsync(string markdown);
    }

    /// <summary>
    /// This represents the service entity to handle markdown.
    /// </summary>
    public class MarkdownService : IMarkdownService
    {
        private readonly IMarkdownHelper _md;
        private readonly IEmojiHelper _emoji;

        public MarkdownService(IMarkdownHelper md, IEmojiHelper emoji)
        {
            this._md = md.ThrowIfNullOrDefault();
            this._emoji = emoji.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public async Task<string> ConvertToHtmlAsync(string markdown)
        {
            var html = await this._md.GetHtmlAsync(markdown).ConfigureAwait(false);
            html = await this._emoji.ConvertEmojiAsync(html).ConfigureAwait(false);

            return html;
        }
    }
}