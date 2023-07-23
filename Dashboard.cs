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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            CountRooms();
            CountCustomers();
            SumAmount();
            SumDaily();
            SumCustomers();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dipesh\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");

        private void CountRooms()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from RoomTbl",con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            Roomsnb.Text = dt.Rows[0][0].ToString()+" Rooms";
            con.Close();
        }
        private void CountCustomers()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from CustomerTbl", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            Customernb.Text = dt.Rows[0][0].ToString()+" Customers";
            con.Close();
        }

        private void SumAmount()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(Cost) from BookingTbl", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            Financenb.Text = dt.Rows[0][0].ToString();
            con.Close();
        }
        private void SumDaily()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(Cost) from BookingTbl where BookDate='"+BDate.Value.Date+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            IncomeRs.Text = dt.Rows[0][0].ToString();
            con.Close();
        }

        private void SumCustomers()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(Cost) from BookingTbl where Customer='" + CustomerCb.SelectedValue.ToString() + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            CustomerRs.Text = dt.Rows[0][0].ToString();
            con.Close();
        }

        private void BDate_ValueChanged(object sender, EventArgs e)
        {
            SumDaily();
        }
    }
} 
