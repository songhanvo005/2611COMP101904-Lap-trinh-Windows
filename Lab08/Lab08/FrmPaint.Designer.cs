using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lab08
{
    public partial class FrmPaint : Form
    {
        private readonly DrawSurface canvas = new DrawSurface();
        private readonly ComboBox cboMode = new ComboBox();
        private readonly NumericUpDown nudWidth = new NumericUpDown();
        private readonly Button btnColor = new Button();
        private readonly Button btnUndo = new Button();
        private readonly Button btnClear = new Button();
        private readonly Button btnSave = new Button();
        private readonly Label lblStatus = new Label();

        private readonly List<DrawItem> items = new List<DrawItem>();
        private DrawItem currentItem;
        private bool isDrawing = false;
        private Color currentColor = Color.Black;

     

        private void BuildUserInterface()
        {
            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(240, 240, 240) };

            Label lblMode = new Label { Text = "Chế độ:", Left = 15, Top = 16, AutoSize = true };
            cboMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMode.Items.AddRange(new object[] { "Pen", "Line", "Rectangle", "Ellipse" });
            cboMode.SelectedIndex = 0;
            cboMode.SetBounds(70, 12, 100, 25);

            Label lblWidth = new Label { Text = "Độ dày:", Left = 190, Top = 16, AutoSize = true };
            nudWidth.Minimum = 1; nudWidth.Maximum = 20; nudWidth.Value = 3;
            nudWidth.SetBounds(250, 12, 50, 25);

            btnColor.Text = "Chọn màu"; btnColor.SetBounds(320, 10, 80, 28);
            btnColor.BackColor = currentColor; btnColor.ForeColor = Color.White;

            btnUndo.Text = "Undo (Ctrl+Z)"; btnUndo.SetBounds(410, 10, 100, 28);
            btnClear.Text = "Clear All"; btnClear.SetBounds(520, 10, 80, 28);
            btnSave.Text = "Lưu Ảnh PNG"; btnSave.SetBounds(610, 10, 100, 28);

            topPanel.Controls.AddRange(new Control[] { lblMode, cboMode, lblWidth, nudWidth, btnColor, btnUndo, btnClear, btnSave });

            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 25;
            lblStatus.Text = " Sẵn sàng";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            Controls.Add(canvas);
            Controls.Add(lblStatus);
            Controls.Add(topPanel);
        }

        private void RegisterEvents()
        {
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;

            btnColor.Click += BtnColor_Click;
            btnUndo.Click += BtnUndo_Click;
            btnClear.Click += BtnClear_Click;
            btnSave.Click += BtnSave_Click;
            KeyDown += FrmPaint_KeyDown;
        }

        private DrawMode CurrentMode => (DrawMode)Enum.Parse(typeof(DrawMode), cboMode.SelectedItem.ToString());

        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog { Color = currentColor })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentColor = dlg.Color;
                    btnColor.BackColor = currentColor;
                    btnColor.ForeColor = (currentColor.R * 0.299 + currentColor.G * 0.587 + currentColor.B * 0.114) > 180 ? Color.Black : Color.White;
                }
            }
        }

        private void PerformUndo()
        {
            if (items.Count > 0)
            {
                items.RemoveAt(items.Count - 1);
                canvas.Invalidate();
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e) => PerformUndo();
        private void FrmPaint_KeyDown(object sender, KeyEventArgs e) { if (e.Control && e.KeyCode == Keys.Z) { PerformUndo(); e.SuppressKeyPress = true; } }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (items.Count > 0 && MessageBox.Show("Xóa toàn bộ vùng vẽ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                items.Clear();
                canvas.Invalidate();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PNG Image|*.png" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bmp = new Bitmap(canvas.Width, canvas.Height))
                    {
                        canvas.DrawToBitmap(bmp, new Rectangle(0, 0, canvas.Width, canvas.Height));
                        bmp.Save(sfd.FileName);
                    }
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isDrawing = true;
            currentItem = new DrawItem { Mode = CurrentMode, Start = e.Location, End = e.Location, Color = currentColor, Width = (float)nudWidth.Value };
            if (currentItem.Mode == DrawMode.Pen) currentItem.Points.Add(e.Location);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            lblStatus.Text = $" Tọa độ chuột: X = {e.X}, Y = {e.Y} | Items: {items.Count}";
            if (!isDrawing || currentItem == null) return;
            currentItem.End = e.Location;
            if (currentItem.Mode == DrawMode.Pen) currentItem.Points.Add(e.Location);
            canvas.Invalidate();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentItem == null) return;
            currentItem.End = e.Location;
            items.Add(currentItem);
            currentItem = null;
            isDrawing = false;
            canvas.Invalidate();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var item in items) DrawOneItem(e.Graphics, item);
            if (currentItem != null) DrawOneItem(e.Graphics, currentItem);
        }

        private void DrawOneItem(Graphics g, DrawItem item)
        {
            using (Pen pen = new Pen(item.Color, item.Width) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                Rectangle rect = GetRectangle(item.Start, item.End);
                switch (item.Mode)
                {
                    case DrawMode.Pen: if (item.Points.Count > 1) g.DrawLines(pen, item.Points.ToArray()); break;
                    case DrawMode.Line: g.DrawLine(pen, item.Start, item.End); break;
                    case DrawMode.Rectangle: g.DrawRectangle(pen, rect); break;
                    case DrawMode.Ellipse: g.DrawEllipse(pen, rect); break;
                }
            }
        }

        private Rectangle GetRectangle(Point start, Point end)
        {
            return new Rectangle(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        }
    }
}