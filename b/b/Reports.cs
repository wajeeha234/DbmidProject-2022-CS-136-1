using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace b
{
    public partial class Reports : Form
    {
        string name;
        string line;
        public Reports()
        {
            InitializeComponent();
        }

        private void load3()
        {
            var con = Configuration.getInstance().getConnection();
            string query = @"SELECT SA.AttendanceId, SA.StudentId, SA.AttendanceStatus, CA.AttendanceDate
                         FROM StudentAttendance SA
                         INNER JOIN ClassAttendance CA ON SA.AttendanceId = CA.Id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
         //   dataGridView1.DataSource = dt;
           // dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
            con.Close();
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

           // dataGridView1.DataSource = dt;
            con.Close();
        }
        private void ShowAttendanceData()
        { MessageBox.Show("nmvakvb");
            try
            {
                var con = Configuration.getInstance().getConnection();

                // Retrieve data from both tables
                string query = @"SELECT ca.Id AS AttendanceId, ca.AttendanceDate, sa.StudentId, sa.AttendanceStatus
                         FROM ClassAttendance ca
                         INNER JOIN StudentAttendance sa ON ca.Id = sa.AttendanceId";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display the data in DataGridView
              //  dataGridView1.DataSource = dt;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching attendance data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("nmvakvb");
            if (comboBox1.Text == "Assessment wise Result of Student")
            {
                view();
            }
            else if (comboBox1.Text == "Clo Wise Result of Students(who attempts Assessments)")
            {
              //  load2();
            }

        
            
            else if (comboBox1.Text == "Attendance Report )")
            {
                MessageBox.Show("nmvakvb");
                ShowAttendanceData();
            }
            else if (comboBox1.Text == "Clo Wise Marks in Assessment Components of Students")
            {
                view();
            }
          
            else if (comboBox1.Text == "Total Result of a Specific Student in Assesment")
            {
                MessageBox.Show("TO check this report sample on Generate Report");
            }
        }
        //private void ExportToPDF(DataGridView dgv, string name, string l)
        //{
        //    try
        //    {
        //        Document document = new Document(PageSize.A4, 20, 20, 20, 20);
        //        PdfWriter.GetInstance(document, new FileStream(name + ".pdf", FileMode.Create));
        //        document.Open();
        //        iTextSharp.text.Font headingFont = FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD);
        //        Paragraph heading = new Paragraph(name, headingFont);
        //        heading.Alignment = Element.ALIGN_CENTER;
        //        heading.SpacingBefore = 10f;
        //        heading.SpacingAfter = 10f;

        //        document.Add(heading);

        //        LineSeparator line = new LineSeparator();
        //        document.Add(line);


        //        iTextSharp.text.Font courseFont = FontFactory.GetFont("Times New Roman", 12);
        //        Paragraph course = new Paragraph(l, courseFont);

        //        course.Alignment = Element.ALIGN_CENTER;
        //        course.IndentationLeft = 55f;
        //        course.SpacingAfter = 20f;
        //        document.Add(course);

        //        LineSeparator line2 = new LineSeparator();
        //        document.Add(line2);



        //        PdfPTable table = new PdfPTable(dgv.Columns.Count);
        //        table.WidthPercentage = 100;
        //        foreach (DataGridViewColumn column in dgv.Columns)
        //        {
        //            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
        //            table.AddCell(cell);
        //        }

        //        foreach (DataGridViewRow row in dgv.Rows)
        //        {
        //           // if (row.Index == dataGridView1.Rows.Count)
        //            {
        //                continue;

        //            }
        //            else
        //            {
        //                try
        //                {
        //                    foreach (DataGridViewCell cell in row.Cells)
        //                    {

        //                        //if (cell.Value == null)
        //                        //{
        //                        //    continue;
        //                        //    MessageBox.Show("Fill all the columns of table (status) it can not be null");
        //                        //}
        //                        //else
        //                        //{
        //                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
        //                        table.AddCell(pdfCell);
        //                        //}
        //                    }
        //                }
        //                catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

        //            }


        //        }
        //        document.Add(table);
        //        document.Close();
        //    }
        //    catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }
        //    // Close the document
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Assessment wise Result of Student")
            {
                AssessmentResReport newUserControl = new AssessmentResReport();
                name = comboBox1.Text;
                line = "Report of Students per Assessment Wise shows the marks of each Assessment Components";
                newUserControl.Show();
                newUserControl.Dock = DockStyle.Fill;
               // this.Parent.Controls.Add(newUserControl);
                newUserControl.BringToFront();
                this.Hide();

            }
            else if (comboBox1.Text == "Clo Wise Result of Students")
            {
                clowise newUserControl = new clowise();
                name = "CLO Wise Result of Students";
                line = "Report of Clo Wise result of Students Who attempts Assessment";
                newUserControl.Show();
                // ExportToPDF(dataGridView1, name, line);
                MessageBox.Show("Report Generated");
            }
        
            else if (comboBox1.Text == " Assessment Components of Students")
            {   
                name = "CLO Wise Result of Assessment Componenets ";
                line = "Report of Clo Wise result of Students According to the Assessment Components they have attempted yet";
              //  ExportToPDF(dataGridView1, name, line);
                MessageBox.Show("Report Generated");
            }
            else if (comboBox1.Text == "Total Result of a Specific Student in Assesment")
            {
               
                MessageBox.Show("Select the Assessment and click on result to generate Report");
               // newUserControl.Dock = DockStyle.Fill;
                //this.Parent.Controls.Add(newUserControl);
               //ewUserControl.BringToFront();
               // this.Hide();

            }
            else if (comboBox1.Text == "Attendance Report")
            {
                ViewAttendance newUserControl = new ViewAttendance();
                 MessageBox.Show("Select the date and click on result to generate Report");
                newUserControl.Show();
                newUserControl.Dock = DockStyle.Fill;
                
                newUserControl.BringToFront();
                this.Hide();

            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            mainForm m =  new mainForm();
            m.Show();
            this.Hide();
        }
    }
}
