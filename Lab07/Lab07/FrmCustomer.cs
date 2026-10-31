using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmCustomer : Form
    {
        private static BindingList<Customer> listCustomers = new BindingList<Customer>();

        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            if (cboLoaiKhach.Items.Count == 0)
            {
                cboLoaiKhach.Items.AddRange(new string[] { "Thường", "Thân thiết", "VIP" });
            }
            cboLoaiKhach.SelectedIndex = 0;

            if (listCustomers.Count == 0)
            {
                listCustomers.Add(new Customer { MaKH = "KH01", HoTen = "Nguyễn Văn A", SoDienThoai = "0901234567", LoaiKhach = "VIP" });
                listCustomers.Add(new Customer { MaKH = "KH02", HoTen = "Trần Thị B", SoDienThoai = "0987654321", LoaiKhach = "Thường" });
            }

            dgvCustomers.DataSource = listCustomers;
        }

        private bool ValidateInput()
        {
            errProvider.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                errProvider.SetError(txtMaKH, "Mã khách hàng không được để trống!");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errProvider.SetError(txtHoTen, "Họ tên khách hàng không được để trống!");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                errProvider.SetError(txtSoDienThoai, "Số điện thoại không được để trống!");
                valid = false;
            }

            return valid;
        }

        private void ClearInput()
        {
            txtMaKH.Clear();
            txtMaKH.Enabled = true;
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            cboLoaiKhach.SelectedIndex = 0;
            errProvider.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string maKH = txtMaKH.Text.Trim();
            if (listCustomers.Any(c => c.MaKH.Equals(maKH, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã khách hàng đã tồn tại!", "Lỗi trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listCustomers.Add(new Customer
            {
                MaKH = maKH,
                HoTen = txtHoTen.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                LoaiKhach = cboLoaiKhach.SelectedItem.ToString()
            });

            ClearInput();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomers.CurrentRow != null && e.RowIndex >= 0)
            {
                var kh = (Customer)dgvCustomers.CurrentRow.DataBoundItem;
                txtMaKH.Text = kh.MaKH;
                txtMaKH.Enabled = false; // Khóa mã KH khi đang Sửa
                txtHoTen.Text = kh.HoTen;
                txtSoDienThoai.Text = kh.SoDienThoai;
                cboLoaiKhach.SelectedItem = kh.LoaiKhach;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null || !ValidateInput()) return;

            var kh = (Customer)dgvCustomers.CurrentRow.DataBoundItem;
            kh.HoTen = txtHoTen.Text.Trim();
            kh.SoDienThoai = txtSoDienThoai.Text.Trim();
            kh.LoaiKhach = cboLoaiKhach.SelectedItem.ToString();

            dgvCustomers.Refresh();
            ClearInput();
            MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;

            var kh = (Customer)dgvCustomers.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {kh.HoTen}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                listCustomers.Remove(kh);
                ClearInput();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            dgvCustomers.DataSource = listCustomers;
            txtTimKiem.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower().Trim();
            var filtered = listCustomers.Where(c => c.HoTen.ToLower().Contains(keyword)
                                                 || c.MaKH.ToLower().Contains(keyword)
                                                 || c.SoDienThoai.Contains(keyword)).ToList();
            dgvCustomers.DataSource = new BindingList<Customer>(filtered);
        }
    }
}