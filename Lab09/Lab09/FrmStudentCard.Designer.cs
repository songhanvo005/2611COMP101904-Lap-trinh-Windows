using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Lab09
{
    public partial class FrmStudentCard : Form
    {
        // 1. Khai báo các Control UI
        private TextBox txtStudentId = new TextBox();
        private TextBox txtFullName = new TextBox();
        private ComboBox cboClass = new ComboBox();
        private ComboBox cboFaculty = new ComboBox();
        private TextBox txtCourse = new TextBox();
        private DateTimePicker dtpBirthDate = new DateTimePicker();
        private ComboBox cboGender = new ComboBox();
        private PictureBox picAvatar = new PictureBox();
        private Panel pnlCardPreview = new Panel();

        private Button btnChoosePhoto = new Button();
        private Button btnPreviewPrint = new Button();
        private Button btnPrint = new Button();
        private Button btnClear = new Button();
        private Button btnSaveImage = new Button();

        // 2. Khai báo công cụ In ấn
        private PrintDocument printDocument1 = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        private PrintDialog printDialog1 = new PrintDialog();

        public FrmStudentCard()
        {
            Text = "Lab 09 - Thiết kế và In Thẻ Sinh Viên";
            Size = new Size(950, 580);
            StartPosition = FormStartPosition.CenterScreen;

            // 3 lệnh cực kỳ quan trọng để vẽ giao diện và nạp dữ liệu
            BuildUI();
            InitializeData();
            RegisterEvents();
        }

        // ==============================================================
        // PHẦN 1: KHỞI TẠO GIAO DIỆN VÀ DỮ LIỆU
        // ==============================================================
        private void BuildUI()
        {
            // Panel Nhập liệu (Cột trái)
            GroupBox grpInput = new GroupBox { Text = "Thông tin Sinh viên", Bounds = new Rectangle(15, 15, 300, 500) };

            Label lblId = new Label { Text = "MSSV (*):", Location = new Point(15, 30), AutoSize = true };
            txtStudentId.Bounds = new Rectangle(90, 27, 190, 25);

            Label lblName = new Label { Text = "Họ tên (*):", Location = new Point(15, 65), AutoSize = true };
            txtFullName.Bounds = new Rectangle(90, 62, 190, 25);

            Label lblClass = new Label { Text = "Lớp:", Location = new Point(15, 100), AutoSize = true };
            cboClass.Bounds = new Rectangle(90, 97, 190, 25);
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblFaculty = new Label { Text = "Khoa:", Location = new Point(15, 135), AutoSize = true };
            cboFaculty.Bounds = new Rectangle(90, 132, 190, 25);
            cboFaculty.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblCourse = new Label { Text = "Khóa:", Location = new Point(15, 170), AutoSize = true };
            txtCourse.Bounds = new Rectangle(90, 167, 190, 25);

            Label lblBirth = new Label { Text = "Ngày sinh:", Location = new Point(15, 205), AutoSize = true };
            dtpBirthDate.Bounds = new Rectangle(90, 202, 190, 25);
            dtpBirthDate.Format = DateTimePickerFormat.Short;

            Label lblGender = new Label { Text = "Giới tính:", Location = new Point(15, 240), AutoSize = true };
            cboGender.Bounds = new Rectangle(90, 237, 190, 25);
            cboGender.DropDownStyle = ComboBoxStyle.DropDownList;

            btnChoosePhoto.Text = "Chọn ảnh (*)";
            btnChoosePhoto.Bounds = new Rectangle(15, 280, 100, 30);

            picAvatar.Bounds = new Rectangle(130, 280, 150, 200);
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            grpInput.Controls.AddRange(new Control[] {
                lblId, txtStudentId, lblName, txtFullName, lblClass, cboClass,
                lblFaculty, cboFaculty, lblCourse, txtCourse, lblBirth, dtpBirthDate,
                lblGender, cboGender, btnChoosePhoto, picAvatar
            });

            // Panel Preview Thẻ (Cột phải)
            GroupBox grpPreview = new GroupBox { Text = "Xem trước Thẻ", Bounds = new Rectangle(330, 15, 580, 380) };
            pnlCardPreview.Bounds = new Rectangle(15, 25, 550, 340);
            pnlCardPreview.BackColor = Color.White;
            pnlCardPreview.BorderStyle = BorderStyle.Fixed3D;
            // Bật DoubleBuffered cho Panel để vẽ mượt, không nháy
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pnlCardPreview, new object[] { true });

            grpPreview.Controls.Add(pnlCardPreview);

            // Các nút Điều khiển phía dưới
            btnPreviewPrint.Text = "Xem trước In";
            btnPreviewPrint.Bounds = new Rectangle(330, 410, 120, 35);

            btnPrint.Text = "In thẻ (Print)";
            btnPrint.Bounds = new Rectangle(460, 410, 120, 35);

            btnClear.Text = "Làm mới (Clear)";
            btnClear.Bounds = new Rectangle(590, 410, 120, 35);

            btnSaveImage.Text = "Lưu ảnh (PNG)";
            btnSaveImage.Bounds = new Rectangle(720, 410, 120, 35);

            Controls.AddRange(new Control[] { grpInput, grpPreview, btnPreviewPrint, btnPrint, btnClear, btnSaveImage });
        }

        private void InitializeData()
        {
            cboClass.Items.AddRange(new object[] { "K49.CNTT.A", "K49.CNTT.B", "K50.CNTT.A" });
            cboFaculty.Items.AddRange(new object[] { "Công nghệ thông tin", "Toán - Tin học" });
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });

            cboClass.SelectedIndex = 0;
            cboFaculty.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;
        }

        private void RegisterEvents()
        {
            // Các sự kiện thay đổi dữ liệu gọi Invalidate để Panel tự vẽ lại
            txtStudentId.TextChanged += (s, e) => pnlCardPreview.Invalidate();
            txtFullName.TextChanged += (s, e) => pnlCardPreview.Invalidate();
            txtCourse.TextChanged += (s, e) => pnlCardPreview.Invalidate();
            cboClass.SelectedIndexChanged += (s, e) => pnlCardPreview.Invalidate();
            cboFaculty.SelectedIndexChanged += (s, e) => pnlCardPreview.Invalidate();
            dtpBirthDate.ValueChanged += (s, e) => pnlCardPreview.Invalidate();

            // Các nút chức năng
            btnChoosePhoto.Click += btnChoosePhoto_Click;
            btnPreviewPrint.Click += btnPreviewPrint_Click;
            btnPrint.Click += btnPrint_Click;
            btnClear.Click += btnClear_Click;
            btnSaveImage.Click += btnSaveImage_Click;

            // Sự kiện vẽ lên Panel và PrintDocument (Chỉ gắn 1 lần)
            pnlCardPreview.Paint += pnlCardPreview_Paint;
            printDocument1.PrintPage += printDocument1_PrintPage;
        }


        // ==============================================================
        // PHẦN 2: CHỌN ẢNH VÀ VALIDATION DỮ LIỆU
        // ==============================================================
        private void btnChoosePhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog { Title = "Chọn ảnh thẻ", Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Dùng Bitmap copy để tránh lỗi file gốc bị khóa (Lock file)
                        using (Image temp = Image.FromFile(dlg.FileName))
                        {
                            Image copy = new Bitmap(temp);
                            if (picAvatar.Image != null) picAvatar.Image.Dispose();
                            picAvatar.Image = copy;
                            pnlCardPreview.Invalidate();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("File không hợp lệ hoặc bị lỗi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private StudentCardInfo GetStudentInfo()
        {
            return new StudentCardInfo
            {
                StudentId = txtStudentId.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                ClassName = cboClass.Text,
                Faculty = cboFaculty.Text,
                Course = txtCourse.Text.Trim(),
                BirthDate = dtpBirthDate.Value,
                Gender = cboGender.Text,
                Photo = picAvatar.Image
            };
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text)) { MessageBox.Show("Chưa nhập Mã sinh viên!", "Cảnh báo"); return false; }
            if (string.IsNullOrWhiteSpace(txtFullName.Text)) { MessageBox.Show("Chưa nhập Họ tên!", "Cảnh báo"); return false; }
            if (picAvatar.Image == null) { MessageBox.Show("Chưa chọn Ảnh đại diện!", "Cảnh báo"); return false; }
            return true;
        }


        // ==============================================================
        // PHẦN 3: VẼ THẺ SINH VIÊN (DÙNG CHUNG CHO PANEL VÀ IN)
        // ==============================================================
        private void pnlCardPreview_Paint(object sender, PaintEventArgs e)
        {
            // Panel Preview sẽ vẽ thẻ kích thước 500x300 cách góc trái 25px
            Rectangle cardRect = new Rectangle(25, 20, 500, 300);
            DrawStudentCard(e.Graphics, cardRect, GetStudentInfo());
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Bản In thật sẽ vẽ thẻ theo Margin (Góc in) với cùng kích thước
            Rectangle cardRect = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, 500, 300);
            DrawStudentCard(e.Graphics, cardRect, GetStudentInfo());
            e.HasMorePages = false; // Chỉ in 1 trang
        }

        private void DrawStudentCard(Graphics g, Rectangle card, StudentCardInfo info)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Định nghĩa Font chữ và Cọ vẽ
            using (Font schoolFont = new Font("Arial", 11, FontStyle.Bold))
            using (Font titleFont = new Font("Arial", 16, FontStyle.Bold))
            using (Font labelFont = new Font("Arial", 10, FontStyle.Bold))
            using (Font normalFont = new Font("Arial", 10, FontStyle.Regular))
            using (Pen borderPen = new Pen(Color.DarkBlue, 2))
            using (SolidBrush blueBrush = new SolidBrush(Color.DarkBlue))
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(238, 247, 247)))
            {
                // 2. Vẽ Nền và Khung viền thẻ
                g.FillRectangle(bgBrush, card);
                g.DrawRectangle(borderPen, card);

                int x = card.Left;
                int y = card.Top;

                // 3. Vẽ Tiêu đề
                g.DrawString("TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH", schoolFont, blueBrush, x + 50, y + 15);
                g.DrawString("THẺ SINH VIÊN", titleFont, blueBrush, x + 160, y + 40);

                // 4. Vẽ Khung chứa Ảnh và Ảnh
                Rectangle photoBox = new Rectangle(x + 25, y + 90, 115, 150);
                g.FillRectangle(Brushes.White, photoBox);
                g.DrawRectangle(Pens.Gray, photoBox);

                if (info.Photo != null)
                {
                    DrawImageZoom(g, info.Photo, photoBox);
                }
                else
                {
                    g.DrawString("Ảnh rỗng", normalFont, Brushes.Gray, photoBox.Left + 25, photoBox.Top + 65);
                }

                // 5. Vẽ Thông tin sinh viên (Cột phải của thẻ)
                int infoX = x + 165;
                int infoY = y + 90;
                DrawInfoLine(g, "MSSV:", info.StudentId, labelFont, normalFont, infoX, infoY);
                DrawInfoLine(g, "Họ tên:", info.FullName, labelFont, normalFont, infoX, infoY + 30);
                DrawInfoLine(g, "Lớp:", info.ClassName, labelFont, normalFont, infoX, infoY + 60);
                DrawInfoLine(g, "Khoa:", info.Faculty, labelFont, normalFont, infoX, infoY + 90);
                DrawInfoLine(g, "Khóa:", info.Course, labelFont, normalFont, infoX, infoY + 120);
                DrawInfoLine(g, "Ngày sinh:", info.BirthDate.ToString("dd/MM/yyyy"), labelFont, normalFont, infoX, infoY + 150);
            }
        }

        private void DrawInfoLine(Graphics g, string label, string value, Font labelFont, Font valueFont, int x, int y)
        {
            g.DrawString(label, labelFont, Brushes.Black, x, y);
            g.DrawString(value, valueFont, Brushes.Black, x + 90, y);
        }

        // Hàm giúp ảnh giữ đúng tỷ lệ không bị méo khi vẽ
        private void DrawImageZoom(Graphics g, Image image, Rectangle box)
        {
            float ratio = Math.Min((float)box.Width / image.Width, (float)box.Height / image.Height);
            int width = (int)(image.Width * ratio);
            int height = (int)(image.Height * ratio);
            int x = box.Left + (box.Width - width) / 2;
            int y = box.Top + (box.Height - height) / 2;
            g.DrawImage(image, new Rectangle(x, y, width, height));
        }

        // ==============================================================
        // PHẦN 4: THAO TÁC IN ẤN VÀ LƯU ẢNH
        // ==============================================================
        private void btnPreviewPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Width = 900;
            printPreviewDialog1.Height = 700;
            printPreviewDialog1.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PNG Image|*.png", Title = "Lưu thẻ sinh viên" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Tạo một Bitmap kích thước 500x300
                    using (Bitmap bmp = new Bitmap(500, 300))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        DrawStudentCard(g, new Rectangle(0, 0, 500, 300), GetStudentInfo());
                        bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Đã lưu thẻ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentId.Clear();
            txtFullName.Clear();
            txtCourse.Clear();
            cboClass.SelectedIndex = 0;
            cboFaculty.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;
            dtpBirthDate.Value = DateTime.Today;

            if (picAvatar.Image != null) picAvatar.Image.Dispose();
            picAvatar.Image = null;

            pnlCardPreview.Invalidate();
            txtStudentId.Focus();
        }
    }
}