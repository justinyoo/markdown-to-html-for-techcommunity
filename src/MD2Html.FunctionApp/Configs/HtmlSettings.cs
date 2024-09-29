namespace MD2Html.FunctionApp.Configs;

/// <summary>
/// This represents the app settings entity for HTML tags.
/// </summary>
public class HtmlSettings
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    public const string Name = "Html";

    /// <summary>
    /// Gets or sets the comma delimited list of HTML tags.
    /// </summary>
    public string? Tags { get; set; }
}
