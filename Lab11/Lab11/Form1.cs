using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace COMP1019_Buoi11_Lab11
{
    public partial class Form1 : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=EduManagerDB;Integrated Security=True;TrustServerCertificate=True";
        private TextBox txtMaLop;
        private TextBox txtTenLop;
        private TextBox txtTimKiem;
        private DataGridView dgvLop;
        private Label lblTrangThai;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;

        private bool daChonLop = false;

        public Form1()
        {
            TaoGiaoDien();
            LoadData();
        }

        private void TaoGiaoDien()
        {
            this.Text = "Lab11 - Quản lý lớp học";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1050, 750);
            this.MinimumSize = new Size(900, 650);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);

            // 1. Thanh trạng thái ở dưới cùng
            Panel pnlTrangThai = new Panel();
            pnlTrangThai.Dock = DockStyle.Bottom;
            pnlTrangThai.Height = 35;
            pnlTrangThai.BackColor = Color.White;
            this.Controls.Add(pnlTrangThai);

            lblTrangThai = new Label();
            lblTrangThai.Text = "Sẵn sàng";
            lblTrangThai.Dock = DockStyle.Fill;
            lblTrangThai.Padding = new Padding(15, 0, 0, 0);
            lblTrangThai.TextAlign = ContentAlignment.MiddleLeft;
            lblTrangThai.ForeColor = Color.FromArgb(70, 80, 90);
            pnlTrangThai.Controls.Add(lblTrangThai);

            // 2. Tiêu đề ở trên cùng
            Panel pnlTieuDe = new Panel();
            pnlTieuDe.Dock = DockStyle.Top;
            pnlTieuDe.Height = 90;
            pnlTieuDe.BackColor = Color.FromArgb(30, 64, 105);
            this.Controls.Add(pnlTieuDe);

            Label lblTieuDe = new Label();
            lblTieuDe.Text = "QUẢN LÝ LỚP HỌC";
            lblTieuDe.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.AutoSize = false;
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            lblTieuDe.Dock = DockStyle.Top;
            lblTieuDe.Height = 50;
            pnlTieuDe.Controls.Add(lblTieuDe);

            Label lblPhuDe = new Label();
            lblPhuDe.Text = "LAB 11 - ADO.NET VÀ CRUD VỚI SQL SERVER";
            lblPhuDe.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblPhuDe.ForeColor = Color.FromArgb(220, 230, 245);
            lblPhuDe.Dock = DockStyle.Bottom;
            lblPhuDe.Height = 30;
            lblPhuDe.TextAlign = ContentAlignment.MiddleCenter;
            pnlTieuDe.Controls.Add(lblPhuDe);

            // 3. Panel nhập thông tin
            Panel pnlThongTin = new Panel();
            pnlThongTin.Dock = DockStyle.Top;
            pnlThongTin.Height = 200;
            pnlThongTin.Padding = new Padding(25, 10, 25, 10);
            pnlThongTin.BackColor = Color.White;
            this.Controls.Add(pnlThongTin);

            Label lblThongTin = new Label();
            lblThongTin.Text = "THÔNG TIN LỚP HỌC";
            lblThongTin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblThongTin.ForeColor = Color.FromArgb(30, 64, 105);
            lblThongTin.Location = new Point(25, 12);
            lblThongTin.AutoSize = true;
            pnlThongTin.Controls.Add(lblThongTin);

            Label lblMaLop = new Label();
            lblMaLop.Text = "Mã lớp:";
            lblMaLop.Location = new Point(30, 52);
            lblMaLop.Size = new Size(90, 30);
            pnlThongTin.Controls.Add(lblMaLop);

            txtMaLop = new TextBox();
            txtMaLop.Location = new Point(135, 50);
            txtMaLop.Size = new Size(300, 30);
            txtMaLop.MaxLength = 20;
            pnlThongTin.Controls.Add(txtMaLop);

            Label lblTenLop = new Label();
            lblTenLop.Text = "Tên lớp:";
            lblTenLop.Location = new Point(30, 92);
            lblTenLop.Size = new Size(90, 30);
            pnlThongTin.Controls.Add(lblTenLop);

            txtTenLop = new TextBox();
            txtTenLop.Location = new Point(135, 90);
            txtTenLop.Size = new Size(500, 30);
            txtTenLop.MaxLength = 100;
            pnlThongTin.Controls.Add(txtTenLop);

            btnThem = TaoNut("THÊM", Color.FromArgb(34, 139, 87));
            btnThem.Location = new Point(135, 135);
            btnThem.Size = new Size(100, 38);
            btnThem.Click += btnThem_Click;
            pnlThongTin.Controls.Add(btnThem);

            btnSua = TaoNut("SỬA", Color.FromArgb(45, 105, 180));
            btnSua.Location = new Point(245, 135);
            btnSua.Size = new Size(100, 38);
            btnSua.Click += btnSua_Click;
            pnlThongTin.Controls.Add(btnSua);

            btnXoa = TaoNut("XÓA", Color.FromArgb(200, 60, 60));
            btnXoa.Location = new Point(355, 135);
            btnXoa.Size = new Size(100, 38);
            btnXoa.Click += btnXoa_Click;
            pnlThongTin.Controls.Add(btnXoa);

            btnLamMoi = TaoNut("LÀM MỚI", Color.FromArgb(100, 110, 125));
            btnLamMoi.Location = new Point(465, 135);
            btnLamMoi.Size = new Size(120, 38);
            btnLamMoi.Click += btnLamMoi_Click;
            pnlThongTin.Controls.Add(btnLamMoi);

            // 4. Panel danh sách chứa DataGridView và tìm kiếm
            Panel pnlDanhSach = new Panel();
            pnlDanhSach.Dock = DockStyle.Fill;
            pnlDanhSach.Padding = new Padding(25, 15, 25, 15);
            pnlDanhSach.BackColor = Color.FromArgb(245, 247, 250);
            this.Controls.Add(pnlDanhSach);

            Panel pnlTimKiem = new Panel();
            pnlTimKiem.Dock = DockStyle.Top;
            pnlTimKiem.Height = 50;
            pnlDanhSach.Controls.Add(pnlTimKiem);

            Label lblTimKiem = new Label();
            lblTimKiem.Text = "Tìm kiếm:";
            lblTimKiem.Location = new Point(0, 12);
            lblTimKiem.Size = new Size(85, 30);
            pnlTimKiem.Controls.Add(lblTimKiem);

            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(90, 10);
            txtTimKiem.Size = new Size(300, 30);
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            pnlTimKiem.Controls.Add(txtTimKiem);

            btnTimKiem = TaoNut("TÌM KIẾM", Color.FromArgb(30, 64, 105));
            btnTimKiem.Location = new Point(405, 8);
            btnTimKiem.Size = new Size(120, 35);
            btnTimKiem.Click += btnTimKiem_Click;
            pnlTimKiem.Controls.Add(btnTimKiem);

            dgvLop = new DataGridView();
            dgvLop.Name = "dgvLop";
            dgvLop.Dock = DockStyle.Fill;
            dgvLop.BackgroundColor = Color.White;
            dgvLop.BorderStyle = BorderStyle.FixedSingle;
            dgvLop.ReadOnly = true;
            dgvLop.AllowUserToAddRows = false;
            dgvLop.AllowUserToDeleteRows = false;
            dgvLop.MultiSelect = false;
            dgvLop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLop.RowHeadersVisible = false;
            dgvLop.RowTemplate.Height = 35;
            dgvLop.EnableHeadersVisualStyles = false;
            dgvLop.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 64, 105);
            dgvLop.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLop.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvLop.ColumnHeadersHeight = 40;
            dgvLop.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 225, 245);
            dgvLop.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLop.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
            dgvLop.CellClick += dgvLop_CellClick;

            // Đảm bảo thứ tự add vào form theo đúng lớp hiển thị từ trên xuống dưới
            this.Controls.Add(pnlTrangThai);
            this.Controls.Add(pnlDanhSach); // Chứa DataGridView ở giữa
            this.Controls.Add(pnlThongTin); // Chứa các ô nhập liệu và nút chức năng
            this.Controls.Add(pnlTieuDe);   // Thanh tiêu đề màu xanh ở trên cùng

            pnlDanhSach.Controls.Add(dgvLop);
            dgvLop.BringToFront();
            pnlTimKiem.BringToFront();
        }

        private Button TaoNut(string text, Color color)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            return btn;
        }

        private void LoadData()
        {
            try
            {
                string sql = @"
                    SELECT l.MaLop, l.TenLop,
                           COUNT(sv.MaSV) AS SoSinhVien
                    FROM LOP l
                    LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
                    GROUP BY l.MaLop, l.TenLop
                    ORDER BY l.MaLop";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLop.DataSource = dt;
                }

                dgvLop.ClearSelection();
                daChonLop = false;
                lblTrangThai.Text = "Đã tải danh sách lớp từ EduManagerDB.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTrangThai.Text = "Tải dữ liệu thất bại.";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng nhập mã lớp.");
                txtMaLop.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lớp.");
                txtTenLop.Focus();
                return false;
            }

            return true;
        }

        private bool IsMaLopExists(string maLop)
        {
            string sql = "SELECT COUNT(*) FROM LOP WHERE MaLop = @ma";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ma", SqlDbType.VarChar, 20).Value = maLop;
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private int CountSinhVienByLop(string maLop)
        {
            string sql = "SELECT COUNT(*) FROM SINHVIEN WHERE MaLop = @ma";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ma", SqlDbType.VarChar, 20).Value = maLop;
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string ma = txtMaLop.Text.Trim();
            string ten = txtTenLop.Text.Trim();

            try
            {
                if (IsMaLopExists(ma))
                {
                    MessageBox.Show("Mã lớp đã tồn tại.");
                    txtMaLop.Focus();
                    return;
                }

                string sql = "INSERT INTO LOP(MaLop, TenLop, MaKhoa) VALUES(@ma, @ten, 'CNTT')";
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ma", SqlDbType.VarChar, 20).Value = ma;
                    cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 100).Value = ten;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm lớp thành công.");
                LoadData();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm lớp: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!daChonLop)
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa trong bảng.");
                return;
            }

            if (!ValidateInput()) return;

            string ma = txtMaLop.Text.Trim();
            string ten = txtTenLop.Text.Trim();

            try
            {
                string sql = "UPDATE LOP SET TenLop = @ten WHERE MaLop = @ma";
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ma", SqlDbType.VarChar, 20).Value = ma;
                    cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 100).Value = ten;
                    conn.Open();

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật lớp thành công.");
                        LoadData();
                        ClearInput();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy lớp cần sửa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật lớp: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!daChonLop || string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa trong bảng.");
                return;
            }

            string ma = txtMaLop.Text.Trim();

            try
            {
                if (CountSinhVienByLop(ma) > 0)
                {
                    MessageBox.Show("Không thể xóa lớp đang có sinh viên.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa lớp " + ma + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                string sql = "DELETE FROM LOP WHERE MaLop = @ma";
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ma", SqlDbType.VarChar, 20).Value = ma;
                    conn.Open();

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa lớp thành công.");
                        LoadData();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa lớp: " + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiemLop();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimKiemLop();
                e.SuppressKeyPress = true;
            }
        }

        private void TimKiemLop()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string sql = @"
                    SELECT l.MaLop, l.TenLop,
                           COUNT(sv.MaSV) AS SoSinhVien
                    FROM LOP l
                    LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
                    WHERE l.MaLop LIKE @kw OR l.TenLop LIKE @kw
                    GROUP BY l.MaLop, l.TenLop
                    ORDER BY l.MaLop";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@kw", SqlDbType.NVarChar, 100).Value = "%" + keyword + "%";
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLop.DataSource = dt;
                }

                dgvLop.ClearSelection();
                daChonLop = false;
                lblTrangThai.Text = "Đã tìm kiếm: " + keyword;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvLop.Rows[e.RowIndex];
            if (row.Cells["MaLop"].Value == null || row.Cells["TenLop"].Value == null) return;

            txtMaLop.Text = row.Cells["MaLop"].Value.ToString();
            txtTenLop.Text = row.Cells["TenLop"].Value.ToString();

            daChonLop = true;
            txtMaLop.ReadOnly = true;
            lblTrangThai.Text = "Đã chọn lớp: " + txtMaLop.Text;
        }

        private void ClearInput()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtTimKiem.Clear();

            txtMaLop.ReadOnly = false;
            daChonLop = false;

            dgvLop.ClearSelection();
            txtMaLop.Focus();
        }
    }
}