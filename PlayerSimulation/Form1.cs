using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayerSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //load the list of users into the combobox
            var allUsers = File.ReadAllLines(@"Data\users.txt")
                            .Select(foo => foo.Trim())
                            .OrderBy(foo => foo)
                            .Select(foo => new
                            {
                                UserName = foo.ToLower().Replace(' ', '.'),
                                ShownName = string.Format($"{foo} ({foo.ToLower().Replace(' ', '.')})")
                            }).ToList();

            cboUser.DataSource = allUsers;
            cboUser.DisplayMember = "ShownName";
            cboUser.ValueMember = "UserName";
        }

        private Player player = new Player();

        private async void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic selectedItem = cboUser.Items[cboUser.SelectedIndex];
            var username = (string)selectedItem.UserName;
            
            //Go online as this user
            player.Connect(username);

            //Now go and fetch the list of online users
            var friends = await player.GetFriends();

            //Print the list of all friends to the log
            var lines = friends.Select(foo => string.Format($"{foo.UserName} : {foo.FullName} : {foo.Online }"));
            var text = string.Format($"Friends of {username}:{Environment.NewLine}") + string.Join(Environment.NewLine, lines);
            txtLog.Text = string.Format($"{text}{Environment.NewLine}{Environment.NewLine}{txtLog.Text}");

            //Bind list of online friends to the Friend combo
            var onlineFriends = friends.Where(foo => foo.Online)
                                    .Select(foo => new
                                    {
                                        UserName = foo.UserName,
                                        ShownName = string.Format($"{foo.FullName} ({foo.UserName})")
                                    })
                                    .ToList();

            cboFriend.DataSource = onlineFriends;
            cboFriend.DisplayMember = "ShownName";
            cboFriend.ValueMember = "UserName";
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start broadcasting events directly to the selected friend
            dynamic selectedUser = cboUser.Items[cboUser.SelectedIndex];
            var username = (string)selectedUser.UserName;

            dynamic selectedFriend = cboFriend.Items[cboFriend.SelectedIndex];
            var friend = (string)selectedFriend.UserName;

            //TODO: Validation here to make sure both have been selected properly

            player.PlayWith(friend, txtLog);
        }
    }
}
