using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace b
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Student m = new Student();
            m.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           ManageAttendence m = new ManageAttendence();
            m.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Attendance m = new Attendance();
            m.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            CLO_S m = new CLO_S();
            m.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            RubricC M = new RubricC();
            M.Show();
            this.Hide();

          }

        private void button6_Click(object sender, EventArgs e)
        {

            StudentResult M = new StudentResult();
            M.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            Reports M = new Reports();
            M.Show();
            this.Hide();
        }
    }
}
