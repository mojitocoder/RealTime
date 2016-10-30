using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer
{
    /// <summary>
    /// Represent a live session of an user
    /// The user may play multiple games per sessions
    /// </summary>
    public class Session
    {
        public Guid UserId { get; set; }

        public string ConnectionId { get; set; }

        public DateTime StartTime { get; set; }
    }
}
