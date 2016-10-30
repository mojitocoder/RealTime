using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer
{
    /// <summary>
    /// In charge of security, mapping from token to user
    /// </summary>
    public class TokenRepository
    {
        /// <summary>
        /// Return null for invalid token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public User ValidateToken(string token)
        {


            return null;
        }
    }
}
