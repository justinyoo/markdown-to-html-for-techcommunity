using System.Text.RegularExpressions;

using Markdig;

using MD2Html.Extensions;

namespace MD2Html.Services;

/// <summary>
/// This provides interfaces to <see cref="ConverterService"/> class.
/// </summary>
public interface IConverterService
{
    /// <summary>
    /// Converts the markdown document to HTML document.
    /// </summary>
    /// <param name="markdown">Markdown document.</param>
    /// <param name="tc">Value indicating whether the markdown file is for Tech Community or not.</param>
    /// <param name="p">Value indicating whether the extra empty paragraph is necessary in between paragraphs not.</param>
    /// <param name="tags">List of tags to put extra empty paragraph.</param>
    /// <returns>Returns the HTML document converted from the markdown document.</returns>
    Task<string> ConvertToHtmlAsync(string markdown, bool tc = false, bool p = false, IEnumerable<string>? tags = null);
}

/// <summary>
/// This represents the service entity for document conversion.
/// </summary>
/// <param name="regex"><see cref="Regex"/> instance.</param>
public class ConverterService (Regex regex): IConverterService
{
    private readonly Regex _regex = regex ?? throw new ArgumentNullException(nameof(regex));

    /// <inheritdoc />
    public async Task<string> ConvertToHtmlAsync(string markdown, bool tc = false, bool p = false, IEnumerable<string>? tags = null)
    {
        var pipeline = new MarkdownPipelineBuilder()
                           .UseAdvancedExtensions()
                           .UseEmojiAndSmiley()
                           .UseYamlFrontMatter()
                           .Build();

        var html = Markdown.ToHtml(markdown, pipeline);
        if (tc == false)
        {
            return html;
        }

        if (tags?.Any() == false)
        {
            return html;
        }

        html = this._regex.Replace(html, "<li-code lang=\"$1\">")
                          .Replace("</code></pre>", "</li-code>");
        if (p == true)
        {
            html = html.AddEmptyParagraph(tags!, tags!);
        }

        return await Task.FromResult(html).ConfigureAwait(false);
    }
}
