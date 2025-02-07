using web_server.source.Resource;
using web_server.source.ThreadKeyBoard;
using web_server.source.WebServer;

namespace web_server {
    class Program {
        static string GetResourcesPath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource");
        static ThreadKeyBoard _t_key;
        static void CreateFileIfNotExists(string path_file) {
            if (!Directory.Exists(GetResourcesPath()))
                Directory.CreateDirectory(GetResourcesPath());
            if (!File.Exists(path_file))
                File.Create(path_file);
        }
        static void Init() 
        {
            string html_default = Path.Combine(GetResourcesPath(), Keys.KEY_HTML_DEFAULT + ".html");
            CreateFileIfNotExists(html_default);
            Values.HTML_FILES.Add(Keys.KEY_HTML_DEFAULT, html_default);
        }
        static async Task Main(string[] args) {
            Init();
            Console.WriteLine("server start");
            WebHead head = new WebHead("http://127.0.0.1:8888/");
            _t_key = new ThreadKeyBoard(ref head);
            await head.StartLoopAsyncBody();
        }
    }
}