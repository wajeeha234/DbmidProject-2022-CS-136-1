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
    public partial class StuRes : Form
    {
        public StuRes()
        {
            InitializeComponent();
        }
        String Name;
        int assessmentId;
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                int assessmentId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                MessageBox.Show($"The value of 'assessmentId' is: {assessmentId}");
            }
            catch (Exception exp)
            {
                // Handle the exception
            }
        }


        private void StuRes_Load(object sender, EventArgs e)
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select *  from Assessment", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Evaluate";
            Update.Text = "Evaluate";
            Update.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn Delete = new DataGridViewButtonColumn();
            Delete.HeaderText = "Result";
            Delete.Text = "Result";
            Delete.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(Update);
            dataGridView1.Columns.Add(Delete);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int index = dataGridView1.CurrentCell.ColumnIndex;
            if (index == 5 || index == 1) // Check if the clicked column is either 5 or 1
            {
                try
                {
                    // Retrieve the assessmentId from the selected row
                    assessmentId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    Name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                    if (index == 5)
                    {
                        Evaluation newUserControl = new Evaluation(Name, assessmentId);
                        newUserControl.Show();
                        newUserControl.Dock = DockStyle.Fill;
                        this.Parent.Controls.Add(newUserControl);
                        newUserControl.BringToFront();
                        this.Hide();
                    }
                    else if (index == 1)
                    {
                        Result newUserControl = new Result(assessmentId, Name);
                        newUserControl.Dock = DockStyle.Fill;
                        // this.Parent.Controls.Add(newUserControl);
                        newUserControl.BringToFront();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }
    }
}
