using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using web_server.source.WebServer;

namespace web_server.source.ThreadKeyBoard
{
    internal class ThreadKeyBoard
    {
        private Thread _thread;
        private bool _exit = false;
        private WebHead _head;
        public ThreadKeyBoard(ref WebHead head)
        {
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
            _head = head;
        }
        public bool GetExit() => _exit;
       
        private void Run()
        {
            while (!_exit)
            {
                string command = Console.ReadLine();
                if (command.Equals("close") || command.Equals("exit")) {
                    _exit = true;
                    _head.Close();
                }
            }
        }
        public void Close()
        {
            if (_thread != null && _thread.IsAlive)
                _thread.Join();
        }
    }
}
