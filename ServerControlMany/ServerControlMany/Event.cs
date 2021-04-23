using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerControlMany
{
    class Event
    {
        public static string[] availableEvents = { "move", "click" , "dbclick" };

        public string Name { get; set; }
        public int Xposition { get; set; }
        public int Yposition { get; set; }

        public Event(int EventIndex , int x , int y)
        {
            this.Name = availableEvents[EventIndex];
            this.Xposition = x;
            this.Yposition = y;
        }
    }
}
