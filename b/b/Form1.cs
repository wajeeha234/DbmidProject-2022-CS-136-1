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

namespace b
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void update_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName=@FirstName, LastName=@LastName, Contact=@Contact, Email=@Email, RegistrationNumber=@NewRegistrationNumber, Status=@Status WHERE RegistrationNumber=@RegistrationNumber", con);
            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
            cmd.Parameters.AddWithValue("@Email", textBox4.Text);
            cmd.Parameters.AddWithValue("@NewRegistrationNumber", textBox5.Text);
            cmd.Parameters.AddWithValue("@Status", textBox6.Text);
            cmd.Parameters.AddWithValue("@RegistrationNumber", textBox7.Text); // Assuming textBox7 contains the registration number of the student to be updated
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

