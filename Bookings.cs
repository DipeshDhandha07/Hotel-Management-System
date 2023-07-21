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
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            populate(); 
            GetRooms();
            GetCustomers();
            FetchCost();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dipesh\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            con.Open();
            string Query = "select * from BookingTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookingDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void GetRooms()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from RoomTbl where RStatus='Available'", con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("RName", typeof(int));
            dt.Load(rdr);
            RoomCb.ValueMember = "RName";
            RoomCb.DataSource = dt;
            con.Close();
        }
        private void GetCustomers()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from CustomerTbl",con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustName", typeof(string));
            dt.Load(rdr);
            CustomerCb.ValueMember = "CustName";
            CustomerCb.DataSource = dt;
            con.Close();
        }

        int Price = 1;
        private void FetchCost()
        {
            con.Open();
            string Query = "select TypeCost from RoomTbl join TypeTbl on RNum=" + RoomCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query,con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                Price = Convert.ToInt32(dr["TypeCost"].ToString());
            }
            con.Close();
        }

        private void Booking()
        {
            if (CustomerCb.SelectedIndex == -1 || RoomCb.SelectedIndex == -1 ||AmountTb.Text == "" || DurationTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BookingTbl(Room,Customer,BookDate,Duration,Cost) values(@R,@C,@BD,@Dura,@Cost)", con);
                    cmd.Parameters.AddWithValue("@R", RoomCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@C", CustomerCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@BD", BDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Dura", DurationTb.Text);
                    cmd.Parameters.AddWithValue("@Cost", AmountTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Booked!!!");
                    con.Close();
                    populate();
                    GetRooms();
                    GetCustomers();
                    FetchCost();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        int Key = 0;
        private void CancelBooking()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Booking!!!");
            }
            else
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from BookingTbl where BookingNum=@BKey", con);
                    cmd.Parameters.AddWithValue("@BKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Booking Canceled!!!");
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void BookBtn_Click(object sender, EventArgs e)
        {
            Booking();
        }

        private void CustomerCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void RoomCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FetchCost();
        }

        private void DurationTb_TextChanged(object sender, EventArgs e)
        {
            if(AmountTb.Text == "")
            {
                AmountTb.Text = "Rs 0";
            }
            else
            {
                try
                {
                    int Total = Price * Convert.ToInt32(DurationTb.Text);
                    AmountTb.Text = "Rs " + Total;
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);                    
                }
                
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CancelBooking();
        }

        private void BookingDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RoomCb.Text = BookingDGV.SelectedRows[0].Cells[1].Value.ToString();
            CustomerCb.Text = BookingDGV.SelectedRows[0].Cells[2].Value.ToString();
            BDate.Text = BookingDGV.SelectedRows[0].Cells[3].Value.ToString();
            DurationTb.Text = BookingDGV.SelectedRows[0].Cells[4].Value.ToString();
            AmountTb.Text = BookingDGV.SelectedRows[0].Cells[5].Value.ToString();
            if (RoomCb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(BookingDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void RoomCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
