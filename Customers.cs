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

namespace HotelMS
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dipesh\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            con.Open();
            string Query = "select * from CustomerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustDGV.DataSource = ds.Tables[0];
            con.Close();
        }
        private void EditCustomers()
        {
            if (CNameTb.Text == "" ||GenderCb.SelectedIndex == -1 || CPhoneTb.Text == "" )
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName=@CN,CustPhone=@CP,CustGender=@CG where CustNum=@CKey", con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CG", GenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void InsertCustomers()
        {
            if (CNameTb.Text == "" || GenderCb.SelectedIndex == -1 || CPhoneTb.Text == "" )
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTbl(CustName,CustGender,CustPhone) values(@CN,@CG,@CP)", con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CG", GenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
        private void DeleteCustomers()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Customer!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from CustomerTbl where CustNum=@CKey", con);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted!!!");
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
        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            InsertCustomers();
        }

        private void CustDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CNameTb.Text = CustDGV.SelectedRows[0].Cells[1].Value.ToString();
            GenderCb.Text = CustDGV.SelectedRows[0].Cells[2].Value.ToString();
            CPhoneTb.Text = CustDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (CNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(CustDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditCustomers();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteCustomers();
        }
    }
}
