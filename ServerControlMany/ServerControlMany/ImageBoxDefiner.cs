using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerControlMany
{
    class ImageBoxDefiner
    {
        public PictureBox picture;
        public TcpClient client;

        public ImageBoxDefiner(PictureBox picture, TcpClient client)
        {
            this.picture = picture;
            this.client = client;
        }
    }
}
