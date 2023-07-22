using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelMS
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if(PasswdTb.Text=="")
            {
                MessageBox.Show("Enter Admin Password");
            }
            else
            {
                if(PasswdTb.Text=="Password")
                {
                    Users obj = new Users();
                    this.Hide();
                    obj.Show();

                }
                else
                {
                    MessageBox.Show("Wrong Admin Password");
                }
            }
        }
    }
}
