using Newtonsoft.Json;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using web_server.source.DataBase;
using web_server.source.JsonPatterns;

namespace web_server.source.WebServer
{
    internal class WebHead
    {
        private HttpListener _server;
        private WebBody _body;
        private DataBase.DataBaseConnection _db_connection;
        private DataBase.DBC_SqlCommand _sql_command;
        public WebHead(string input_url) 
        {

            _server = new HttpListener();
            _server.Prefixes.Add(input_url);
            _server.Start();

            _body = new WebBody();

            _db_connection = new DataBase.DataBaseConnection();
            _db_connection.OpenConnection();

            _sql_command = new DataBase.DBC_SqlCommand(_db_connection);
        }
        public async Task StartLoopAsyncBody()
        {
            bool exit_main_thread = false;
            while (!exit_main_thread) {
                HttpListenerContext context = default(HttpListenerContext);
                HttpListenerRequest request = default(HttpListenerRequest);
                HttpListenerResponse response = default(HttpListenerResponse);
                try
                {
                    context = await _server.GetContextAsync();
                    request = context.Request;
                    response = context.Response;
                }
                catch (Exception ex) { Console.WriteLine($"E_RG: {ex.Message}"); exit_main_thread = true; }
                string responseText = "";
                string html_raw = request?.RawUrl;

                if (html_raw != null)
                {
                    if (html_raw.Equals("/make_client"))
                    {
                        using (var input = request.InputStream)
                        {
                            byte[] buffer = new byte[request.ContentLength64];
                            input.ReadAsync(buffer, 0, buffer.Length);
                            string text = Encoding.UTF8.GetString(buffer);
                            Console.WriteLine($"text:= {text}");
                            SQLUser user = JsonConvert.DeserializeObject<SQLUser>(text);
                            user.Create();
                        }
                        responseText = "client";
                    }
                    else if (html_raw.Equals("/get_all_client"))
                    {
                        responseText = "";
                        List<SQLUser> users = SQLUser.Select();
                        foreach (SQLUser user in users)
                        {
                            responseText += $" {{id: {user.id} , fullname: \"{user.fullname}\" , phone: \"{user.phone}\" }} ";
                        }
                    }
                    else if (html_raw.Equals("/send_message"))
                    {

                        using (var input = request.InputStream)
                        {
                            byte[] buffer = new byte[request.ContentLength64];
                            input.ReadAsync(buffer, 0, buffer.Length);
                            string text = Encoding.UTF8.GetString(buffer);

                            p_g_messag g_message = JsonConvert.DeserializeObject<p_g_messag>(text);
                            SQLUser user = SQLUser.Find(g_message.phone);
                            SQLMessage sql_message = new SQLMessage();
                            if (user != null)
                            {
                                sql_message.message = g_message.message;
                                sql_message.id_user = user.id;
                                responseText = "success send message";
                                sql_message.Create();
                            }
                            else
                                responseText = "user not found";
                        }
                    }
                    else if (html_raw.Equals("/get_all_message"))
                    {
                        List<p_s_message> s_messgaers = new List<p_s_message>();
                        List<SQLMessage> sql_messagers = SQLMessage.Select();
                        foreach (SQLMessage i_message in sql_messagers)
                        {
                            p_s_message obj_p_s_message = new p_s_message();
                            var user = SQLUser.FindById(i_message.id_user);
                            obj_p_s_message.phone = user.phone;
                            obj_p_s_message.message = i_message.message;
                            obj_p_s_message.fullname = user.fullname;
                            obj_p_s_message.date = i_message.date;
                            obj_p_s_message.time = i_message.time;
                            s_messgaers.Add(obj_p_s_message);
                        }
                        responseText = JsonConvert.SerializeObject(s_messgaers);
                    }
                    else if (html_raw.Equals("/get_last_message")) 
                    {
                        List<p_s_message> s_messgaers = new List<p_s_message>();
                        List<SQLMessage> sql_messagers = SQLMessage.Select();
                        foreach (SQLMessage i_message in sql_messagers)
                        {
                            p_s_message obj_p_s_message = new p_s_message();
                            var user = SQLUser.FindById(i_message.id_user);
                            if (DateTime.Now > DateTime.Parse(i_message.time).AddMinutes(10)) {
                                obj_p_s_message.phone = user.phone;
                                obj_p_s_message.message = i_message.message;
                                obj_p_s_message.fullname = user.fullname;
                                obj_p_s_message.date = i_message.date;
                                obj_p_s_message.time = i_message.time;
                                s_messgaers.Add(obj_p_s_message);
                            }
                        }
                        responseText = JsonConvert.SerializeObject(s_messgaers);
                    }
                    else
                        responseText = _body.GetDefaultHtmlBody();
                }

                if (response != null) {
                    byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                    response.ContentLength64 = buffer.Length;
                    using (var output = response.OutputStream)
                    {
                        await output.WriteAsync(buffer, 0, buffer.Length);
                    }

                    Console.WriteLine("success send html file");
                }
            }
            
            _db_connection.CloseConnection();
        }
        public void Close() {
            _server.Close();
        }
    }
}
