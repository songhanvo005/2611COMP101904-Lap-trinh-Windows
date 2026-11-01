using System;
using System.Windows.Forms;
using Lab12.Forms; // Thêm namespace chứa form của bạn

namespace Lab12
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Gọi form FrmNhapDiemTheoLop làm giao diện khởi chạy chính
            Application.Run(new FrmNhapDiemTheoLop());
        }
    }
}