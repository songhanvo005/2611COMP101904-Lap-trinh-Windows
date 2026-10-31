using System;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmMain : Form
    {
        private User currentUser;

        public FrmMain(User user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (currentUser != null)
            {
                lblUserInfo.Text = $"Người dùng: {currentUser.FullName} ({currentUser.Role})";
            }
            lblTime.Text = "Thời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = "Thời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        // Mở Form con MDI không bị trùng lặp
        private void OpenMdiChildForm<T>() where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Activate();
                    return;
                }
            }
            T newChild = new T();
            newChild.MdiParent = this;
            newChild.Show();
        }

        private void menuQuanLySanPham_Click(object sender, EventArgs e)
        {
            OpenMdiChildForm<FrmProduct>();
        }

        private void menuQuanLyKhachHang_Click(object sender, EventArgs e)
        {
            OpenMdiChildForm<FrmCustomer>();
        }

        private void menuGioiThieu_Click(object sender, EventArgs e)
        {
            FrmAbout frmAbout = new FrmAbout();
            frmAbout.ShowDialog();
        }

        private void menuDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất khỏi hệ thống?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}