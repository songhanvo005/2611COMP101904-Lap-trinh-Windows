using System.Drawing;
using System.Windows.Forms;

namespace Lab08
{
    public class DrawSurface : Panel
    {
        public DrawSurface()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Dock = DockStyle.Fill;
        }
    }
}