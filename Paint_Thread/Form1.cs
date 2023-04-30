using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_Thread
{
    public partial class Form1 : Form
    {
        private Animator animator;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Красный");
            comboBox1.Items.Add("Зеленый");
            animator = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
            animator.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            animator.AddBall();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            animator.Stop();
        }
    }
}
