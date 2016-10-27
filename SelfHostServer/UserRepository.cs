using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer
{
    public class UserRepository
    {
        public IEnumerable<User> GetAll()
        {
            //Function to convert fullname => username
            Func<string, string> funcConvert = (name) =>
            {
                var parts = name.ToLower().Split(' ');
                return $"{parts[0]}.{parts[1]}";
            };

            //Load the list of full names from text file
            var names = File.ReadLines(@"Data\users.txt")
                            .Select(name => name.Trim())
                            .Select(name => new User
                            {
                                Id = Guid.NewGuid(),
                                FullName = name,
                                UserName = funcConvert(name)
                            });

            return names;
        }
    }
}
