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
    public partial class AssesmentComponenet : Form
    {
        public AssesmentComponenet()
        {
            InitializeComponent();
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Id FROM Rubric", con);
            //con.Open()
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox1.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd.ExecuteNonQuery();
        }
        private void load2()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select  Id FROM  Assessment", con);
            //con.Open()
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();
            cmd.ExecuteNonQuery();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dateTimePicker1.Value;
            string sqlDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            string sqlDateTime2 = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (textBox1.Text != String.Empty)
            {
               AddAssessmentComponent(textBox1.Text,int.Parse(comboBox1.Text),int.Parse(textBox2.Text), selectedDateTime,selectedDateTime2, int.Parse(comboBox2.Text));
            }
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
        }
        private void AddAssessmentComponent(string name, int rubricId, int totalMarks,DateTime dateCreated, DateTime dateupdate, int assessmentId)
        {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO AssessmentComponent  (Name, RubricId, TotalMarks, DateCreated,DateUpdated, AssessmentId) VALUES (@Name, @RubricId, @TotalMarks, @DateCreated,@DateUpdated, @AssessmentId)", con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@RubricId", rubricId);
                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
                cmd.Parameters.AddWithValue("@DateUpdated", dateupdate);
                cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Assessment component added successfully.", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                view();
                con.Close();
        }

        private void Assessment_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
        }

        private void AssesmentComponenet_Load(object sender, EventArgs e)
        {
            Assessment_Load(sender,e);
            load();
            load2();
            view();
        }
        private void view()
        {
            var con = Configuration.getInstance().getConnection();
            //con.Open();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            //dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con.Close();
        }
        private void UpdateAssessment(int Id,string name, int rubricId, int totalMarks, DateTime dateCreated, DateTime dateupdate, int assessmentId)
        {
            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();

            }
            SqlCommand cmd = new SqlCommand("UPDATE AssessmentComponent SET Name =@Name, RubricId=@RubricId, TotalMarks= @TotalMarks,DateCreated =@DateCreated , DateUpdated=@DateUpdated ,AssessmentId = @AssessmentId WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@RubricId", rubricId);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", dateupdate);
            cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);
            MessageBox.Show("AssessmentComponent updated successfully.", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            view();
          //  con.Close();
            // Optionally, you can refresh your view here if needed.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int assCOM;
            if (!int.TryParse(textBox3.Text, out assCOM))
            {
                MessageBox.Show("Please enter a valid Assessment ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string title = textBox1.Text;
            DateTime dateCreated = dateTimePicker1.Value;
            DateTime Dateupdate = dateTimePicker2.Value;// Assuming you have a DateTimePicker for date selection
            int totalMarks;
            if (!int.TryParse(textBox2.Text, out totalMarks))
            {
                MessageBox.Show("Please enter a valid Total Marks.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int assid;
            if (!int.TryParse(comboBox2.Text, out assid))
            {
                MessageBox.Show("Please enter a valid AssesmentID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateAssessment(assCOM ,textBox1.Text, int.Parse(comboBox1.Text), int.Parse(textBox2.Text), dateCreated, Dateupdate, int.Parse(comboBox2.Text));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            view();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBox4.Text, out id))
            {
                MessageBox.Show("Please enter a valid Assesment Component ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DeleteASSCOM(id);

        }
        private void DeleteASSCOM(int Id)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM AssessmentComponent  WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Assessment Component  deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox4.Text = string.Empty;
                    view(); // Refresh the view after deletion
                }
                else
                {
                    MessageBox.Show("No Assessment Component  found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter an ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SearchAssessment(int.Parse(textBox5.Text));
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

                SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string assessmentDetails = $"Name: {reader["Name"]}\nRubricId: {reader["RubricId"]}\nTotalMarks: {reader["TotalMarks"]}\nDateCreated: {reader["DateCreated"]}\nDateUpdated: {reader["DateUpdated"]}\nAssessmentId: {reader["AssessmentId"]}";
                    MessageBox.Show(assessmentDetails, "Assessment Component Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No Assessment Component found with the provided ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
