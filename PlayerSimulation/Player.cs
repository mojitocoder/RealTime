using Microsoft.AspNet.SignalR.Client;
using SharedServerClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayerSimulation
{
    public class Player
    {
        private List<string> sentences;
        private Random random = new Random();
        private HubConnection hubConnection;
        private IHubProxy hubProxy;
        private IDisposable playWithHandler;
        private IDisposable playWithObservable;

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

            hubConnection.Start().Wait();

            //Call the server here
            //Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            //{
            //    var sentence = sentences[random.Next(sentences.Count)];
            //    //Console.WriteLine($"{username} : {sentence}");
            //    hubProxy.Invoke("SendToAll", sentence);
            //});

            var x = hubProxy.On("NotifyFriends", (string friendUserName, bool online) =>
            {
                Console.WriteLine($"User {friendUserName} has just come " + (online ? "online" : "offline"));
            });

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

        public void PlayWith(string username, TextBox log)
        {
            //Remove the previous handler
            if (playWithHandler != null)
                playWithHandler.Dispose();

            //Attach event handler for calls from server
            playWithHandler = hubProxy.On("AddDirectMessage", (string name, string message) =>
            {
                var text = string.Format($"{name}: {message} {Environment.NewLine}{log.Text}");

                //Console.WriteLine(text);

                log.Invoke((MethodInvoker)delegate
                 {
                     log.Text = text; // runs on UI thread
                 });
            });

            if (playWithObservable != null)
                playWithObservable.Dispose();

            //Call the server here
            playWithObservable = Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            {
                var sentence = sentences[random.Next(sentences.Count)];
                //Console.WriteLine($"{username} : {sentence}");
                hubProxy.Invoke("SendToFriend", username, sentence);
            });
        }

        public void Play()
        {
            //Call the server here
            Observable.Interval(TimeSpan.FromMilliseconds(random.Next(300, 1000))).Subscribe((tick) =>
            {
                var sentence = sentences[random.Next(sentences.Count)];
                //Console.WriteLine($"{username} : {sentence}");
                hubProxy.Invoke("SendToAll", sentence);
            });
        }

        public event EventHandler FriendOnOffline;

        protected virtual void OnFriendOnOffline(EventArgs e)
        {
            EventHandler handler = FriendOnOffline;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class FriendOnOfflineEventArgs : EventArgs
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool Online { get; set; }
    }
}
