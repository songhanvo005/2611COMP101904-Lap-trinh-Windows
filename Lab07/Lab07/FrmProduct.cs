using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmProduct : Form
    {
        private static BindingList<Product> listProducts = new BindingList<Product>();

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            numDonGia.Maximum = 1000000000;
            numSoLuong.Maximum = 100000;

            if (cboLoaiSP.Items.Count == 0)
            {
                cboLoaiSP.Items.AddRange(new string[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
            }
            cboLoaiSP.SelectedIndex = 0;

            if (listProducts.Count == 0)
            {
                listProducts.Add(new Product { MaSP = "SP01", TenSP = "Laptop ASUS", LoaiSP = "Điện tử", DonGia = 18000000, SoLuong = 10 });
                listProducts.Add(new Product { MaSP = "SP02", TenSP = "Chuột không dây", LoaiSP = "Điện tử", DonGia = 350000, SoLuong = 50 });
            }

            dgvProducts.DataSource = listProducts;
        }

        private bool ValidateInput()
        {
            errProvider.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                errProvider.SetError(txtMaSP, "Mã SP không được để trống!");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                errProvider.SetError(txtTenSP, "Tên SP không được để trống!");
                valid = false;
            }
            if (numDonGia.Value <= 0)
            {
                errProvider.SetError(numDonGia, "Đơn giá phải lớn hơn 0!");
                valid = false;
            }
            if (numSoLuong.Value < 0)
            {
                errProvider.SetError(numSoLuong, "Số lượng không được âm!");
                valid = false;
            }

            return valid;
        }

        private void ClearInput()
        {
            txtMaSP.Clear();
            txtMaSP.Enabled = true;
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = 0;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            errProvider.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string maSP = txtMaSP.Text.Trim();
            if (listProducts.Any(p => p.MaSP.Equals(maSP, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Lỗi trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listProducts.Add(new Product
            {
                MaSP = maSP,
                TenSP = txtTenSP.Text.Trim(),
                LoaiSP = cboLoaiSP.SelectedItem.ToString(),
                DonGia = numDonGia.Value,
                SoLuong = (int)numSoLuong.Value
            });

            ClearInput();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow != null && e.RowIndex >= 0)
            {
                var sp = (Product)dgvProducts.CurrentRow.DataBoundItem;
                txtMaSP.Text = sp.MaSP;
                txtMaSP.Enabled = false;
                txtTenSP.Text = sp.TenSP;
                cboLoaiSP.SelectedItem = sp.LoaiSP;
                numDonGia.Value = sp.DonGia;
                numSoLuong.Value = sp.SoLuong;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !ValidateInput()) return;

            var sp = (Product)dgvProducts.CurrentRow.DataBoundItem;
            sp.TenSP = txtTenSP.Text.Trim();
            sp.LoaiSP = cboLoaiSP.SelectedItem.ToString();
            sp.DonGia = numDonGia.Value;
            sp.SoLuong = (int)numSoLuong.Value;

            dgvProducts.Refresh();
            ClearInput();
            MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var sp = (Product)dgvProducts.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm {sp.TenSP}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                listProducts.Remove(sp);
                ClearInput();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            dgvProducts.DataSource = listProducts;
            txtTimKiem.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower().Trim();
            var filtered = listProducts.Where(p => p.TenSP.ToLower().Contains(keyword) || p.MaSP.ToLower().Contains(keyword)).ToList();
            dgvProducts.DataSource = new BindingList<Product>(filtered);
        }
    }
}