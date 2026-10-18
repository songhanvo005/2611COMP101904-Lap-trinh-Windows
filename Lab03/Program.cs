using System;
using System.Collections.Generic;
using System.Linq; 

namespace Lab03_QuanLySinhVienOOP
{
    public class Nguoi
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }

        public Nguoi(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        public virtual string LayThongTin()
        {
            return $"Họ tên: {HoTen} | Ngày sinh: {NgaySinh:dd/MM/yyyy}";
        }
    }

    public class SinhVien : Nguoi
    {
        public string MaSinhVien { get; set; }
        public string MaLop { get; set; }

        private double diemTrungBinh;
        public double DiemTrungBinh
        {
            get { return diemTrungBinh; }
            set
            {
                if (value < 0 || value > 10)
                    throw new Exception("Điểm trung bình phải từ 0 đến 10!");
                diemTrungBinh = value;
            }
        }

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, string maLop, double diemTB)
            : base(hoTen, ngaySinh)
        {
            MaSinhVien = maSV;
            MaLop = maLop;
            DiemTrungBinh = diemTB;
        }

        public string XepLoai()
        {
            if (DiemTrungBinh >= 8.0) return "Giỏi";
            if (DiemTrungBinh >= 6.5) return "Khá";
            if (DiemTrungBinh >= 5.0) return "Trung bình";
            return "Yếu";
        }

        public override string LayThongTin()
        {
            return $"Mã SV: {MaSinhVien} | {base.LayThongTin()} | Lớp: {MaLop} | Điểm: {DiemTrungBinh} | Xếp loại: {XepLoai()}";
        }
    }

    public class QuanLySinhVien
    {
        private List<SinhVien> danhSachSV = new List<SinhVien>();

        public void ThemSinhVien(SinhVien sv)
        {
            if (danhSachSV.Any(s => s.MaSinhVien == sv.MaSinhVien))
            {
                Console.WriteLine("Lỗi: Mã sinh viên đã tồn tại!");
                return;
            }
            danhSachSV.Add(sv);
            Console.WriteLine("Thêm sinh viên thành công!");
        }

        public void XuatDanhSach()
        {
            if (danhSachSV.Count == 0)
            {
                Console.WriteLine("Danh sách trống!");
                return;
            }
            foreach (var sv in danhSachSV)
            {
                Console.WriteLine(sv.LayThongTin());
            }
        }

        public void TimTheoMa(string maSV)
        {
            var sv = danhSachSV.FirstOrDefault(s => s.MaSinhVien == maSV);
            if (sv != null) Console.WriteLine(sv.LayThongTin());
            else Console.WriteLine("Không tìm thấy sinh viên!");
        }

        public void TimTheoTen(string tuKhoa)
        {
            var ketQua = danhSachSV.Where(s => s.HoTen.ToLower().Contains(tuKhoa.ToLower())).ToList();
            if (ketQua.Count > 0)
            {
                foreach (var sv in ketQua) Console.WriteLine(sv.LayThongTin());
            }
            else Console.WriteLine("Không tìm thấy sinh viên nào chứa từ khóa này!");
        }

        public void SuaDiem(string maSV, double diemMoi)
        {
            var sv = danhSachSV.FirstOrDefault(s => s.MaSinhVien == maSV);
            if (sv != null)
            {
                sv.DiemTrungBinh = diemMoi;
                Console.WriteLine("Cập nhật điểm thành công!");
            }
            else Console.WriteLine("Không tìm thấy mã sinh viên này!");
        }

        public void XoaSinhVien(string maSV)
        {
            var sv = danhSachSV.FirstOrDefault(s => s.MaSinhVien == maSV);
            if (sv != null)
            {
                danhSachSV.Remove(sv);
                Console.WriteLine("Xóa sinh viên thành công!");
            }
            else Console.WriteLine("Không tìm thấy sinh viên để xóa!");
        }

        public void SapXepTheoDiemGiamDan()
        {
            var ketQua = danhSachSV.OrderByDescending(s => s.DiemTrungBinh).ToList();
            foreach (var sv in ketQua) Console.WriteLine(sv.LayThongTin());
        }

        public void LocSinhVienDat()
        {
            var ketQua = danhSachSV.Where(s => s.DiemTrungBinh >= 5.0).ToList();
            if (ketQua.Count > 0)
            {
                foreach (var sv in ketQua) Console.WriteLine(sv.LayThongTin());
            }
            else Console.WriteLine("Không có sinh viên nào đạt!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            QuanLySinhVien qlsv = new QuanLySinhVien();
            int luaChon;

            do
            {
                Console.WriteLine("\n===== QUAN LY SINH VIEN =====");
                Console.WriteLine("1. Them sinh vien");
                Console.WriteLine("2. Xuat danh sach");
                Console.WriteLine("3. Tim sinh vien theo ma");
                Console.WriteLine("4. Tim sinh vien theo ten");
                Console.WriteLine("5. Sua diem trung binh");
                Console.WriteLine("6. Xoa sinh vien");
                Console.WriteLine("7. Sap xep theo diem giam dan");
                Console.WriteLine("8. Loc sinh vien dat");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                if (!int.TryParse(Console.ReadLine(), out luaChon)) continue;

                try
                {
                    switch (luaChon)
                    {
                        case 1:
                            Console.Write("Nhập mã SV: "); string ma = Console.ReadLine();
                            Console.Write("Nhập họ tên: "); string ten = Console.ReadLine();
                            Console.Write("Nhập ngày sinh (yyyy-mm-dd): "); DateTime ngay = DateTime.Parse(Console.ReadLine());
                            Console.Write("Nhập lớp: "); string lop = Console.ReadLine();
                            Console.Write("Nhập điểm TB: "); double diem = double.Parse(Console.ReadLine());

                            SinhVien sv = new SinhVien(ma, ten, ngay, lop, diem);
                            qlsv.ThemSinhVien(sv);
                            break;
                        case 2: qlsv.XuatDanhSach(); break;
                        case 3:
                            Console.Write("Nhập mã SV cần tìm: ");
                            qlsv.TimTheoMa(Console.ReadLine());
                            break;
                        case 4:
                            Console.Write("Nhập tên cần tìm: ");
                            qlsv.TimTheoTen(Console.ReadLine());
                            break;
                        case 5:
                            Console.Write("Nhập mã SV cần sửa điểm: "); string maSua = Console.ReadLine();
                            Console.Write("Nhập điểm mới: "); double diemMoi = double.Parse(Console.ReadLine());
                            qlsv.SuaDiem(maSua, diemMoi);
                            break;
                        case 6:
                            Console.Write("Nhập mã SV cần xóa: ");
                            qlsv.XoaSinhVien(Console.ReadLine());
                            break;
                        case 7: qlsv.SapXepTheoDiemGiamDan(); break;
                        case 8: qlsv.LocSinhVienDat(); break;
                        case 0: Console.WriteLine("Thoát chương trình."); break;
                        default: Console.WriteLine("Lựa chọn không hợp lệ!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi nhập liệu: {ex.Message}");
                }

            } while (luaChon != 0);
        }
    }
}