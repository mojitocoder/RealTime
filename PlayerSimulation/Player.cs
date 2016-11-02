using Microsoft.AspNet.SignalR.Client;
using SharedServerClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerSimulation
{
    public class Player
    {
        private List<string> sentences;
        private Random random = new Random();
        private HubConnection hubConnection;
        private IHubProxy hubProxy;

        public Player()
        {
            this.sentences = File.ReadLines(@"Data\sentences.txt").ToList();
        }

        public void Connect(string username)
        {
            var url = @"http://localhost:8080/signalr";
            var hubName = @"gameHub";

            hubConnection = new HubConnection(url, new Dictionary<string, string>
            {
                { "token", username }
            });

            hubProxy = hubConnection.CreateHubProxy(hubName);

            //Attach event handler for calls from server
            //hubProxy.On("AddMessage", (string name, string message) =>
            //{
            //    Console.WriteLine($"{name}  : {message}");
            //});

            hubConnection.Start().Wait();

            //Call the server here
            //Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            //{
            //    var sentence = sentences[random.Next(sentences.Count)];
            //    //Console.WriteLine($"{username} : {sentence}");
            //    hubProxy.Invoke("SendToAll", sentence);
            //});
        }

        public async Task<IEnumerable<OnlineUser>> GetFriends()
        {
            var friends = await hubProxy.Invoke<IEnumerable<OnlineUser>>("GetFriends");
            return friends;
        }

        public void Stop()
        {
            hubConnection.Stop();
        }

        public void Play()
        {
            //Call the server here
            Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            {
                var sentence = sentences[random.Next(sentences.Count)];
                //Console.WriteLine($"{username} : {sentence}");
                //hubProxy.Invoke("SendToAll", sentence);
            });
        }
    }
}
