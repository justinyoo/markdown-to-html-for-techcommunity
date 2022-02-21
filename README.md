# Markdown to HTML for Posts at Microsoft Tech Community #

This provides an Azure Functions app that converts a markdown document to an HTML one for Microsoft Tech Community posts.


## Prerequisites ##

* .NET 6 LTS
* Azure Functions Core Tools v4


## Getting Started ##

1. Clone this repository.
2. Run the function app locally.
3. Send a POST request to `http://localhost:7071/api/convert/md/to/html` with the markdown document as the body.
4. Get the HTML converted result and put the result to the HTML editor at Tech Community.

