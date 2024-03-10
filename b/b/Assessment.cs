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
    public partial class Assessment : Form
    {

        public Assessment()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void AddAssesment(string title, string sqlDateTime, int totalMarks, int totalWaitage)
        {

            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Assessment (Title, DateCreated, TotalMarks, TotalWeightage) VALUES (@Title, @DateCreated, @TotalMarks, @TotalWeightage)", con);
            cmd.Parameters.AddWithValue("@Title", textBox1.Text);
            cmd.Parameters.AddWithValue("@DateCreated", sqlDateTime);
            cmd.Parameters.AddWithValue("@TotalMarks", textBox2.Text);
            cmd.Parameters.AddWithValue("@TotalWeightage", textBox3.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);


            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;

        }

        private String check()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($" IF ( select MAX(1) FROM Assessment WHERE Assessment.Title ='{textBox1.Text}') > 0 BEGIN   SELECT '1' END ELSE BEGIN   SELECT '2' END", con);
            string X = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                X = (reader.GetString(0));
            }
            reader.Close();

            // X=cmd.ExecuteReader().GetString(0);
            cmd.ExecuteNonQuery();
            return X;


        }

        private void Assessment_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
            // view();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dateTimePicker1.Value;
            string sqlDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (textBox1.Text != String.Empty)
            {
                AddAssesment(textBox1.Text, sqlDateTime, int.Parse(textBox2.Text), int.Parse(textBox3.Text));
            }
            //view();
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Assessment", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int assessmentId;
            if (!int.TryParse(textBox4.Text, out assessmentId))
            {
                MessageBox.Show("Please enter a valid Assessment ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string title = textBox1.Text;
            DateTime dateCreated = dateTimePicker1.Value; // Assuming you have a DateTimePicker for date selection
            int totalMarks;
            if (!int.TryParse(textBox2.Text, out totalMarks))
            {
                MessageBox.Show("Please enter a valid Total Marks.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int totalWeightage;
            if (!int.TryParse(textBox3.Text, out totalWeightage))
            {
                MessageBox.Show("Please enter a valid Total Weightage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateAssessment(assessmentId, title, dateCreated, totalMarks, totalWeightage);


        }
        private void UpdateAssessment(int assessmentId, string title, DateTime dateCreated, int totalMarks, int totalWeightage)
        {
            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("UPDATE Assessment SET Title=@Title, DateCreated=@DateCreated, TotalMarks=@TotalMarks, TotalWeightage=@TotalWeightage WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", assessmentId);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Assessment updated successfully.", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            // Optionally, you can refresh your view here if needed.
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBox5.Text, out id))
            {
                MessageBox.Show("Please enter a valid Assesment ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DeleteRubric_level(id);

        }
        private void DeleteRubric_level(int rubric_levelId)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM Assessment WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", rubric_levelId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Rubric Level deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox5.Text = string.Empty;
                    // view(); // Refresh the view after deletion
                }
                else
                {
                    MessageBox.Show("No Rubric_level found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           if(string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter an ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SearchAssessment(int.Parse(textBox6.Text));
        }

        private void SearchAssessment(int id)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM Assessment WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string assessmentDetails = $"Assessment ID: {reader["Id"]}\nTitle: {reader["Title"]}\nDate Created: {reader["DateCreated"]}\nTotal Marks: {reader["TotalMarks"]}\nTotal Weightage: {reader["TotalWeightage"]}";
                    MessageBox.Show(assessmentDetails, "Assessment Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Clear any previous search text
                    textBox6.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("No Assessment found with the provided ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Attendance m = new Attendance();
            m.Show();
            this.Hide();

        }
    }
    }
