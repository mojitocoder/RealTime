using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer
{
    public class ConnectionRegistry<T>
    {
        private readonly Dictionary<T, HashSet<string>> globalConnections = new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return globalConnections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (globalConnections)
            {
                HashSet<string> userConnections;
                if (!globalConnections.TryGetValue(key, out userConnections))
                {
                    userConnections = new HashSet<string>();
                    globalConnections.Add(key, userConnections);
                }

                lock (userConnections)
                {
                    userConnections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> userConnections;
            if (globalConnections.TryGetValue(key, out userConnections))
            {
                return userConnections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (globalConnections)
            {
                HashSet<string> userConnections;
                if (!globalConnections.TryGetValue(key, out userConnections))
                {
                    //User does not appear in the global connection registry
                    //  unlikely to happen
                    return;
                }

                lock (userConnections)
                {
                    userConnections.Remove(connectionId);

                    if (userConnections.Count == 0) //last connection of the user => remove him as well
                    {
                        globalConnections.Remove(key);
                    }
                }
            }
        }
    }
}
