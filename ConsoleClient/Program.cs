using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main_(string[] args)
        {
            Console.WriteLine("Hello world from Console Client!");

            var url = @"http://localhost:27427";
            var hubConnection = new HubConnection(url);

            var hubName = @"stockTicker"; //"StockTickerHub"
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy(hubName);
            stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));



            //QueryString can be used to pass information
            // between client vs. server: https://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-net-client

            //Specify header into connection
            hubConnection.Headers.Add("key", "value");

            //Client certificate can be added like this
            //hubConnection.AddClientCertificate()

            hubConnection.Start().Wait();

            //Types of transports - can specify in the Connection.Start method:
            // + LongPollingTransport
            // + ServerSentEventsTransport
            // + WebSocketTransport
            // + AutoTransport
            // + ForeverFrame - can only be used by the browser


            //Handle connection life time events:
            // + Received
            // + ConnectionSlow
            // + Reconnection
            // + Reconnected
            // + StateChanged
            // + Closed 
            hubConnection.ConnectionSlow += () => Console.WriteLine("Connection problems.");

            //Handle an error raised by SignalR server
            hubConnection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);


            // ==============================
            // Enable client side logging
            // ==============================
            hubConnection.TraceLevel = TraceLevels.All;
            hubConnection.TraceWriter = Console.Out;


            // ==============================
            // Call some methods on the server
            // ==============================

            //Handle error from server side method invocation
            try
            {
                IEnumerable<Stock> allStocks = stockTickerHubProxy.Invoke<IEnumerable<Stock>>("GetAllStocks").Result;
                foreach (Stock stock in allStocks)
                {
                    Console.WriteLine("Symbol: {0} price: {1}", stock.Symbol, stock.Price);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error invoking GetAllStocks: {0}", ex.Message);
            }

            //Call a method on the server - with server returning nothing
            stockTickerHubProxy.Invoke("JoinGroup", "SomeRandomGroup");

            //Call a method on the server - with server return some value
            var stocks = stockTickerHubProxy.Invoke<IEnumerable<Stock>>("AddStock", new Stock() { Symbol = "XYZ" }).Result;


            // ==============================
            // Some local methods that the server can call
            // ==============================

            // Method without params: Notify
            stockTickerHubProxy.On("Notify", () => Console.WriteLine("Notified!"));

            // With some params - string typing: UpdateStockPrice
            stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock =>
                                Console.WriteLine("Symbol {0} Price {1}", stock.Symbol, stock.Price)
                            );

            // With some params - dynamic typing: UpdateStockPrice
            stockTickerHubProxy.On("UpdateStockPrice", stock =>
                                Console.WriteLine("Symbol {0} Price {1}", stock.Symbol, stock.Price)
                            );

            Console.ReadLine();
        }


        static void Main(string[] args)
        {
            var url = @"http://localhost:8080/signalr";
            var hubName = @"myHub";

            var hubConnection = new HubConnection(url, new Dictionary<string, string>
            {
                { "token", "NetClient"}
            });

            var hubProxy = hubConnection.CreateHubProxy(hubName);

            hubProxy.On("addMessage", (string name, string message) =>
            {
                Console.WriteLine($"{name}: {message}");
            });

            hubConnection.Start().Wait();
            Console.ReadLine();
        }
    }
}
