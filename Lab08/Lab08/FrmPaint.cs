using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab08
{
    public partial class FrmPaint : Form
    {
        public FrmPaint()
        {
            Text = "COMP1019 - Simple Paint (Lab 08)";
            Width = 1050;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            // Gọi 2 hàm này để tạo giao diện và gắn sự kiện thay vì dùng InitializeComponent
            BuildUserInterface();
            RegisterEvents();
        }
    }
}
