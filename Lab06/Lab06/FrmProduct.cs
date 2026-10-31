using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Lab06
{
    public partial class FrmProduct : Form
    {
        private BindingList<Product> danhSachSP = new BindingList<Product>();

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            cboLoaiSP.Items.Clear();
            cboLoaiSP.Items.AddRange(new string[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
            cboLoaiSP.SelectedIndex = 0; // Tự động chọn "Điện tử"

            // Tạo dữ liệu mẫu
            danhSachSP.Add(new Product { MaSP = "SP01", TenSP = "Laptop Dell", LoaiSP = "Điện tử", DonGia = 15000000, SoLuong = 5, NgayNhap = DateTime.Now, ConKinhDoanh = true });
            danhSachSP.Add(new Product { MaSP = "SP02", TenSP = "Bút bi", LoaiSP = "Văn phòng", DonGia = 5000, SoLuong = 100, NgayNhap = DateTime.Now, ConKinhDoanh = true });
            danhSachSP.Add(new Product { MaSP = "SP03", TenSP = "Tủ lạnh", LoaiSP = "Gia dụng", DonGia = 8000000, SoLuong = 2, NgayNhap = DateTime.Now, ConKinhDoanh = false });

            dgvProducts.DataSource = danhSachSP;
            lblStatus.Text = $"Tổng số sản phẩm: {danhSachSP.Count}";
        }

        private bool ValidateInput()
        {
            errProvider.Clear();
            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtMaSP.Text)) { errProvider.SetError(txtMaSP, "Không được rỗng"); hopLe = false; }
            if (string.IsNullOrWhiteSpace(txtMaSP.Text)) { errProvider.SetError(txtMaSP, "Không được rỗng"); hopLe = false; }
            if (numDonGia.Value <= 0) { errProvider.SetError(numDonGia, "Đơn giá phải > 0"); hopLe = false; }
            if (numSoLuong.Value < 0) { errProvider.SetError(numSoLuong, "Số lượng không được âm"); hopLe = false; }
            if (dtpNgayNhap.Value > DateTime.Now) { errProvider.SetError(dtpNgayNhap, "Ngày nhập không hợp lệ"); hopLe = false; }

            return hopLe;
        }

        private void ClearInput()
        {
            txtMaSP.Clear();
            txtMaSP.Enabled = true;
            txtTenSP.Clear();
            if (cboLoaiSP.Items.Count > 0) cboLoaiSP.SelectedIndex = 0;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            dtpNgayNhap.Value = DateTime.Now;
            chkConKinhDoanh.Checked = false;
            picSanPham.Image = null;
            txtMaSP.Tag = null;
            errProvider.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            if (cboLoaiSP.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (danhSachSP.Any(p => p.MaSP.Equals(txtMaSP.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            danhSachSP.Add(new Product
            {
                MaSP = txtMaSP.Text.Trim(),
                TenSP = txtTenSP.Text.Trim(),
                LoaiSP = cboLoaiSP.SelectedItem.ToString(),
                DonGia = numDonGia.Value,
                SoLuong = (int)numSoLuong.Value,
                NgayNhap = dtpNgayNhap.Value,
                ConKinhDoanh = chkConKinhDoanh.Checked,
                DuongDanAnh = txtMaSP.Tag?.ToString()
            });

            ClearInput();
            lblStatus.Text = $"Đã thêm. Tổng số SP: {danhSachSP.Count}";
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow != null && e.RowIndex >= 0)
            {
                var sp = (Product)dgvProducts.CurrentRow.DataBoundItem;
                txtMaSP.Text = sp.MaSP; txtMaSP.Enabled = false;
                txtMaSP.Text = sp.TenSP;
                cboLoaiSP.SelectedItem = sp.LoaiSP;
                numDonGia.Value = sp.DonGia;
                numSoLuong.Value = sp.SoLuong;
                dtpNgayNhap.Value = sp.NgayNhap;
                chkConKinhDoanh.Checked = sp.ConKinhDoanh;

                if (!string.IsNullOrEmpty(sp.DuongDanAnh) && File.Exists(sp.DuongDanAnh))
                    picSanPham.Image = Image.FromFile(sp.DuongDanAnh);
                else picSanPham.Image = null;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !ValidateInput()) return;

            var sp = (Product)dgvProducts.CurrentRow.DataBoundItem;
            sp.TenSP = txtMaSP.Text.Trim(); sp.LoaiSP = cboLoaiSP.SelectedItem.ToString();
            sp.DonGia = numDonGia.Value; sp.SoLuong = (int)numSoLuong.Value;
            sp.NgayNhap = dtpNgayNhap.Value; sp.ConKinhDoanh = chkConKinhDoanh.Checked;
            sp.DuongDanAnh = txtMaSP.Tag?.ToString();

            dgvProducts.Refresh();
            ClearInput();
            lblStatus.Text = "Đã cập nhật sản phẩm!";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            if (MessageBox.Show("Xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                danhSachSP.Remove((Product)dgvProducts.CurrentRow.DataBoundItem);
                ClearInput();
                lblStatus.Text = $"Đã xóa. Tổng số SP: {danhSachSP.Count}";
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            dgvProducts.DataSource = danhSachSP;
            txtTimKiem.Clear();
            lblStatus.Text = $"Tổng số sản phẩm: {danhSachSP.Count}";
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.ToLower().Trim();
            var locSP = danhSachSP.Where(p => p.TenSP.ToLower().Contains(tuKhoa) || p.MaSP.ToLower().Contains(tuKhoa)).ToList();
            dgvProducts.DataSource = new BindingList<Product>(locSP);
            lblStatus.Text = $"Tìm thấy {locSP.Count} sản phẩm.";
        }

        private void btnChonAnh_Click_1(object sender, EventArgs e)
        {
            // Cấu hình bộ lọc chỉ hiển thị các file ảnh
            ofdChonAnh.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            ofdChonAnh.Title = "Chọn hình ảnh sản phẩm";

            // Mở hộp thoại chọn file
            if (ofdChonAnh.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Hiển thị ảnh lên PictureBox
                    picSanPham.Image = Image.FromFile(ofdChonAnh.FileName);

                    // Chỉnh chế độ co giãn ảnh vừa khung
                    picSanPham.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Lưu đường dẫn ảnh tạm thời vào thuộc tính Tag của txtMaSP
                    txtMaSP.Tag = ofdChonAnh.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải hình ảnh này: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}