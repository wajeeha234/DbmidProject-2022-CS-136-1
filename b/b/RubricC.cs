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
    public partial class RubricC : Form
    {
        public RubricC()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainForm m = new mainForm();
            m.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rubric_Level m = new Rubric_Level();
            m.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Rubric m = new Rubric();
            m.Show();
            this.Hide();
        }
    }
}
