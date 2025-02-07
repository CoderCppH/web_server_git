using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_server.source.DataBase
{
    internal class DBC_SqlCommand
    {
        public DBC_SqlCommand(DataBaseConnection connect)
        {
            connect.OpenConnection();
            SQLUser.connect = connect;
            SQLUser.CreateTable();
            SQLMessage.connect = connect;
            SQLMessage.CreateTable();
        }
    }
}
