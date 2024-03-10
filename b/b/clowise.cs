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
    public partial class clowise : Form
    {
        public clowise()
        {
            InitializeComponent();

        }
       private void generatePDF()
        {
            // Create a new PDF document
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Clo.pdf", FileMode.Create));

            // Open the document
            doc.Open();

            // Add main heading
            Paragraph heading = new Paragraph("Manage Clo", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.BOLD));
            heading.Alignment = Element.ALIGN_CENTER;
            doc.Add(heading);

            // Add page border
            PdfContentByte content = wri.DirectContent;
            content.SetLineWidth(2);
            content.Rectangle(20, 20, doc.PageSize.Width - 40, doc.PageSize.Height - 40);
            content.Stroke();

            // Add current date
            Paragraph date = new Paragraph("Date: " + DateTime.Now.ToString("yyyy-MM-dd"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12));
            date.Alignment = Element.ALIGN_MIDDLE;
            doc.Add(date);

            // Create a table with 3 columns
            PdfPTable table = new PdfPTable(4);

            // Add headers to the table
            table.AddCell("CLO ID");
            table.AddCell("CLO Name");
            table.AddCell("Obtained Marks");
            table.AddCell("Max Marks");

            // Query the database for CLO data and perform calculation
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Clo", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Calculate obtained marks for each CLO
                float obtainedMarks = GetObtainedMarksForClo(reader["Id"].ToString());

                // Get maximum marks for the CLO
                float maxMarks = GetMaxMarksForClo(reader["Id"].ToString());

                // Add data to the table
                table.AddCell(reader["Id"].ToString());
                table.AddCell(reader["Name"].ToString());
                table.AddCell(obtainedMarks.ToString());
                table.AddCell(maxMarks.ToString());
            }

            // Close the SqlDataReader
            reader.Close();

            // Add the table to the document
            doc.Add(table);

            // Close the document
            doc.Close();

            // Notify the user
            MessageBox.Show("PDF report generated successfully.");
        }

        // Function to calculate obtained marks for a CLO
        private float GetObtainedMarksForClo(string cloId)
        {
            // Perform necessary calculations based on the provided formula and database data
            float obtainedMarks = 0; // Placeholder value, replace with actual calculation
                                     // Implement your logic to calculate obtained marks for the CLO
            return obtainedMarks;
        }

        // Function to get maximum marks for a CLO
        private float GetMaxMarksForClo(string cloId)
        {
            // Fetch maximum marks for the CLO from the database or any other source
            float maxMarks = 0; // Placeholder value, replace with actual value
                                // Implement your logic to fetch maximum marks for the CLO
            return maxMarks;
        }
    

        private void clowise_Load(object sender, EventArgs e)
        {
              try
                {
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            generatePDF();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reports m = new Reports();
            m.Show();
            this.Hide();
        } 
    }
}
