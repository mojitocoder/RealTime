using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace SelfHostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world from self hosting");

            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var token = request.QueryString["token"];

            return token ?? "unknown";
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

            //app.MapSignalR("", new HubConfiguration { });
        }
    }

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

        //this method is not being called from the clients
        public void Send(string name, string message)
        {
            //call addMessage method on javascript in all clients
            Clients.All.addMessage(name, message);

            var x = this.Context.User;
            var y = this.Context.Request;

            var username = this.Context.QueryString["token"];

            Console.WriteLine();
        }
    }

    //public class StockTickerHub : Hub
    //{
    //    public IEnumerable<Stock> GetAllStocks()
    //    {
    //        return _stockTicker.GetAllStocks();
    //    }
    //}
}
