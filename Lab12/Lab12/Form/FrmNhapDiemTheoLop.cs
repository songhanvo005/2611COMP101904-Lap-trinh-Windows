using System;
using System.Data;
using System.Windows.Forms;
using Lab12.DAL;

namespace Lab12.Forms
{
    public partial class FrmNhapDiemTheoLop : Form
    {
        private KetQuaDAL dal = new KetQuaDAL();
        private BindingSource bindingSource = new BindingSource();

        // Khai báo các điều khiển trên giao diện
        private ComboBox cboLop;
        private ComboBox cboMonHoc;
        private Button btnTaiDanhSach;
        private DataGridView dgvSinhVien;
        private Button btnLuu;
        private Label lblLop;
        private Label lblMon;

        public FrmNhapDiemTheoLop()
        {
            KhoiTaoGiaoDienCode();
            LoadComboboxData();
        }

        // Thiết kế giao diện hoàn toàn bằng code (Clean & Dynamic)
        private void KhoiTaoGiaoDienCode()
        {
            this.Text = "Nhập Điểm Theo Lớp";
            this.Width = 750;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblLop = new Label() { Text = "Chọn Lớp:", Left = 20, Top = 20, Width = 70 };
            cboLop = new ComboBox() { Left = 100, Top = 18, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            lblMon = new Label() { Text = "Chọn Môn:", Left = 330, Top = 20, Width = 70 };
            cboMonHoc = new ComboBox() { Left = 410, Top = 18, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            btnTaiDanhSach = new Button() { Text = "Tải Danh Sách", Left = 620, Top = 16, Width = 95, Height = 25 };
            btnTaiDanhSach.Click += BtnTaiDanhSach_Click;

            dgvSinhVien = new DataGridView()
            {
                Left = 20,
                Top = 60,
                Width = 695,
                Height = 380,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnLuu = new Button() { Text = "Lưu Thay Đổi", Left = 600, Top = 460, Width = 115, Height = 35 };
            btnLuu.BackColor = System.Drawing.Color.LightGreen;
            btnLuu.Click += BtnLuu_Click;

            this.Controls.Add(lblLop);
            this.Controls.Add(cboLop);
            this.Controls.Add(lblMon);
            this.Controls.Add(cboMonHoc);
            this.Controls.Add(btnTaiDanhSach);
            this.Controls.Add(dgvSinhVien);
            this.Controls.Add(btnLuu);
        }

        // Đổ dữ liệu vào ComboBox Lớp và Môn Học lúc mở form
        private void LoadComboboxData()
        {
            try
            {
                cboLop.DataSource = dal.GetAllLop();
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";

                cboMonHoc.DataSource = dal.GetAllMonHoc();
                cboMonHoc.DisplayMember = "TenMon";
                cboMonHoc.ValueMember = "MaMon";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện bấm nút Tải danh sách sinh viên theo lớp và môn
        private void BtnTaiDanhSach_Click(object sender, EventArgs e)
        {
            if (cboLop.SelectedValue == null || cboMonHoc.SelectedValue == null) return;

            string maLop = cboLop.SelectedValue.ToString();
            string maMon = cboMonHoc.SelectedValue.ToString();

            try
            {
                DataTable dt = dal.GetDanhSachDiem(maLop, maMon);
                bindingSource.DataSource = dt;
                dgvSinhVien.DataSource = bindingSource;

                // Tùy chỉnh hiển thị cột trên DataGridView
                if (dgvSinhVien.Columns["MaSV"] != null) dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
                if (dgvSinhVien.Columns["HoTen"] != null) dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và tên";
                if (dgvSinhVien.Columns["Diem"] != null) dgvSinhVien.Columns["Diem"].HeaderText = "Điểm số";
                if (dgvSinhVien.Columns["GhiChu"] != null) dgvSinhVien.Columns["GhiChu"].HeaderText = "Ghi chú";

                // Khóa không cho sửa Mã SV và Họ tên, chỉ cho phép sửa cột Điểm và Ghi chú
                if (dgvSinhVien.Columns["MaSV"] != null) dgvSinhVien.Columns["MaSV"].ReadOnly = true;
                if (dgvSinhVien.Columns["HoTen"] != null) dgvSinhVien.Columns["HoTen"].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện bấm nút Lưu điểm sử dụng cơ chế Transaction
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (cboMonHoc.SelectedValue == null) return;
            string maMon = cboMonHoc.SelectedValue.ToString();

            try
            {
                dgvSinhVien.EndEdit(); // Kết thúc quá trình chỉnh sửa trên grid
                DataTable dtChanges = ((DataTable)bindingSource.DataSource).GetChanges();

                if (dtChanges == null || dtChanges.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu thay đổi nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool success = dal.SaveDanhSachDiem(dtChanges, maMon);
                if (success)
                {
                    MessageBox.Show("Lưu bảng điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Nạp lại dữ liệu sau khi lưu
                    BtnTaiDanhSach_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu (Transaction đã rollback): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}