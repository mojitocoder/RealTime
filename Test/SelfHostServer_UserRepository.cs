using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{

    public class SelfHostServer_UserRepository
    {
        [Fact]
        public void GetAll_Successful()
        {
            int minFriends = 5;
            var userRepo = new SelfHostServer.UserRepository(minFriends);
            var users = userRepo.GetAll();

            Assert.NotEmpty(users);
            foreach (var user in users)
            {
                Assert.True(user.FriendIds.Count >= minFriends);
            }
        }
    }
}
