using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab04_QuanLySanPham
{
    // 1. Interface IEntity dùng làm ràng buộc Generic
    public interface IEntity
    {
        string Id { get; }
    }

    // 2. Các Exception tự tạo (Custom Exceptions)
    public class DuplicateProductException : Exception
    {
        public DuplicateProductException(string message) : base(message) { }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    // 3. Class Product kế thừa IEntity
    public class Product : IEntity
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }

        private decimal price;
        public decimal Price
        {
            get => price;
            // Kiểm tra nghiệp vụ: Giá không được âm
            set { if (value < 0) throw new Exception("Đơn giá không được âm!"); price = value; }
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            // Kiểm tra nghiệp vụ: Số lượng không được âm[cite: 22]
            set { if (value < 0) throw new Exception("Số lượng không được âm!"); quantity = value; }
        }

        public string Id => MaSP; // Triển khai thuộc tính Id của IEntity[cite: 22]

        public Product(string maSP, string tenSP, decimal price, int quantity)
        {
            MaSP = maSP;
            TenSP = tenSP;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Mã SP: {MaSP,-5} | Tên: {TenSP,-15} | Đơn giá: {Price,-10} | Số lượng: {Quantity}";
        }
    }

    // 4. Lớp Generic Repository<T> để quản lý dữ liệu[cite: 22]
    public class Repository<T> where T : IEntity
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            if (items.Any(x => x.Id == item.Id))
                throw new DuplicateProductException($"Lỗi: Mã '{item.Id}' đã tồn tại!"); // Ném lỗi Duplicate[cite: 22]
            items.Add(item);
        }

        public void Remove(string id)
        {
            var item = FindById(id);
            if (item == null)
                throw new ProductNotFoundException($"Lỗi: Không tìm thấy sản phẩm mã '{id}'!"); // Ném lỗi Not Found[cite: 22]
            items.Remove(item);
        }

        public T FindById(string id) => items.FirstOrDefault(x => x.Id == id);

        // Sử dụng Func<T, bool> để lọc và tìm kiếm chung[cite: 22, 23]
        public List<T> Find(Func<T, bool> predicate) => items.Where(predicate).ToList();

        public List<T> GetAll() => items;
    }

    // 5. Lớp ProductService giao tiếp giữa Menu và Repository[cite: 22]
    public class ProductService
    {
        private Repository<Product> repo = new Repository<Product>();

        // Khai báo Action / Event để thông báo khi dữ liệu thay đổi[cite: 22]
        public event Action<string> OnProductChanged;

        public void AddProduct(Product p)
        {
            repo.Add(p);
            OnProductChanged?.Invoke($"Đã THÊM thành công sản phẩm: {p.TenSP}"); // Phát event[cite: 22]
        }

        public void RemoveProduct(string id)
        {
            repo.Remove(id);
            OnProductChanged?.Invoke($"Đã XÓA thành công sản phẩm mã: {id}"); // Phát event[cite: 22]
        }

        public List<Product> GetAll() => repo.GetAll();
        public Product SearchById(string id) => repo.FindById(id);
        public List<Product> SearchByName(string name) => repo.Find(p => p.TenSP.ToLower().Contains(name.ToLower()));

        // Dùng Func để lọc theo khoảng giá
        public List<Product> FilterByPrice(decimal min, decimal max) => repo.Find(p => p.Price >= min && p.Price <= max);
    }

    // 6. Hàm Main và giao diện Console[cite: 22]
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ProductService service = new ProductService();

            // Đăng ký sự kiện (Lắng nghe event OnProductChanged)[cite: 22]
            service.OnProductChanged += (message) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[THÔNG BÁO EVENT] {message}");
                Console.ResetColor();
            };

            int chon;
            do
            {
                Console.WriteLine("\n===== PRODUCT MANAGER =====");
                Console.WriteLine("1. Them san pham");
                Console.WriteLine("2. Xuat danh sach");
                Console.WriteLine("3. Tim theo ma");
                Console.WriteLine("4. Tim theo ten");
                Console.WriteLine("5. Loc theo khoang gia");
                Console.WriteLine("6. Xoa san pham");
                Console.WriteLine("7. Tinh tong gia tri kho");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon: ");

                if (!int.TryParse(Console.ReadLine(), out chon)) continue;

                try
                {
                    switch (chon)
                    {
                        case 1:
                            Console.Write("Nhập mã: "); string ma = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(ma)) throw new Exception("Mã không được rỗng!");
                            Console.Write("Nhập tên: "); string ten = Console.ReadLine();
                            Console.Write("Nhập đơn giá: "); decimal gia = decimal.Parse(Console.ReadLine());
                            Console.Write("Nhập số lượng: "); int sl = int.Parse(Console.ReadLine());

                            service.AddProduct(new Product(ma, ten, gia, sl));
                            break;
                        case 2:
                            var ds = service.GetAll();
                            if (ds.Count == 0) Console.WriteLine("Danh sách rỗng!");
                            else ds.ForEach(p => Console.WriteLine(p.ToString()));
                            break;
                        case 3:
                            Console.Write("Nhập mã cần tìm: ");
                            var pMa = service.SearchById(Console.ReadLine());
                            if (pMa != null) Console.WriteLine(pMa.ToString());
                            else Console.WriteLine("Không tìm thấy!");
                            break;
                        case 4:
                            Console.Write("Nhập tên cần tìm: ");
                            var dsTen = service.SearchByName(Console.ReadLine());
                            if (dsTen.Count == 0) Console.WriteLine("Không tìm thấy!");
                            else dsTen.ForEach(p => Console.WriteLine(p.ToString()));
                            break;
                        case 5:
                            Console.Write("Nhập giá MIN: "); decimal min = decimal.Parse(Console.ReadLine());
                            Console.Write("Nhập giá MAX: "); decimal max = decimal.Parse(Console.ReadLine());
                            var dsLoc = service.FilterByPrice(min, max);
                            if (dsLoc.Count == 0) Console.WriteLine("Không có sản phẩm trong khoảng giá này!");
                            else dsLoc.ForEach(p => Console.WriteLine(p.ToString()));
                            break;
                        case 6:
                            Console.Write("Nhập mã cần xóa: ");
                            service.RemoveProduct(Console.ReadLine());
                            break;
                        case 7:
                            decimal tongK = service.GetAll().Sum(p => p.Price * p.Quantity);
                            Console.WriteLine($"Tổng giá trị kho: {tongK:N0}");
                            break;
                        case 0: Console.WriteLine("Thoát chương trình."); break;
                        default: Console.WriteLine("Chọn sai!"); break;
                    }
                }
                // Khối try-catch xử lý exception cụ thể[cite: 22, 23]
                catch (DuplicateProductException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                catch (ProductNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Lỗi nhập liệu: {ex.Message}");
                    Console.ResetColor();
                }

            } while (chon != 0);
        }
    }
}