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
    public partial class SearchStu : Form
    {
        public SearchStu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text;
            SearchStudent(searchTerm);
        }
        private void SearchStudent(string searchTerm)
        {
            dataGridView1.DataSource = null;

            // Trim and validate the search term
            searchTerm = searchTerm.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            SqlConnection con = null; // Declare SqlConnection outside try block
            DataTable dt = new DataTable(); // Declare DataTable outside try block

            try
            {
                con = Configuration.getInstance().getConnection();

                // Prepare the SQL query to search for a student by ID or RegistrationNumber
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE Id = @SearchTerm ", con);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                // Open database connection and execute the query
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection in the finally block to ensure it's always closed
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            // Bind the DataTable to the DataGridView to display the search results
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                MessageBox.Show("Search successful. Found " + dt.Rows.Count + " student(s).");
            }
            else
            {
                MessageBox.Show("No student found with the given ID or Registration Number");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Student s = new Student();
            s.Show();
            this.Hide();
        }
    }
}
