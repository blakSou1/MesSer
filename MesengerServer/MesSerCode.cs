using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading;
using System.Net.WebSockets;
using System.Net;
using System;

namespace MesengerServer
{
    public class ChatHub : Hub
    {
        private static List<User> Users = new List<User>();
        private static List<Groups> Group = new List<Groups>();

        static async Task Main(string[] args)
        {
            int port = 8080;
            await StartServer(port);
            Console.WriteLine("сервер стартует на порту:" + port);
        }
        private static HttpListener httpListener;
        private static CancellationTokenSource cancellationTokenSource;
        static async Task StartServer(int port)
        {
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add("http://localhost:" + port + "/");
                httpListener.Start();
                cancellationTokenSource = new CancellationTokenSource();

                while (true)
                {
                    var context = await httpListener.GetContextAsync();

                    if (!httpListener.IsListening)
                    {
                        break;
                    }
                    if (context.Request.IsWebSocketRequest)
                    {
                        //принимаем веб сокет запрос
                        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                        //обрабатываем подключение клиента
                        HandleClient(webSocketContext.WebSocket);
                    }
                    else
                    {
                        //возвращаем ошибку если запрос не веб сокет
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }

                }
            }
            catch (Exception ex )
            {
                Console.WriteLine("ошибка при запуске сервера:" + ex.Message);
            }

        }
        private void newGroup(User Us, string nameGroup, string password)
        {
            Groups gr = new Groups(nameGroup, password);
            Group.Add(gr);
            Us.Group.Add(gr);
            Us.winGroup.Add(gr);
        }
        private void newChat(string naim, User pl, Groups gr)
        {

        }
        public string Register(string name, string password)
        {

            if(Users.Any(user => user.UserName == name))
            {
                Clients.Caller.SendAsync("RegistrationFail", "Username already exists.");
                return null;
            }
            else
            {
                string id = GenerateId.GenerateID();
                User newUser = new User(id, name, password);
                Users.Add(newUser);
                Clients.Caller.SendAsync("RegistrationSuccess");
                return id;
            }
        }
        public void CreateChat(string groupName, string groupPassword)
        {
            User currentUser = Users.FirstOrDefault(user => user.ID == Context.ConnectionId);

            if(currentUser != null)
            {
                newGroup(currentUser, groupName, groupPassword);
                Clients.Caller.SendAsync("ChatCreated", groupName);
            }
            else
            {
                Clients.Caller.SendAsync("Unauthorized", "User is not registered.");
            }

        }//создание групп
        public void CreatsChats(string naim, Groups gr)
        {
            User currentUser = Users.FirstOrDefault(user => user.ID == Context.ConnectionId);

        }
        public void joinChat(string grName, string password, string id)
        {
            User currentUser = Users.FirstOrDefault(user => user.ID == Context.ConnectionId);

            if(currentUser != null)
            {
                Groups grup = Group.Find(x => x.groupName.Contains(grName) && x.Password.Contains(password) && x.ID.Contains(id));
                if (grup != null)
                {
                    currentUser.Group.Add(grup);
                }
            }
            else
            {
                Clients.Caller.SendAsync("Unauthorized", "User is not Registered");
            }

        }//подключение к групам
        public List<Groups> GetChatList()
        {
            User currentUser = Users.FirstOrDefault(user => user.ID == Context.ConnectionId);

            if(currentUser != null)
            {
                return currentUser.Group;
            }
            return null;
        }//получение доступных груп
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

    public class MesSerCode
    {
    }
}
