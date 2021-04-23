using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ClientControlMany
{
    class ControlMessageParser
    {

        [DllImport("C:\\Windows\\System32\\user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        public static void ControlMessageExcuter(string message)
        {
            Event RecievedEvent = JsonConvert.DeserializeObject<Event>(message);
            switch (RecievedEvent.Name)
            {
                case "move":
                    Cursor.Position = new Point(RecievedEvent.Xposition, RecievedEvent.Yposition);
                    break;
                case "click":
                    uint X = (uint)RecievedEvent.Xposition;
                    uint Y = (uint)RecievedEvent.Yposition;
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                    break;
                case "dbclick":
                    uint Xi = (uint)RecievedEvent.Xposition;
                    uint Yi = (uint)RecievedEvent.Yposition;
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Xi, Yi, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Xi, Yi, 0, 0);
                    break;

            }
        }
    }
}
