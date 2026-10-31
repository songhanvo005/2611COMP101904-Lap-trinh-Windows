using System;
using System.Windows.Forms;

namespace Lab05 // Đã cập nhật tên project thành Lab05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 1. Hàm Load Form (Khởi tạo dữ liệu)
        private void Form1_Load(object sender, EventArgs e)
        {
            // Nạp dữ liệu vào ComboBox
            cboKhoaHoc.Items.Clear();
            cboKhoaHoc.Items.Add("C# WinForms cơ bản");
            cboKhoaHoc.Items.Add("SQL Server cơ bản");
            cboKhoaHoc.Items.Add("Web Frontend cơ bản");
            cboKhoaHoc.Items.Add("Lập trình Python cơ bản");

            // Chọn giá trị mặc định
            cboKhoaHoc.SelectedIndex = 0;
            radOnline.Checked = true;

            numSoThang.Minimum = 1;
            numSoThang.Maximum = 12;
            numSoThang.Value = 1;

            TinhTongHocPhi();
        }

        // Hàm tính toán học phí dùng chung
        private void TinhTongHocPhi()
        {
            // Tránh lỗi khi ComboBox chưa chọn gì
            if (cboKhoaHoc.SelectedIndex == -1) return;

            decimal hocPhiThang = 0;
            string khoaHoc = cboKhoaHoc.SelectedItem.ToString();

            // Giá tiền theo yêu cầu Lab 05
            if (khoaHoc == "C# WinForms cơ bản") hocPhiThang = 800000;
            else if (khoaHoc == "SQL Server cơ bản") hocPhiThang = 700000;
            else if (khoaHoc == "Web Frontend cơ bản") hocPhiThang = 750000;
            else if (khoaHoc == "Lập trình Python cơ bản") hocPhiThang = 650000;

            decimal tongTien = hocPhiThang * numSoThang.Value;
            lblTongTien.Text = $"{tongTien:N0} VND";
        }

        // 2. Tự động tính tiền khi đổi môn học hoặc đổi số tháng
        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhTongHocPhi();
        }

        private void numSoThang_ValueChanged(object sender, EventArgs e)
        {
            TinhTongHocPhi();
        }

        // 3. Xử lý khi bấm nút Đăng ký
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được rỗng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không được rỗng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            string hinhThuc = radOnline.Checked ? "Online" : "Trực tiếp";
            string nhanEmail = chkNhanEmail.Checked ? "Có" : "Không";

            string ketQua = $"PHIẾU ĐĂNG KÝ KHÓA HỌC\n\n" +
                            $"Họ tên: {txtHoTen.Text.Trim()}\n" +
                            $"SĐT: {txtSoDienThoai.Text.Trim()}\n" +
                            $"Ngày sinh: {dtpNgaySinh.Value:dd/MM/yyyy}\n" +
                            $"Khóa học: {cboKhoaHoc.SelectedItem}\n" +
                            $"Hình thức: {hinhThuc}\n" +
                            $"Số tháng: {numSoThang.Value}\n" +
                            $"Tổng tiền: {lblTongTien.Text}\n" +
                            $"Nhận email: {nhanEmail}";

            MessageBox.Show(ketQua, "Đăng ký thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 4. Xử lý khi bấm nút Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            chkNhanEmail.Checked = false;
            cboKhoaHoc.SelectedIndex = 0;
            radOnline.Checked = true;
            numSoThang.Value = 1;
            txtHoTen.Focus();
        }

        // 5. Xử lý khi bấm nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
