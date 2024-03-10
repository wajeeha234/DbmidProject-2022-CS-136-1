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
    public partial class Attendance : Form
    {
        public Attendance()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainForm m = new mainForm();
            m.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Assessment m = new Assessment();
            m.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {

           AssesmentComponenet m = new  AssesmentComponenet();
            m.Show();
            this.Hide();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {

        }
    }
}
