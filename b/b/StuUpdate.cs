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
    public partial class StuUpdate : Form
    {
        public StuUpdate()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = null; // Declare con outside try block

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
    
        private void RefreshDataGridView()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            string studentIdToUpdate = textBox7.Text.Trim();

            if (!string.IsNullOrEmpty(studentIdToUpdate))
            {
                SqlConnection con = null; // Declare SqlConnection outside try block

                try
                {
                    con = Configuration.getInstance().getConnection();
                    string newName = textBox1.Text;
                    string newLastName = textBox2.Text;
                    string newContact = textBox3.Text;
                    string newEmail = textBox4.Text;
                    string newRegNo = textBox5.Text;
                    string newStatus = textBox6.Text;

                    Console.WriteLine("Student ID to Update: " + studentIdToUpdate);

                    SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email,RegistrationNumber = @RegistrationNumber,  Status = @Status WHERE Id = @StudentId", con);
                    cmd.Parameters.AddWithValue("@StudentId", studentIdToUpdate);
                    cmd.Parameters.AddWithValue("@FirstName", newName);
                    cmd.Parameters.AddWithValue("@LastName", newLastName);
                    cmd.Parameters.AddWithValue("@Contact", newContact);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", newRegNo);
                    cmd.Parameters.AddWithValue("@Status", newStatus);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student with ID " + studentIdToUpdate + " updated successfully");
                        RefreshDataGridView(); // Optional: Refresh the DataGridView after update
                    }
                    else
                    {
                        MessageBox.Show("No student found with the given ID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    // Ensure the connection is closed in all cases
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a Student ID to update");
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
