﻿using System;
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
    public partial class Result : Form
    {

        int id; string name;
        string x;
        int am;
        int acm;
        public Result()
        {
            InitializeComponent();
        }

        private void Result_Load(object sender, EventArgs e)
        {

        }
        public Result(int id, string name)
        {
            InitializeComponent();
            this.id = id;
            this.name = name;
        }
    }
}
