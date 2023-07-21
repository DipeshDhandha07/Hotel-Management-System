using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace HotelMS
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            GetCategories();
            populate();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dipesh\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            con.Open();
            string Query = "select * from RoomTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            RoomDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void GetCategories()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from TypeTbl", con); 
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeNum", typeof(int));
            dt.Load(rdr);
            this.RTypeCb.ValueMember = "TypeName";
            this.RTypeCb.DataSource =dt;
            con.Close();
        }
        
        private void EditRooms()
        {
            if (RnameTb.Text == "" || RTypeCb.SelectedIndex == -1 || StatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("update RoomTbl set RName=@RN,RType=@RT,RStatus=@RS where RNum=@RKey", con);
                    cmd.Parameters.AddWithValue("@RN", RnameTb.Text);
                    cmd.Parameters.AddWithValue("@RT", RTypeCb.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@RS", StatusCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@RKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Updated!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void InsertRooms()
        {
            if (RnameTb.Text == "" || RTypeCb.SelectedIndex == -1 || StatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into RoomTbl(RName,RType,RStatus) values(@RN,@RT,@RS)", con);
                    cmd.Parameters.AddWithValue("@RN", RnameTb.Text);
                    cmd.Parameters.AddWithValue("@RT", RTypeCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@RS", StatusCb.SelectedItem.ToString()); ;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Added!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
        private void DeleteRooms()
        {
            if (Key==0)
            {
                MessageBox.Show("Select a Room!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from RoomTbl where RNum=@RKey", con);
                    cmd.Parameters.AddWithValue("@RKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Deleted!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            InsertRooms();
        }
        int Key = 0;
        private void RoomDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RnameTb.Text = RoomDGV.SelectedRows[0].Cells[1].Value.ToString();
            RTypeCb.Text = RoomDGV.SelectedRows[0].Cells[2].Value.ToString();
            StatusCb.Text = RoomDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (RnameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(RoomDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditRooms();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteRooms();
        }

        private void RnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void RTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
