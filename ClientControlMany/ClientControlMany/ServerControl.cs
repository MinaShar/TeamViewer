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

namespace ClientControlMany
{
    class ServerControl
    {

        static TcpListener server = null;
        static TcpClient client = null;
        static NetworkStream stream = null;

        public static Object o = new object();


        public static void BeginServer()
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 11001);

            listener.Start();

            Console.WriteLine("Now CONTROLLER LISTENING ON " + listener.LocalEndpoint.ToString(), "Listening");

            var task = AcceptClientsAsync(listener);

            //Console.ReadKey();
            //cancellationTokenSource.Cancel();
            //task.Wait();
            //Console.WriteLine("end");

            //Console.ReadKey(true);
        }

        public static async Task AcceptClientsAsync(TcpListener listener)
        {

            await Task.Yield();
            while (true)
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

                    stream = client.GetStream();
                    RecieveEvents();
                }

                catch (Exception aex)
                {
                    var ex = aex.GetBaseException();
                    Console.WriteLine("Error THROWN while trying to connect user: " + ex.Message, "ERROR");
                }

            }

        }

        public static void RecieveEvents()
        {
            
            while (true)
            {
                try
                {
                    Byte[] data = new Byte[256];
                    String responseData = String.Empty;

                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    Console.WriteLine("Controller Message Received: {0}", responseData);
                    Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

                    ///send confirm back
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes("confirm");
                    stream.Write(msg, 0, msg.Length);

                    ControlMessageParser.ControlMessageExcuter(responseData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    break;
                }
            }
        }
    }
}
