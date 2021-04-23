using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;
using System.Threading;

namespace ServerControlMany
{
    class ServerControl
    {

        static TcpListener server = null;
        static TcpClient client = null;
        static NetworkStream stream = null;

        public static Object o = new object();

        static Form2 form;
        public static string ClientIP;

        static CancellationTokenSource cancellationTokenSource;
        static CancellationToken cancel;



        public static void ConnectToControllerAtClient(string clientIP, Form2 myform)
        {
            cancellationTokenSource = new CancellationTokenSource();
            ClientIP = clientIP;
            cancel = cancellationTokenSource.Token;
            form = myform;

            if (!cancel.IsCancellationRequested)
            {
                try
                {

                    client = new TcpClient(clientIP, 11001);
                    stream = client.GetStream();

                    form.ShowMessage("Connecting the controller Established", "Succeeded!");
                }
                catch (Exception e)
                {
                    form.ShowMessage("Couldnot connect the controller", "Error Occured!");
                    Thread.Sleep(2000);
                    if (!cancel.IsCancellationRequested)
                    {
                        ConnectToControllerAtClient(clientIP, myform);
                    }
                }
            }

        }


        public static void CancelOperation()
        {
            client.Close();
            cancellationTokenSource.Cancel();
        }

        public static void SendMoveEvent(int EventIndex, int X, int Y)
        {
            if (!cancel.IsCancellationRequested)
            {
                lock (o)
                {
                    if (stream == null)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            Byte[] confirm_message = new Byte[256];
                            Event newevent = new Event(EventIndex, X, Y);

                            string json = JsonConvert.SerializeObject(newevent);

                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
                            stream.Write(data, 0, data.Length);
                            Console.WriteLine("Control message Sent: {0}", json);

                            Int32 bytes = stream.Read(confirm_message, 0, confirm_message.Length);
                            Console.WriteLine("Confirm message Recieved");
                            string responseData = System.Text.Encoding.ASCII.GetString(confirm_message, 0, bytes);
                            if (string.Compare(responseData, "confirm") == 0)
                            {
                                return;
                            }
                            else
                            {
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                MessageBox.Show("Event didnot get the confirm message!", "Socket exception!", buttons);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            MessageBox.Show(e.ToString(), "Socket exception!", buttons);
                            stream = null;
                            ConnectToControllerAtClient(ClientIP, form);
                        }
                    }
                }
            }
        }
    }
}
