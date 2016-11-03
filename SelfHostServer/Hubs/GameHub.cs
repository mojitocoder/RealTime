using Microsoft.AspNet.SignalR;
using SharedServerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer
{
    public class GameHub : Hub
    {
        private readonly static ConnectionRegistry<string> connectionRegistry = new ConnectionRegistry<string>();
        //private readonly sta

        private string GetUserName()
        {
            return Context.QueryString["token"] ?? "unknown";
        }

        public override async Task OnConnected()
        {
            connectionRegistry.Add(GetUserName(), Context.ConnectionId);

            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            connectionRegistry.Remove(GetUserName(), Context.ConnectionId);

            await base.OnDisconnected(stopCalled);
        }

        public override async Task OnReconnected()
        {
            if (!connectionRegistry.GetConnections(GetUserName()).Contains(Context.ConnectionId))
            {
                connectionRegistry.Add(GetUserName(), Context.ConnectionId);
            }

            await base.OnReconnected();
        }

        /// <summary>
        /// The user call this method to register 
        /// the interest to play a multi-player game.
        /// The server will call back to the client when
        /// a matching partner is found.
        /// </summary>
        /// <param name="mode"></param>
        public void FindPartner(GameMode mode)
        {
            var connectionId = Context.ConnectionId;

            //keep in a queue somewhere, resolve every so often

            //Clients.Client("").
        }


        //This method is being called from the clients
        public void SendToAll(string message)
        {
            //Broadcast the message to all connected clients
            Clients.All.AddMessage(GetUserName(), message);
        }

        public void SendToFriend(string friendUserName,string message)
        {
            Clients.User(friendUserName).AddDirectMessage(GetUserName(), message);
        }


        public IEnumerable<OnlineUser> GetOnlineUsers()
        {
            var users = new UserRepository().GetAllUsers().ToDictionary(foo => foo.UserName, foo => foo.FullName);

            return connectionRegistry.GetKeys()
                        .Where(foo => users.ContainsKey(foo)) //this step, in theory, is not necessary
                        .Select(foo => new OnlineUser
                        {
                            UserName = foo,
                            FullName = users[foo],
                            Online = true
                        });
        }

        public IEnumerable<OnlineUser> GetFriends()
        {
            var allUsers = new UserRepository().GetAllUsers();
            var onlineUserNames = new HashSet<string>(connectionRegistry.GetKeys());

            var self = allUsers.First(foo => foo.UserName == GetUserName());

            var friends = self.FriendIds
                            .Select(foo => allUsers.First(bar => bar.Id == foo))
                            .Select(foo => new OnlineUser
                            {
                                UserName = foo.UserName,
                                FullName = foo.FullName,
                                Online = onlineUserNames.Contains(foo.UserName)
                            })
                            .ToList();

            return friends;
        }
    }

    public enum GameMode
    {
        Crucible = 0, // players against one another
        Vanguard = 1 // players cooperate
    }

    //public class OnlineUser
    //{
    //    public string UserName { get; set; }
    //    public string FullName { get; set; }
    //}
}
