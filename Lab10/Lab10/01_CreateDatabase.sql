-- Chuyển sang cơ sở dữ liệu master để tránh lỗi đang dùng database muốn xóa[cite: 21, 22]
USE master;
GO

-- Xóa database cũ nếu đã tồn tại để tránh lỗi trùng lặp khi chạy nhiều lần
DROP DATABASE IF EXISTS EduManagerDB;
GO

-- 1. Tạo database mới[cite: 20, 22]
CREATE DATABASE EduManagerDB;
GO

USE EduManagerDB;
GO

-- 2. Tạo bảng KHOA (Phải tạo đầu tiên vì LOP tham chiếu đến nó)[cite: 20]
CREATE TABLE KHOA (
    MaKhoa VARCHAR(20) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15) NULL
);

-- 3. Tạo bảng LOP[cite: 20]
CREATE TABLE LOP (
    MaLop VARCHAR(20) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    MaKhoa VARCHAR(20) NOT NULL,
    CONSTRAINT FK_LOP_KHOA 
        FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);

-- 4. Tạo bảng SINHVIEN[cite: 20]
CREATE TABLE SINHVIEN (
    MaSV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NOT NULL,
    MaLop VARCHAR(20) NOT NULL,
    -- Ràng buộc: Giới tính chỉ được là 'Nam' hoặc 'Nữ'[cite: 20]
    CONSTRAINT CK_SINHVIEN_GioiTinh 
        CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    CONSTRAINT FK_SINHVIEN_LOP 
        FOREIGN KEY (MaLop) REFERENCES LOP(MaLop)
);

-- 5. Tạo bảng MONHOC[cite: 20]
CREATE TABLE MONHOC (
    MaMH VARCHAR(20) PRIMARY KEY,
    TenMH NVARCHAR(100) NOT NULL,
    SoTinChi INT NOT NULL,
    -- Ràng buộc: Số tín chỉ phải > 0[cite: 20]
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);

-- 6. Tạo bảng KETQUA[cite: 20]
CREATE TABLE KETQUA (
    MaSV VARCHAR(20) NOT NULL,
    MaMH VARCHAR(20) NOT NULL,
    Diem DECIMAL(4,2) NOT NULL,
    -- Khóa chính ghép từ 2 cột để một sinh viên không thể có 2 điểm cùng 1 môn học
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMH),
    -- Ràng buộc: Điểm phải từ 0 đến 10[cite: 20]
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem BETWEEN 0 AND 10),
    CONSTRAINT FK_KETQUA_SINHVIEN 
        FOREIGN KEY (MaSV) REFERENCES SINHVIEN(MaSV),
    CONSTRAINT FK_KETQUA_MONHOC 
        FOREIGN KEY (MaMH) REFERENCES MONHOC(MaMH)
);

-- 7. Tạo bảng TAIKHOAN[cite: 20]
CREATE TABLE TAIKHOAN (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(100) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    VaiTro VARCHAR(20) NOT NULL DEFAULT 'User',
    TrangThai BIT NOT NULL DEFAULT 1
);