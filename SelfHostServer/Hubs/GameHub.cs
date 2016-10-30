using Microsoft.AspNet.SignalR;
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


        public override async Task OnConnected()
        {
            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled);
        }

        public override async Task OnReconnected()
        {
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

        
    }

    public enum GameMode
    {
        Crucible = 0, // players against one another
        Vanguard = 1 // players cooperate
    }
}
