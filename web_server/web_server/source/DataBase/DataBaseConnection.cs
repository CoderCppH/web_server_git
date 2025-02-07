using System.Data.SqlClient;
using System.Data.SQLite;
namespace web_server.source.DataBase
{
    internal class DataBaseConnection
    {
        private SQLiteConnection _connection;
        private string _connectionString = "Data Source = db.db";
        public DataBaseConnection() {
           _connection = new SQLiteConnection(_connectionString);
        }
        public void OpenConnection() {
            if (_connection.State == System.Data.ConnectionState.Closed) 
                _connection.Open();
        }
        public void CloseConnection() {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
        }
        public SQLiteConnection GetConnection() => _connection;
    }
}
