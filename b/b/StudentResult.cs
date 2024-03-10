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
    public partial class StudentResult : Form
    {
        public StudentResult()
        {
            InitializeComponent();

        }
        private void load2()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select  MeasurementLevel FROM  RubricLevel", con);
            //con.Open()
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox3.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();
            cmd.ExecuteNonQuery();
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select  Id FROM  AssessmentComponent", con);
            con.Open();
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();
            cmd.ExecuteNonQuery();
        }
        private void load3()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select  Id FROM  Student", con);
            //con.Open()
            SqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                comboBox1.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();
            cmd.ExecuteNonQuery();
        }
        private void view()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand(@"SELECT sr.*, 
                                          ac.TotalMarks AS ComponentMarks,
                                          rl.MeasurementLevel AS StudentLevel
                                       FROM StudentResult sr
                                       JOIN AssessmentComponent ac ON sr.AssessmentComponentId = ac.Id
                                       JOIN RubricLevel rl ON sr.RubricMeasurementId = rl.Id", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Add a new column for Obtained Marks
            DataColumn obtainedMarksColumn = new DataColumn("ObtainedMarks", typeof(double));
            dt.Columns.Add(obtainedMarksColumn);

            // Calculate and populate the Obtained Marks column
            foreach (DataRow row in dt.Rows)
            {
                double componentMarks = Convert.ToDouble(row["ComponentMarks"]);
                double studentLevel = Convert.ToDouble(row["StudentLevel"]);
                double obtainedMarks = (studentLevel / 4) * componentMarks; // Assuming rubric levels are on a scale of 1 to 4
                row["ObtainedMarks"] = obtainedMarks;
            }

            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
                {

                    if (comboBox3.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a Rubric Measurement ID.");
                        return;
                    }


                    string selectedMeasurementLevel = comboBox3.SelectedItem.ToString();

                    int rubricMeasurementId = GetRubricMeasurementId(selectedMeasurementLevel);

                    if (rubricMeasurementId == -1)
                    {
                        MessageBox.Show("Rubric Measurement ID does not exist.");
                        return;
                    }

                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate) VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate)", con);
                    cmd.Parameters.AddWithValue("@StudentId", comboBox1.SelectedItem);
                    cmd.Parameters.AddWithValue("@AssessmentComponentId", comboBox2.SelectedItem);
                    cmd.Parameters.AddWithValue("@RubricMeasurementId", rubricMeasurementId);
                    cmd.Parameters.AddWithValue("@EvaluationDate", dateTimePicker1.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Added Successfully");
                       view();
                    }
                    else
                    {
                        MessageBox.Show("No Record Added");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured: " + ex.Message);
                }
        }
        private int GetRubricMeasurementId(string measurementLevel)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM RubricLevel WHERE MeasurementLevel = @MeasurementLevel", con);
                cmd.Parameters.AddWithValue("@MeasurementLevel", measurementLevel);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving Rubric Measurement ID: " + ex.Message);
                return -1;
            }
        }
        private void StudentResult_Load_1(object sender, EventArgs e)
        {

            view();
            load();
            load3();
        }
        private void CalculateObtainedMarks()
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    // Retrieve data from StudentResult, AssessmentComponent, and Assessment tables
                    string query = @"SELECT sr.StudentId, sr.AssessmentComponentId, sr.RubricMeasurementId, ac.TotalMarks
                                     FROM StudentResult sr
                                     INNER JOIN AssessmentComponent ac ON sr.AssessmentComponentId = ac.Id";

                    SqlCommand command = new SqlCommand(query, con);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int studentId = reader.GetInt32(0);
                        int assessmentComponentId = reader.GetInt32(1);
                        int rubricMeasurementId = reader.GetInt32(2);
                        int totalMarks = reader.GetInt32(3);

                        // Calculate obtained marks based on rubric measurement level
                        double obtainedMarks = (rubricMeasurementId / 4.0) * totalMarks;

                        // Update the StudentResult table with the calculated obtained marks
                        UpdateObtainedMarks(studentId, assessmentComponentId, obtainedMarks);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while calculating obtained marks: " + ex.Message);
            }
        }
        private void UpdateObtainedMarks(int studentId, int assessmentComponentId, double obtainedMarks)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    string query = @"UPDATE StudentResult 
                                     SET ObtainedMarks = @ObtainedMarks 
                                     WHERE StudentId = @StudentId 
                                     AND AssessmentComponentId = @AssessmentComponentId";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@ObtainedMarks", obtainedMarks);
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@AssessmentComponentId", assessmentComponentId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Obtained marks updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No records updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating obtained marks: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CalculateObtainedMarks();

            // Refresh DataGridView to display updated data
            view();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainForm m = new mainForm();
            m.Show();
            this.Hide();

        }

        //private void AddStudentResult(int studentId, int assessmentComponentId, DateTime evaluationDate)
        //{
        //        var con = Configuration.getInstance().getConnection();
        //    // Get the RubricId from the selected AssessmentComponentId
        //    int RubricMeasurementLevel = 0;//GetRubricMeasurementLevel(assessmentComponentId);
        //    MessageBox.Show($"RubricMeasurementLevel: {RubricMeasurementLevel}", "MeasurementLevel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    if (RubricMeasurementLevel != 0)
        //        {
        //            SqlCommand cmdInsert = new SqlCommand(@"INSERT INTO StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate)
        //            VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate)", con);
        //            cmdInsert.Parameters.AddWithValue("@StudentId", studentId);
        //            cmdInsert.Parameters.AddWithValue("@AssessmentComponentId", assessmentComponentId);
        //            cmdInsert.Parameters.AddWithValue("@RubricMeasurementId", RubricMeasurementLevel);
        //            cmdInsert.Parameters.AddWithValue("@EvaluationDate", evaluationDate);
        //            int rowsAffected = cmdInsert.ExecuteNonQuery();
        //            if (rowsAffected > 0)
        //            {
        //                MessageBox.Show("Student result added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Failed to add student result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("No RubricMeasurementLevel found for the provided AssessmentComponentId.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        con.Close();
        //    }    
        //private int GetRubricMeasurementLevel(int assessmentComponentId)
        //{
        //    int rubricMeasurementLevel = -1; // Default value indicating failure
        //    var con = Configuration.getInstance().getConnection();
        //        {
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //            // Get the RubricId from the selected AssessmentComponentId
        //            SqlCommand cmdRubricId = new SqlCommand("SELECT RubricId FROM AssessmentComponent WHERE Id = @AssessmentComponentId", con);
        //            cmdRubricId.Parameters.AddWithValue("@AssessmentComponentId", assessmentComponentId);
        //            int rubricId = Convert.ToInt32(cmdRubricId.ExecuteScalar()); // Storing RubricId in a variable
        //        MessageBox.Show($"RubricId: {rubricId}", "Rubric ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        if (rubricId > 0)
        //            {        
        //         MessageBox.Show($"RubricId: {rubricId}", "Rubric ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //         SqlCommand cmdRubricMeasurementLevel = new SqlCommand("SELECT RubricLevel.MeasurementLevel FROM RubricLevel " +
        //            "INNER JOIN AssessmentComponent ON RubricLevel.RubricId = AssessmentComponent.RubricId" +
        //            " WHERE AssessmentComponent.Id = @AssessmentComponentId", con);
        //            cmdRubricMeasurementLevel.Parameters.AddWithValue("@AssessmentComponentId", assessmentComponentId);
        //           object measurementLevelObj = cmdRubricMeasurementLevel.ExecuteScalar();
        //                if (measurementLevelObj != null && measurementLevelObj != DBNull.Value)
        //                {
        //                    rubricMeasurementLevel = Convert.ToInt32(measurementLevelObj);
        //                MessageBox.Show($"MeasurementLevel: {rubricMeasurementLevel}", "MeasurementLevel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            } 
        //        }
        //    }
        // return rubricMeasurementLevel;
        //}

    }
}
