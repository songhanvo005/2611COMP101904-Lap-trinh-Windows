using System;

namespace Lab02
{
    class Program
    {
        static int NhapSoNguyen(string message)
        {
            int so;
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out so))
                {
                    return so;
                }
                Console.WriteLine("Lỗi: Vui lòng nhập một số nguyên hợp lệ!");
            }
        }

        static int NhapSoNguyenDuong(string message)
        {
            int so;
            while (true)
            {
                so = NhapSoNguyen(message);
                if (so > 0)
                {
                    return so;
                }
                Console.WriteLine("Lỗi: Số lượng phải là số nguyên dương (> 0)!");
            }
        }


        static int[] NhapMang()
        {
            int n = NhapSoNguyenDuong("Nhập số lượng phần tử n = ");
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = NhapSoNguyen($"Nhập phần tử a[{i}] = ");
            }
            return a;
        }

        static void XuatMang(int[] a)
        {
            Console.Write("Các phần tử trong mảng: ");
            foreach (int item in a)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }

        static int TinhTong(int[] a)
        {
            int tong = 0;
            foreach (int item in a)
            {
                tong += item;
            }
            return tong;
        }

        static int TimMax(int[] a)
        {
            int max = a[0];
            foreach (int item in a)
            {
                if (item > max) max = item;
            }
            return max;
        }

        static int TimMin(int[] a)
        {
            int min = a[0];
            foreach (int item in a)
            {
                if (item < min) min = item;
            }
            return min;
        }

        static int DemChan(int[] a)
        {
            int dem = 0;
            foreach (int item in a)
            {
                if (item % 2 == 0) dem++;
            }
            return dem;
        }

        static int DemLe(int[] a)
        {
            int dem = 0;
            foreach (int item in a)
            {
                if (item % 2 != 0) dem++;
            }
            return dem;
        }

        static void SapXepTangDan(int[] a)
        {
            Array.Sort(a);
            Console.WriteLine("Đã sắp xếp mảng tăng dần.");
            XuatMang(a);
        }

        static void TimKiem(int[] a, int x)
        {
            int viTri = Array.IndexOf(a, x);
            if (viTri != -1)
            {
                Console.WriteLine($"Tìm thấy {x} tại vị trí đầu tiên là {viTri} (tính từ 0).");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy {x} trong mảng.");
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int[] mang = null;
            int luaChon;

            do
            {
                Console.WriteLine("\n===== MENU =====");
                Console.WriteLine("1. Nhap mang");
                Console.WriteLine("2. Xuat mang");
                Console.WriteLine("3. Tinh tong");
                Console.WriteLine("4. Tim max/min");
                Console.WriteLine("5. Dem chan/le");
                Console.WriteLine("6. Sap xep tang dan");
                Console.WriteLine("7. Tim kiem");
                Console.WriteLine("0. Thoat");

                luaChon = NhapSoNguyen("Chon chuc nang: ");


                if (luaChon >= 2 && luaChon <= 7 && mang == null)
                {
                    Console.WriteLine("Lỗi: Bạn chưa nhập mảng! Vui lòng chọn chức năng 1 trước.");
                    continue;
                }

                switch (luaChon)
                {
                    case 1:
                        mang = NhapMang();
                        break;
                    case 2:
                        XuatMang(mang);
                        break;
                    case 3:
                        Console.WriteLine($"Tổng các phần tử trong mảng: {TinhTong(mang)}");
                        break;
                    case 4:
                        Console.WriteLine($"Max = {TimMax(mang)}, Min = {TimMin(mang)}");
                        break;
                    case 5:
                        Console.WriteLine($"Số chẵn = {DemChan(mang)}, Số lẻ = {DemLe(mang)}");
                        break;
                    case 6:
                        SapXepTangDan(mang);
                        break;
                    case 7:
                        int x = NhapSoNguyen("Nhập giá trị cần tìm x = ");
                        TimKiem(mang, x);
                        break;
                    case 0:
                        Console.WriteLine("Đã thoát chương trình.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ, vui lòng chọn lại!");
                        break;
                }
            } while (luaChon != 0);
        }
    }
}