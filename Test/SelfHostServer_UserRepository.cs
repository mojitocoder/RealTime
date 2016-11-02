using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SelfHostServer;

namespace Test
{

    public class SelfHostServer_UserRepository
    {
        [Fact]
        public void GetAllUsers_Successful()
        {

            var userRepo = new UserRepository();
            var users = userRepo.GetAllUsers();

            Assert.NotEmpty(users);
            foreach (var user in users)
            {
                Assert.True(user.FriendIds.Count >= UserRepository.MinFriends);
            }
        }

        [Fact]
        public void GetAllUsers_Is_Deterministic_Per_Instance()
        {
            var userRepo = new UserRepository();

            var usersAttempt1 = userRepo.GetAllUsers();
            var usersAttempt2 = userRepo.GetAllUsers();
            var usersAttempt3 = userRepo.GetAllUsers();

            Assert.Equal<User>(usersAttempt1, usersAttempt2);
            Assert.Equal<User>(usersAttempt1, usersAttempt3);
        }

        [Fact]
        public void GetAllUsers_Is_Deterministic_Per_Any_Instance()
        {
            var usersAttempt1 = new UserRepository().GetAllUsers();
            var usersAttempt2 = new UserRepository().GetAllUsers();
            var usersAttempt3 = new UserRepository().GetAllUsers();

            Assert.Equal<User>(usersAttempt1, usersAttempt2);
            Assert.Equal<User>(usersAttempt1, usersAttempt3);
        }

        [Fact]
        public void GetAllUsers_Friendship_Is_Mutual()
        {
            var userRepo = new UserRepository();
            var users = userRepo.GetAllUsers();

            foreach (var user in users)
            {
                foreach (var friendId in user.FriendIds)
                {
                    var friend = users.First(foo => foo.Id == friendId);
                    Assert.Contains(user.Id, friend.FriendIds);
                }
            }
        }
    }
}
