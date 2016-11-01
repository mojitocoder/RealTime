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
                            .Select(foo => new
                            {
                                UserName = foo.ToLower().Replace(' ', '.'),
                                ShownName = string.Format($"{foo} ({foo.ToLower().Replace(' ', '.')})")
                            }).ToList();

            cboUser.DataSource = allUsers;
            cboUser.DisplayMember = "ShownName";
            cboUser.ValueMember = "UserName";
        }

        private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic selectedItem = cboUser.Items[cboUser.SelectedIndex];
            var username = (string)selectedItem.UserName;

            //Go online as this user


            //Now go and fetch the list of online users
        }
    }




}
