using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class AutomatedClient
    {
        private string username;
        private List<string> sentences;
        private Random random;

        public AutomatedClient(string username, Random random)
        {
            this.username = username;
            this.random = random;
            this.sentences = File.ReadLines(@"Data\sentences.txt").ToList();
        }

        public void Start(bool listening)
        {
            var url = @"http://localhost:8080/signalr";
            var hubName = @"gameHub";

            var hubConnection = new HubConnection(url, new Dictionary<string, string>
            {
                { "token", this.username }
            });

            var hubProxy = hubConnection.CreateHubProxy(hubName);

            //Attach event handler for calls from server
            if (listening)
            {
                hubProxy.On("AddMessage", (string name, string message) =>
                {
                    Console.WriteLine($"{name}  : {message}");
                });
            }

            hubConnection.Start().Wait();

            //Call the server here
            Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            {
                var sentence = sentences[random.Next(sentences.Count)];
                //Console.WriteLine($"{username} : {sentence}");
                hubProxy.Invoke("SendToAll", sentence);
            });
        }
    }
}
