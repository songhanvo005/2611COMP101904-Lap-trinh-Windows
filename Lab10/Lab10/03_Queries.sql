USE EduManagerDB;
GO

-- YC1. Liệt kê toàn bộ sinh viên[cite: 21]
SELECT MaSV, HoTen, NgaySinh, GioiTinh, MaLop
FROM SINHVIEN;

-- YC2. Liệt kê sinh viên kèm tên lớp và tên khoa (Dùng INNER JOIN 3 bảng)[cite: 19, 21]
SELECT sv.MaSV, sv.HoTen, l.TenLop, k.TenKhoa
FROM SINHVIEN sv
INNER JOIN LOP l ON sv.MaLop = l.MaLop
INNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa;

-- YC3. Tìm sinh viên theo một phần họ tên bằng LIKE (Tìm chữ "An")[cite: 19, 21]
SELECT MaSV, HoTen, MaLop
FROM SINHVIEN
WHERE HoTen LIKE N'%An%'
ORDER BY HoTen ASC;

-- YC4. Liệt kê sinh viên theo mã lớp (Ví dụ: CNTT01)[cite: 19, 21]
SELECT MaSV, HoTen, GioiTinh
FROM SINHVIEN
WHERE MaLop = 'CNTT01';

-- YC5. Cập nhật họ tên của một sinh viên[cite: 21]
UPDATE SINHVIEN
SET HoTen = N'Nguyễn Văn An (Đã cập nhật)'
WHERE MaSV = 'SV001';

-- YC6. Xóa một sinh viên chưa có dữ liệu điểm[cite: 21]
-- (Dùng NOT EXISTS để đảm bảo SV này không có trong bảng KETQUA)[cite: 19]
DELETE FROM SINHVIEN
WHERE MaSV = 'SV003' 
  AND NOT EXISTS (SELECT 1 FROM KETQUA WHERE MaSV = 'SV003');

-- YC7. Liệt kê điểm của từng sinh viên theo từng môn (INNER JOIN 3 bảng)[cite: 19, 21]
SELECT sv.MaSV, sv.HoTen, mh.TenMH, kq.Diem
FROM KETQUA kq
INNER JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV
INNER JOIN MONHOC mh ON kq.MaMH = mh.MaMH
ORDER BY sv.MaSV, mh.TenMH;

-- YC8. Tính điểm trung bình của từng sinh viên (Dùng hàm AVG và GROUP BY)[cite: 19, 21]
SELECT sv.MaSV, sv.HoTen, AVG(kq.Diem) AS DiemTrungBinh
FROM SINHVIEN sv
INNER JOIN KETQUA kq ON sv.MaSV = kq.MaSV
GROUP BY sv.MaSV, sv.HoTen;

-- YC9. Thống kê số sinh viên theo từng lớp (Dùng COUNT và LEFT JOIN để tính cả lớp chưa có SV)[cite: 19, 21]
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop;

-- YC10. Liệt kê các lớp có từ 2 sinh viên trở lên (Thêm điều kiện HAVING)[cite: 19, 21]
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
HAVING COUNT(sv.MaSV) >= 2;


-- =========================================================================
-- KIỂM THỬ RÀNG BUỘC LỖI (Chú ý: Các câu dưới đây CỐ TÌNH sinh ra báo lỗi đỏ)
-- =========================================================================

-- YC11. Test lỗi khóa ngoại: Lớp 'CNTT99' không tồn tại trong bảng LOP[cite: 19, 21]
-- -> SQL Server sẽ báo lỗi liên quan đến FOREIGN KEY.
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)
VALUES ('SV999', N'Sinh viên lỗi', '2005-01-01', N'Nam', 'CNTT99');

-- YC12. Test lỗi điều kiện CHECK: Điểm lớn hơn 10[cite: 19, 21]
-- -> SQL Server sẽ báo lỗi liên quan đến CHECK constraint 'CK_KETQUA_Diem'.
INSERT INTO KETQUA (MaSV, MaMH, Diem)
VALUES ('SV001', 'COMP1019', 11.5);