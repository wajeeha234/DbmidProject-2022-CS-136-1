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
    public partial class ManageAttendence : Form
    {
        bool check_date = false;
        bool check_update = false;
        int id;
        public ManageAttendence()
        {
            InitializeComponent();
        }
        private void ManageAttendence_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
            SetupDataGridView();
        }
        private void SetupDataGridView()
        {
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.HeaderText = "Attendance Status";
            comboBoxColumn.Name = "cmbAttendanceStatus";
            comboBoxColumn.Items.AddRange("Present", "Absent", "Late", "Leave");
            dataGridView1.Columns.Add(comboBoxColumn);
        }
        private void update_Click(object sender, EventArgs e)
        {
            check_update = true;
        }

        private void RefreshDataGridView()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT TOP 2 Id , Concat(Student.FirstName,Student.LastName)  as StudentName, RegistrationNumber FROM Student WHERE Status = 5", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void load1(string sqlDateTime)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Insert into ClassAttendance values (@date)", con);
            cmd.Parameters.AddWithValue("@date", (sqlDateTime));
            cmd.ExecuteNonQuery();
        }
        private void load2(string sqlDateTime)
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("select max(Id) from ClassAttendance where AttendanceDate=@date", con2);
            cmd2.Parameters.AddWithValue("@date", sqlDateTime);
            cmd2.ExecuteNonQuery();
            id = (Int32)cmd2.ExecuteScalar();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDateTime = dateTimePicker1.Value;
                string sqlDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                load1(sqlDateTime);
                load2(sqlDateTime);

                //   MessageBox.Show(id.ToString());
                if (dataGridView1.Rows.Count != 0)
                {
                    // Fetch or generate the class attendance identifier
                    int classAttendanceId = GetClassAttendanceId();

                    foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                    {
                        // Retrieve registration number
                        string registrationNumber = selectedRow.Cells["RegistrationNumber"].Value?.ToString();
                        if (string.IsNullOrEmpty(registrationNumber))
                        {
                            MessageBox.Show("Registration number not found in the selected row.");
                            continue; // Move to the next row
                        }

                        // Retrieve attendance status
                        string selectedStatus = selectedRow.Cells["cmbAttendanceStatus"].Value?.ToString();
                        if (string.IsNullOrEmpty(selectedStatus))
                        {
                            MessageBox.Show("Attendance status not found in the selected row.");
                            continue; // Move to the next row
                        }

                        // Fetch the lookup ID for the selected attendance status
                        int statusId = GetStatusId(selectedStatus);
                        if (statusId == -1)
                        {
                            MessageBox.Show($"Status '{selectedStatus}' not found in the Lookup table.");
                            continue; // Move to the next row
                        }

                        // Get the student ID based on the registration number
                        int studentId = GetStudentId(registrationNumber);
                        if (studentId == -1)
                        {
                            MessageBox.Show($"Student with registration number '{registrationNumber}' not found.");
                            continue; // Move to the next row
                        }

                        // Save attendance record
                        InsertAttendanceRecord(studentId, statusId, classAttendanceId);
                    }

                    MessageBox.Show("Attendance saved successfully.");
                }
                else
                {
                    MessageBox.Show("Please select a student.");
                }
            }
            catch (Exception exp) { MessageBox.Show(exp.Message.ToString()); }
        }

        private int GetClassAttendanceId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO ClassAttendance (AttendanceDate) VALUES (@AttendanceDate); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@AttendanceDate", DateTime.Now);

            // Use ExecuteScalar to get the auto-generated Id
            int classAttendanceId = Convert.ToInt32(cmd.ExecuteScalar());

            return classAttendanceId;
        }
        private void InsertAttendanceRecord(int studentId, int attendanceStatusId, int classAttendanceId)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance (StudentId, AttendanceStatus, AttendanceId) VALUES (@StudentId, @AttendanceStatus, @AttendanceId)", con);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@AttendanceStatus", attendanceStatusId);
            cmd.Parameters.AddWithValue("@AttendanceId", classAttendanceId);
            cmd.ExecuteNonQuery();
        }

        private void InsertAttendanceRecord(int studentId, int attendanceStatusId)
        {
            var con = Configuration.getInstance().getConnection();
            //   SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance (StudentId, AttendanceStatus) VALUES (@StudentId, @AttendanceStatus)", con);
            SqlCommand cmd = new SqlCommand("Select AttendanceId, AttendanceDate, Concat(FirstName, LastName) as StudentName, RegistrationNumber from StudentAttendance AS S join Student as SA on S.StudentId = SA.Id join ClassAttendance as C ON C.Id = S.AttendanceId", con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@AttendanceStatus", attendanceStatusId);
            cmd.ExecuteNonQuery();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);

            if (dateTimePicker1.Value.Year != 2024)
            {
                MessageBox.Show("Please select a date within the year 2023");
                check_date = false;
                return;
            }
            else
            {
                check_date = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (check_update == true)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select   AttendanceId, AttendanceDate, Concat(FirstName, LastName) as StudentName, RegistrationNumber            from StudentAttendance AS S join Student as SA on S.StudentId = SA.Id    join ClassAttendance as C           ON C.Id = S.AttendanceId", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                SetupDataGridView();
                check_update = false;
            }
        }

        private int GetStudentId(string registrationNumber)
        {
            int studentId = -1; // Default value if student is not found
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Student WHERE RegistrationNumber = @RegistrationNumber", con);
            cmd.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                studentId = (int)result;
            }
            return studentId;
        }

        private int GetStatusId(string statusName)
        {
            int statusId = -1; // Default value if status is not found
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT LookupId FROM Lookup WHERE Name = @Name AND Category = 'ATTENDANCE_STATUS'", con);
            cmd.Parameters.AddWithValue("@Name", statusName);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                statusId = (int)result;
            }
            return statusId;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainForm m = new mainForm();
            m.Show();
            this.Hide();
        }
    }
}
