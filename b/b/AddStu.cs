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
    public partial class AddStu : Form
    {
        public AddStu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection con = null; // Declare con outside try block

            try
            {
              
                con = Configuration.getInstance().getConnection();
                if (IsRegistrationNumberExists(textBox5.Text))
                {
                    MessageBox.Show("A student with the same registration number already exists.");
                    return; // Exit the function if registration number already exists
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status, Active_Flag) VALUES (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status, @Active_Flag)", con);
                cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@RegistrationNumber", textBox5.Text);
                cmd.Parameters.AddWithValue("@Status", textBox6.Text);
                cmd.Parameters.AddWithValue("@Active_Flag", 1); // Set Active_Flag to 1 initially

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student added successfully");
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private bool IsRegistrationNumberExists(string registrationNumber)
        {
            bool exists = false;

            SqlConnection con = null; // Declare con outside try block

            try
            {
                con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE RegistrationNumber = @RegistrationNumber", con);
                cmd.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                exists = (count > 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return exists;
        }
        private void RefreshDataGridView()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void button2_Click(object sender, EventArgs e)
        { SqlConnection con = null; // Declare con outside try block

            try
            {
                con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE Active_Flag = 1", con);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student m = new Student();
            m.Show();

        }
    }
}
