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
    public partial class Rubric_Level : Form
    {
        bool check_update = false;
     
        int id, RubericID, Measurment;
   

        public Rubric_Level()
        {
            InitializeComponent();
          
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  id FROM Rubric", con);
            //con.Open()
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd.ExecuteNonQuery();
        }
        private void view()
        {
            var con = Configuration.getInstance().getConnection();
            //con.Open();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con.Close();
        }

        private void Rubric_Level_Load(object sender, EventArgs e)
        {
            load_combobox_assessment_data();
           
          //  load();
            view();
           // bool check_update = false;

        }
        private void load_combobox_assessment_data()
        {

           // comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            load();
           

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string rubricDetails = richTextBox1.Text;
            int RubricId;
            if (!int.TryParse(comboBox1.Text, out RubricId))
            {
                MessageBox.Show("Please enter a valid Rubric ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rubricDetails != String.Empty)
            {
                if (check_update == false)
                {
                    addRubric_level(int.Parse(comboBox2.Text), rubricDetails, int.Parse(comboBox1.Text));
                }
           
                else { MessageBox.Show("Fill the data First"); }


            }
        }
        private void addRubric_level(int RubricId, string details, int MeasurementLevel)
        {
            try
            {
               
                var con = Configuration.getInstance().getConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO RubricLevel (RubricId,Details,MeasurementLevel) VALUES (@RubricId,@Details, @MeasurementLevel)", con);
                cmd.Parameters.AddWithValue("@RubricId", RubricId);
                cmd.Parameters.AddWithValue("@Details", details);
                cmd.Parameters.AddWithValue("@MeasurementLevel", MeasurementLevel);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Rubric level added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
               richTextBox1.Text = String.Empty;
                con.Close();
                view();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateRubric_level(int RubricId, string details, int MeasurementLevel)
        { 
            var con = Configuration.getInstance().getConnection();
            con.Open();
            SqlCommand cmd2 = new SqlCommand("Update RubricLevel Set RubricId=@RubricId ,Details=@Details, MeasurementLevel=@MeasurementLevel where Id=@Id", con);
            cmd2.Parameters.AddWithValue("@Id", id);
            cmd2.Parameters.AddWithValue("@RubricId", RubricId);
            cmd2.Parameters.AddWithValue("@Details", details);
            cmd2.Parameters.AddWithValue("@MeasurementLevel", MeasurementLevel);
            cmd2.ExecuteNonQuery();
            MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            view();
            check_update = false;
            richTextBox1.Text = String.Empty;
            comboBox1.Text = String.Empty;

            //  }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            view();
;        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBox2.Text, out id))
            {
                MessageBox.Show("Please enter a valid Rubric_Level ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM RubricLevel WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", rubric_levelId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Rubric Level deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox2.Text = string.Empty;
                    view(); // Refresh the view after deletion
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

        private void button3_Click(object sender, EventArgs e)
        {
            int RubricId;
            if (!int.TryParse(textBox1.Text, out RubricId))
            {
                MessageBox.Show("Please enter a valid Rubric_LEVEL ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string rubricDetails = richTextBox1.Text;
            if (rubricDetails != String.Empty)
            {
              
             UpdateRubric_level(int.Parse(comboBox2.Text), rubricDetails, int.Parse(comboBox1.Text));
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter an ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SearchRubric(int.Parse(textBox3.Text));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RubricC m = new RubricC();
            m.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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

                SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string rubricDetails = $"Rubric ID: {reader["Id"]}\nDetails: {reader["Details"]}\nMeasurementLevel ID: {reader["MeasurementLevel"]}";
                    MessageBox.Show(rubricDetails, "Rubric_level Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


    }
}
