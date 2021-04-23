using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientControlMany
{
    class Program
    {
        static int imageSequence = 0;
        static NetworkStream stream;

        public static bool SendImageMethod()
        {
            try
            {
                Console.WriteLine("Now sending image number {0}", imageSequence++);
                Byte[] confirm_message = new Byte[256];
                ScreenCapture sc = new ScreenCapture();
                Image img = sc.CaptureScreen();
                //Bitmap myimage = new Bitmap(img);

                ImageConverter converter = new ImageConverter();

                byte[] ImageInBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
                string ImageLengthInString = ImageInBytes.Length.ToString();

                Byte[] ImageLength = System.Text.Encoding.ASCII.GetBytes(ImageLengthInString);
                //byte[] ImageLength = BitConverter.GetBytes(ImageInBytes.Length);


                //Send IMAGE SIZE
                Console.WriteLine("Sending image size ==> " + ImageLengthInString);
                stream.Write(ImageLength, 0, ImageLength.Length);
                Console.WriteLine("Image size sent");


                //CONFIRM RECIEVING SIZE 
                Console.WriteLine("Recieveing confrm message");
                Int32 bytes = stream.Read(confirm_message, 0, confirm_message.Length);
                Console.WriteLine("Confirm message Recieved");

                string responseData = System.Text.Encoding.ASCII.GetString(confirm_message, 0, bytes);
                if (string.Compare(responseData, "confirm") != 0)
                {
                    return false;
                }
                Console.WriteLine("the confirmation : {0}", responseData);

                //Send THE IMAGE
                Console.WriteLine("Sending the image");
                stream.Write(ImageInBytes, 0, ImageInBytes.Length);
                Console.WriteLine("Image Sent successfully!");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR HAPPENED WHILE TRING TO SEND IMAGE CASE: => "+ex.ToString()+" >>> WE GONNA TRY TO RECONNECT");
                return false;
            }
        }

        public static string getIPOFServer()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("server.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                return streamReader.ReadLine();
            }
        }

        static void Main(string[] args)
        {

            Thread thread1 = new Thread(() => ServerControl.BeginServer());
            thread1.Start();

            while (true)
            {
                try
                {
                    
                    TcpClient client = new TcpClient(getIPOFServer(), 11000);

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes("Test");
                    Console.WriteLine("<<<<<<<<<<<<<<Client Now trying to Connect>>>>>>>>>>>>");
                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();

                    stream = client.GetStream();

                    while (true)
                    {
                        System.Threading.Thread.Sleep(3000);
                        if (SendImageMethod() == false)
                        {
                            break;
                        }
                    }

                    //Timer timer = new Timer(SendImageMethod, null, 1000, 3000);

                    // Send the message to the connected TcpServer. 
                    //stream.Write(data, 0, data.Length);

                    //Console.WriteLine("Sent: {0}", "Test");

                    //// Receive the TcpServer.response.

                    //// Buffer to store the response bytes.
                    //data = new Byte[256];

                    //// String to store the response ASCII representation.
                    //String responseData = String.Empty;

                    //// Read the first batch of the TcpServer response bytes.
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);

                    //// Close everything.
                    //stream.Close();
                    //client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
            }


            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
