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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace b
{
    public partial class AssessmentResReport : Form
    {
        public AssessmentResReport()
        {
            InitializeComponent();
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
        private void GenerateReport(string fileName)
        {
            try
            {
                // Create a new PDF document
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                document.Open();

                // Add title to the document
                Paragraph title = new Paragraph("Assessment Results Report");
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Add a table to display assessment results
                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                table.WidthPercentage = 100;

                // Add column headers
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    table.AddCell(cell);
                }

                // Add data rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        table.AddCell(new Phrase(cell.Value?.ToString() ?? ""));
                    }
                }

                // Add the table to the document
                document.Add(table);

                // Close the document
                document.Close();

                MessageBox.Show("Report generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating the report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "AssessmentResultsReport.pdf";
            GenerateReport(fileName);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void AssessmentResReport_Load(object sender, EventArgs e)
        {
            view();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reports m = new Reports();
            m.Show();
            this.Hide();

        }
    }
}
