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
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddStu newForm = new AddStu();
            newForm.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StuUpdate f = new StuUpdate();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StuDelete s = new StuDelete();
            s.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchStu s = new SearchStu();
            s.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
         
            mainForm m = new mainForm();
            m.Show();
            this.Hide();

        }
    }
}
