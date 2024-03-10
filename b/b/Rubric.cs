using System;
using iTextSharp.text.pdf;
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
    public partial class Rubric : Form
    {
        public Rubric()
        {
            InitializeComponent();
        }
        private void view()
        {
            var con = Configuration.getInstance().getConnection();
            //con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Rubric", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rubricDetails = DetailTextBox1.Text;
            int cloId;
            int maxId = GetMaxRubricId();
            if (!int.TryParse(comboBox1.Text, out cloId))
            {
                MessageBox.Show("Please enter a valid CLO ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rubricDetails != String.Empty)
            {
                addRubric(maxId, rubricDetails, int.Parse(comboBox1.Text));
            }
            else { MessageBox.Show("Fill the data First"); }


        }
        private void addRubric(int maxid, string details, int cloid)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Rubric (Id,Details,CloId) VALUES (@Id,@Details, @CloId)", con);
                cmd.Parameters.AddWithValue("@Id", maxid);
                cmd.Parameters.AddWithValue("@Details", details);
                cmd.Parameters.AddWithValue("@CloId", cloid);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Rubric added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DetailTextBox1.Text = String.Empty;
                //  con.Close();
                view();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GetMaxRubricId()
        {
            int maxId = 0;
            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT MAX(Id) FROM Rubric", con);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                maxId = Convert.ToInt32(result);
            }
            // Increment the maximum ID to get a new unique ID
            int newId = maxId + 1;
            return newId;
        }
        private void load1()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Name FROM Clo", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();

        }

        private void load()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  id FROM Clo", con);
            con.Open();
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox1.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd.ExecuteNonQuery();
        }

        private void Rubric_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Update";
            Update.Text = "Update";
            Update.UseColumnTextForButtonValue = true;
            view();
            load1();
            load();
        }
        private void UpdateRubric(int rubricId, string details, int cloId)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                // con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details = @Details , CloId = @CloId  WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", rubricId);
                cmd.Parameters.AddWithValue("@Details", details);
                cmd.Parameters.AddWithValue("@CloId", cloId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("CLO updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //con.Close();
                textBox1.Text = String.Empty;
                DetailTextBox1.Text = String.Empty;
                view();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBox1.Text, out id))
            {
                MessageBox.Show("Please enter a valid Rubric ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string rubricDetails = DetailTextBox1.Text;
            int cloId;
            if (!int.TryParse(comboBox1.Text, out cloId))
            {
                MessageBox.Show("Please enter a valid CLO ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (rubricDetails != String.Empty)
            {
                // Call the method to update the Rubric
                UpdateRubric(id, rubricDetails, cloId);
            }
            else
            {
                MessageBox.Show("Please fill in the Rubric details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void DeleteRubric(int rubricId)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM Rubric WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", rubricId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Rubric deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox2.Text = string.Empty;
                    view(); // Refresh the view after deletion
                }
                else
                {
                    MessageBox.Show("No Rubric found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBox2.Text, out id))
            {
                MessageBox.Show("Please enter a valid Rubric ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DeleteRubric(id);
        }
        private void SearchRubric(int id)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM Rubric WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string rubricDetails = $"Rubric ID: {reader["Id"]}\nDetails: {reader["Details"]}\nCLO ID: {reader["CloId"]}";
                    MessageBox.Show(rubricDetails, "Rubric Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Clear any previous search text
                    textBox3.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("No Rubric found with the provided ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter an ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SearchRubric(int.Parse(textBox3.Text));

        }

        private void button5_Click(object sender, EventArgs e)
        {
            RubricC n = new RubricC();
            n.Show();
            this.Hide();

        }
    }
}