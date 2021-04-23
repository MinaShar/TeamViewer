using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerControlMany
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Thread thread = new Thread(() => Server.BeginServer(this));
            thread.Start();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[5];
            if(IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[0];
            if (IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[1];
            if (IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[2];
            if (IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[3];
            if (IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ImageBoxDefiner IBD = Server.list[4];
            if (IBD.client == null)
            {
                Server.ShowMessage("Nothing here...", "Nothing to show!");
                return;
            }
            Form2 form = new Form2(IBD.client, IBD.picture);
            form.Show();
        }
    }
}
