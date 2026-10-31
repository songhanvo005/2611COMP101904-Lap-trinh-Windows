using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmLogin : Form
    {
        public User CurrentUser { get; private set; }

        private List<User> listUsers = new List<User>
        {
            new User { Username = "admin", Password = "123", FullName = "Quản trị viên", Role = "Admin" },
            new User { Username = "user", Password = "123", FullName = "Nhân viên bán hàng", Role = "User" }
        };

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errProvider.Clear();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                errProvider.SetError(txtUsername, "Vui lòng nhập tên đăng nhập!");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                errProvider.SetError(txtPassword, "Vui lòng nhập mật khẩu!");
                return;
            }

            var user = listUsers.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                                  && u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}