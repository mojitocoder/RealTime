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
            var userRepo = new SelfHostServer.UserRepository();
            var users = userRepo.GetAll();
            Assert.NotEmpty(users);
        }
    }
}
