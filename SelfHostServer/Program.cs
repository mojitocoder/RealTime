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
            return token;
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
    //public class StockTickerHub : Hub
    //{
    //    public IEnumerable<Stock> GetAllStocks()
    //    {
    //        return _stockTicker.GetAllStocks();
    //    }
    //}
}
