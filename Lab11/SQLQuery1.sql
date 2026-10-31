USE EduManagerDB;
GO
-- Thêm khoa CNTT nếu chưa tồn tại trong bảng KHOA
IF NOT EXISTS (SELECT 1 FROM KHOA WHERE MaKhoa = 'CNTT')
BEGIN
    INSERT INTO KHOA (MaKhoa, TenKhoa) VALUES ('CNTT', N'Công nghệ thông tin');
END
GO