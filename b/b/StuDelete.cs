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
    public partial class StuDelete : Form
    {
        public StuDelete()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE Status  = 5", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    
        private int GetStatusId(string statusName)
        {
            int statusId = -1; // Default value if status is not found
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT LookupId FROM Lookup WHERE Name = @Name", con);
            cmd.Parameters.AddWithValue("@Name", statusName);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                statusId = (int)result;
            }
            return statusId;
        }
        private void UpdateStudentStatus(string Id, string newStatus)
        {
            int newStatusId = GetStatusId(newStatus);
            if (newStatusId == -1)
            {
                MessageBox.Show($"Status '{newStatus}' not found in the Lookup table.");
                return;
            }
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Student SET Status = @NewStatusId WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@NewStatusId", newStatusId);
            cmd.Parameters.AddWithValue("@Id", textBoxId.Text);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Student status deleted successfully.");
            }
            else
            {
                MessageBox.Show("No student found with the specified ID.");
            }

        }
        public static int GetLookupId(string statusName)
        {
            int lookupId = -1; // Initialize to a default value

            try
            {
                using (SqlConnection con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    // Query to retrieve the ID from the Lookup table based on the provided status name
                    string query = "SELECT LookupId FROM dbo.Lookup WHERE Name = @StatusName";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StatusName", statusName);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            lookupId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return lookupId;
        }
        private void RefreshDataGridView()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE Status  = 5", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxId.Text))
            {
                MessageBox.Show("Please enter an registration Number to modify the status.");
                return;
            }



            UpdateStudentStatus(textBoxId.Text, "Inactive");
        }

        private void StuDelete_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Student s = new Student();
            s.Show();
            this.Hide();
        }
    }
}
