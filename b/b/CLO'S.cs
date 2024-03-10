using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace b
{
    public partial class CLO_S : Form
    {
        bool check_update = false;
        string name;
        int id;
        public CLO_S()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime selectedDateTime1 = dateTimePicker1.Value;
            string dateC = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            string dataU = selectedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            bool check_date = false;
            string dateFormat = "yyyy-MM-dd";
            bool validDate1 = DateTime.TryParseExact(dateC, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime1);
            bool validDate2 = DateTime.TryParseExact(dataU, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime2);

            if (validDate1 && validDate2)
            {
                if (selectedDateTime2 > selectedDateTime1 || selectedDateTime2 == selectedDateTime1)
                {
                    check_date = true;
                }
            }
            if (check_update == false)
            {
                string title = textBox1.Text; // Assuming textBoxTitle is the TextBox for entering the CLO title
                DateTime dateCreated = dateTimePicker1.Value;
                DateTime dateUpdated = dateTimePicker2.Value;
                AddCLO(title, dateCreated, dateUpdated);
            }
            // Refresh the DataGridView after adding the new CLO
            RefreshDataGridView();
        }
        private void SetupForm()
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
            dateTimePicker2.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker2.MaxDate = new DateTime(2024, 12, 31);
            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
        }
        private void RefreshDataGridView()
        {

            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Clo", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();

        }
        private bool CLOExists(int id)
        {
            bool exists = false;

            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Clo WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                int count = (int)cmd.ExecuteScalar();
                exists = count > 0;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking for the existence of the CLO: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return exists;
        }
        private void CLO_S_Load(object sender, EventArgs e)
        {
            SetupForm();
            RefreshDataGridView();  
        }
        private void AddCLO(string title, DateTime dateCreated, DateTime dateUpdated)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Clo (Name, DateCreated, DateUpdated) VALUES (@Name, @DateCreated, @DateUpdated)", con);
                cmd.Parameters.AddWithValue("@Name", title);
                cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
                cmd.Parameters.AddWithValue("@DateUpdated", dateUpdated);
                cmd.ExecuteNonQuery();
                MessageBox.Show("CLO added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                textBox1.Text = String.Empty;
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateCLO(int id, string title, DateTime dateCreated, DateTime dateUpdated)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Clo SET Name = @Name, DateCreated = @DateCreated, DateUpdated = @DateUpdated WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", title);
                cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
                cmd.Parameters.AddWithValue("@DateUpdated", dateUpdated);
                cmd.ExecuteNonQuery();
                MessageBox.Show("CLO updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
               MessageBox.Show("Please enter an ID to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }
            if (!int.TryParse(textBox2.Text, out id))
            {
                MessageBox.Show("Please enter a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if the CLO with the provided ID exists in the database
            if (!CLOExists(id))
            {
                MessageBox.Show("CLO with the provided ID does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime selectedDateTime1 = dateTimePicker1.Value;
            string dateC = selectedDateTime1.ToString("MM-dd-yyyy HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            string dataU = selectedDateTime2.ToString("MM-dd-yyyy HH:mm:ss");
            bool check_date = false;
                if ((selectedDateTime2 > selectedDateTime1 )|| (selectedDateTime2 == selectedDateTime1))
                {
                    check_date = true;
                }
            if (check_date)
            {
                UpdateCLO(id, textBox1.Text, selectedDateTime1, selectedDateTime2);
            }
            else
            {
                MessageBox.Show("Updated date must be greater than or equal to created date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter an ID to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
            if (!int.TryParse(textBox3.Text, out id))
            {
                MessageBox.Show("Please enter a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Call the DeleteCLO method with the provided ID
            DeleteCLO(id);
        }
        private void DeleteCLO(int id)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Clo WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("CLO deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox3.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("No CLO found with the provided ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter an ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox4.Text, out id))
            {
                MessageBox.Show("Please enter a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Call the SearchCLO method with the provided ID
            SearchCLO(id);
        }
        private void SearchCLO(int id)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Clo WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    string cloDetails = $"CLO ID: {reader["Id"]}\nName: {reader["Name"]}\nDate Created: {reader["DateCreated"]}\nDate Updated: {reader["DateUpdated"]}";
                    MessageBox.Show(cloDetails, "CLO Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox4.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("No CLO found with the provided ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            mainForm m = new mainForm();
            m.Show();
            this.Hide();

        }
    }

}
