using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerControlMany
{
    public partial class Form2 : Form
    {
        public Form2(TcpClient client, PictureBox clientImage)
        {
            InitializeComponent();

            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            Thread thread1 = new Thread(() => ServerControl.ConnectToControllerAtClient(clientIP,this));
            thread1.Start();

            Thread thread = new Thread(() => BindImage(clientImage));
            thread.Start();

            this.KeyDown += Form2_KeyDown;

        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                ServerControl.CancelOperation();
                this.Close();
            }
            else
            {

            }
        }

        public void BindImage(PictureBox PB)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    pictureBox1.Image = PB.Image;
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void ShowMessage(String message, String caption)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ServerControl.SendMoveEvent(1, Cursor.Position.X, Cursor.Position.Y);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ServerControl.SendMoveEvent(2, Cursor.Position.X, Cursor.Position.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            ServerControl.SendMoveEvent(0, e.X,e.Y);
        }
    }
}
