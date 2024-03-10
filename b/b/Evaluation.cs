using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace b
{
    public partial class Evaluation : Form
    {
        String name;
        int id;
        int ACID;
        int name_id;
        int mid;

        public Evaluation()
        {
            InitializeComponent();
        }

        public Evaluation(String name, int id)
        {

            InitializeComponent();
            this.name = name;
            this.id = id;
            MessageBox.Show($"The value of 'assessmentId' is: {id}");
        }

        private void Evaluation_Load(object sender, EventArgs e)
        {
            label.Text = name;
            LoadAssessmentComponents();
        }

        private void LoadAssessmentComponents()
        {
            var con = Configuration.getInstance().getConnection();
            {
                //con.Open();
                SqlCommand cmd = new SqlCommand($"SELECT MeasurementLevel FROM RubricLevel where RubricId = (Select RubricId From AssessmentComponent where Id ={id} ) ", con); SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
        private void view()
        {
            var con = Configuration.getInstance().getConnection();
            //con.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM AssessmentComponent WHERE AssessmentComponent.AssessmentId = {id}", con);
            MessageBox.Show($"The value of 'id' is: {id}");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            //dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con.Close();
        }
        
      
        private void button2_Click(object sender, EventArgs e)
        {
            view();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //comboBox1.Items.Clear();
            //comboBox2.Items.Clear();
            //comboBox3.Items.Clear();
            //comboBox4.Items.Clear();
            ACID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            MessageBox.Show("Component Selected :)");
            //LoadMeasurement();
            //LoadStudents();
            //LoadStudentID();
            //LoadMeasurementID();
        }
      
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                name_id = Convert.ToInt32(comboBox1.SelectedItem.ToString());
            }
            catch (Exception exp) { }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mid = Convert.ToInt32(comboBox3.SelectedItem.ToString());
            }
            catch (Exception exp) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime1 = DateTime.Now;
            string dateC = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");
            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO StudentResult VALUES ({name_id}, {ACID}, {mid}, @dateC)", con);
                cmd.Parameters.AddWithValue("@dateC", dateC);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Evaluated");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
