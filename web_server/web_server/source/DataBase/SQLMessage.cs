using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace web_server.source.DataBase
{
    internal class SQLMessage
    {
        public int id;

        public string message = string.Empty;

        public string date = string.Empty;

        public string time = string.Empty;

        public int id_user;
        
        public static DataBaseConnection connect;
        public static void CreateTable() 
        {
            string sql_command = "CREATE TABLE IF NOT EXISTS message_data(id INTEGER PRIMARY KEY AUTOINCREMENT, message TEXT NOT NULL, id_user INTEGER NOT NULL, date TEXT NOT NULL, time TEXT NOT NULL, FOREIGN KEY (id_user)  REFERENCES user_data (id) )";
            using (SQLiteCommand command = new SQLiteCommand(sql_command, connect.GetConnection())) 
                command.ExecuteNonQuery();
        }
        public void Create() 
        {
            string sql_command = "INSERT INTO message_data (message, date, time, id_user) VALUES (@message, @date, @time, @id_user)";
            using (SQLiteCommand command = new SQLiteCommand(sql_command, connect.GetConnection())) 
            {
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                command.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                command.Parameters.AddWithValue("@id_user", id_user);
                command.ExecuteNonQuery();
            }
        }
        public static List<SQLMessage> Select() 
        {
            List<SQLMessage> messagers = new List<SQLMessage>();
            string sql_command = "SELECT * FROM message_data";
            using (SQLiteDataReader read = new SQLiteCommand(sql_command, connect.GetConnection()).ExecuteReader())
                while (read.Read())
                    messagers.Add(new SQLMessage() { id = read.GetInt32(0), message = read.GetString(1), id_user = read.GetInt32(2), date = read.GetString(3), time = read.GetString(4) });
            return messagers;
        }
    }
}
