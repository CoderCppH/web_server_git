using web_server.source.Resource;

namespace web_server.source.WebServer
{
    internal class WebBody
    {
        public WebBody() { }
        private string LoadFileInText(string key_html) => File.ReadAllText(Values.HTML_FILES[key_html]);
        public string GetDefaultHtmlBody() {
            return LoadFileInText(Keys.KEY_HTML_DEFAULT);
        }
    }
}
