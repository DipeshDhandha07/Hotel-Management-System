using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HotelMS
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            populate();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dipesh\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            con.Open();
            string Query = "select * from UserTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserDGV.DataSource = ds.Tables[0];
            con.Close();
        }
        private void EditUsers()
        {
            if (UNameTb.Text == "" || GenderCb.SelectedIndex == -1 || UPhoneTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("update UserTbl set UName=@UN,UPhone=@UP,UGender=@UG,UPassword=@UPa where UNum=@UKey", con);
                    cmd.Parameters.AddWithValue("@UN", UNameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@UG", GenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@UPa", PasswordTb.Text);
                    cmd.Parameters.AddWithValue("@UKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void InsertUsers()
        {
            if (UNameTb.Text == "" || GenderCb.SelectedIndex == -1 || UPhoneTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into UserTbl(UName,UPhone,UGender,UPassword) values(@UN,@UP,@UG,@UPa)", con);
                    cmd.Parameters.AddWithValue("@UN", UNameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@UG", GenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@UPa", PasswordTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Added!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
        private void DeleteUsers()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a User!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from UserTbl where UNum=@UKey", con);
                    cmd.Parameters.AddWithValue("@UKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        int Key = 0;
        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }


        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            InsertUsers();
        }

        private void UserDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            UNameTb.Text = UserDGV.SelectedRows[0].Cells[1].Value.ToString();
            UPhoneTb.Text = UserDGV.SelectedRows[0].Cells[2].Value.ToString();
            GenderCb.Text = UserDGV.SelectedRows[0].Cells[3].Value.ToString();
            PasswordTb.Text = UserDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (UNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(UserDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteUsers();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditUsers();
        }
    }
}
