using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer.Hubs
{
    public class MyHub : Hub
    {
        public override async Task OnConnected()
        {
            string token = Context.QueryString["token"];

            var user = Context.User;

            Console.WriteLine();
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Connection Id: {Context.ConnectionId}");

            await base.OnConnected();
        }

        //This method is being called from the clients
        public void Send(string name, string message)
        {
            //call addMessage method on javascript in all clients
            Clients.All.addMessage(name, message);

            var x = this.Context.User;
            var y = this.Context.Request;

            var username = this.Context.QueryString["token"];

            //Console.WriteLine();
        }

        public void SendToUser(string username, string name, string message)
        {
            Clients.User(username).addMessage(name, message);
        }
    }
}
