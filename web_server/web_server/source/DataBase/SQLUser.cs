using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace web_server.source.DataBase
{
    internal class SQLUser
    {
        public static DataBaseConnection connect;
        public int id { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }

        public static void CreateTable() {
            string sql_comand = "CREATE TABLE IF NOT EXISTS user_data(id INTEGER PRIMARY KEY AUTOINCREMENT, fullname TEXT NOT NULL, phone TEXT NOT NULL)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql_comand, connect.GetConnection()))
                cmd.ExecuteNonQuery();
        }
        public void Create(){
            Console.WriteLine("create");
            string sql_command = "INSERT INTO user_data (fullname, phone) VALUES (@fullname, @phone)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql_command, connect.GetConnection())) {
                cmd.Parameters.AddWithValue("@fullname", fullname);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.ExecuteNonQuery();
            }
        }
        public static SQLUser? FindById(int id) 
        {
            SQLUser user = new SQLUser();
            string sql_command = "SELECT * FROM user_data WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(sql_command, connect.GetConnection()))
            {

                command.Parameters.AddWithValue("@id", id);
                var read = command.ExecuteReader();
                read.Read();
                user.id = read.GetInt32(0);
                user.fullname = read.GetString(1);
                user.phone = read.GetString(2);
            }
            return user;
        }
     
        public static List<SQLUser> Select() {
            List<SQLUser> users = new List<SQLUser>();
            string sql_command = "SELECT * FROM user_data";
            using (SQLiteDataReader read = new SQLiteCommand(sql_command, connect.GetConnection()).ExecuteReader()) 
                while (read.Read()) 
                    users.Add(new SQLUser() { id = read.GetInt32(0), fullname = read.GetString(1), phone = read.GetString(2) });
            return users;
        }
        public static SQLUser? Find(string phone) 
        {
            SQLUser r_user = null;
            var users = Select();
            foreach (SQLUser user in users) 
                if (user.phone.Equals(phone)) {
                    r_user = user;
                    break;
                }
            return r_user;
        }
    }
}
