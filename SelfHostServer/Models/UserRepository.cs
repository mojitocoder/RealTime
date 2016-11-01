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
        public static readonly int MinFriends = 5;
        private static readonly IList<User> users;

        static UserRepository()
        {
            //Function to convert fullname => username
            Func<string, string> funcConvert = (name) =>
            {
                var parts = name.ToLower().Split(' ');
                return $"{parts[0]}.{parts[1]}";
            };

            //Load the list of full names from text file
            users = File.ReadLines(@"Data\users.txt")
                            .Select(name => name.Trim())
                            .Select(name => new User
                            {
                                Id = Guid.NewGuid(),
                                FullName = name,
                                UserName = funcConvert(name),
                                FriendIds = new HashSet<Guid>()
                            })
                            .ToList();

            var random = new Random();

            //Assign random friends
            foreach (var user in users)
            {
                while (user.FriendIds.Count < MinFriends)
                {
                    var index = random.Next(users.Count); //a random number in the possible range
                    var id = users[index].Id;

                    //must not be itself or an existing friends
                    if (user.Id != id && !user.FriendIds.Contains(id))
                    {
                        user.FriendIds.Add(id);
                    }
                }
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            //var users = new Userr
            return users;
        }
    }
}
