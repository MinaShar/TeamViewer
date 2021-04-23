using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerControlMany
{
    class Server
    {
        static int ConnectedUsers = 0;
        public static List<ImageBoxDefiner> list;

        public static List<ImageBoxDefiner> CreateImagesList(Form1 form)
        {
            List<ImageBoxDefiner> list = new List<ImageBoxDefiner>();
            list.Add(new ImageBoxDefiner(form.pictureBox1, null) );
            list.Add(new ImageBoxDefiner(form.pictureBox2, null));
            list.Add(new ImageBoxDefiner(form.pictureBox3, null));
            list.Add(new ImageBoxDefiner(form.pictureBox4, null));
            list.Add(new ImageBoxDefiner(form.pictureBox5, null));
            list.Add(new ImageBoxDefiner(form.pictureBox6, null));

            return list;
        }

        public static void BeginServer(Form1 form)
        {
            list = CreateImagesList(form);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CancellationToken cancel = cancellationTokenSource.Token;

            TcpListener listener = new TcpListener(IPAddress.Any, 11000);

            listener.Start();

            ShowMessage("Service listening at " + listener.LocalEndpoint.ToString(), "Listening");

            var task = AcceptClientsAsync(listener, cancel);

            //Console.ReadKey();
            //cancellationTokenSource.Cancel();
            //task.Wait();
            //Console.WriteLine("end");

            //Console.ReadKey(true);
        }

        public static async Task AcceptClientsAsync(TcpListener listener, CancellationToken cancel)
        {

            await Task.Yield();
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    var timeoutTask = Task.Delay(2000);
                    Task<TcpClient> acceptTask = listener.AcceptTcpClientAsync();
                    await Task.WhenAny(timeoutTask, acceptTask);
                    if (!acceptTask.IsCompleted)
                    {
                        continue;
                    }
                    TcpClient client = await acceptTask;

                    ////////////GET THE AVAILABLE iMAGEbOXdEFINER/////////////////////
                    ImageBoxDefiner IBD=getAvailableImageBoxDefiner(client);
                    HandleClientAsync(client, cancel,IBD);
                }

                catch (Exception aex)
                {
                    var ex = aex.GetBaseException();
                    ShowMessage("Error THROWN while trying to connect user: " + ex.Message, "ERROR");
                }

            }

        }

        public static ImageBoxDefiner getAvailableImageBoxDefiner(TcpClient client)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].client == null)
                {
                    list[i].client = client;
                    return list[i];
                }
            }
            return null;
        }


        public static async Task HandleClientAsync(TcpClient client, CancellationToken cancel,ImageBoxDefiner ClientImage)
        {
            await Task.Yield();
            ConnectedUsers++;
            var local = client.Client.LocalEndPoint.ToString();
            ShowMessage("Connected " + local, "User conected successfully");

            
            try
            {
                var stream = client.GetStream();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (!cancel.IsCancellationRequested && client.Connected)
                {
                    int i = stream.Read(bytes, 0, bytes.Length);

                    // Translate data bytes to a ASCII string.

                    //recieve the image size 
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);
                    int ImageSize = int.Parse(data);

                    ///send confirm back
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes("confirm");
                    stream.Write(msg, 0, msg.Length);

                    ///recieve image
                    byte[] ImageRecieved = new byte[ImageSize];
                    i = stream.Read(ImageRecieved, 0, ImageRecieved.Length);

                    MemoryStream memory_stream = new MemoryStream(ImageRecieved, 0, i);
                    Image my_image = Image.FromStream(memory_stream);
                    //Image my_image = (Image)converter.ConvertFrom(bytes);
                    ClientImage.picture.Image = my_image;

                    // Process the data sent by the client.
                    //data = data.ToUpper();

                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    //stream.Write(msg, 0, msg.Length);
                    //Console.WriteLine("Sent: {0}", data);

                }

            }

            catch (Exception aex)
            {
                var ex = aex.GetBaseException();
                ShowMessage(ex.ToString(), "Error!");
            }

            finally
            {
                ShowMessage("Disconnected " + local, "SomeoneDisconnected");
                if (ClientImage.client.Connected)
                {
                    ClientImage.client.Close();
                }
                ClientImage.client = null;
            }
        }


        public static void ShowMessage(String message, String caption)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
        }
    }
}
