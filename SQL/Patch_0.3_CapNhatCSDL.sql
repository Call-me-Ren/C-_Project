-- ===================================================================
-- SCRIPT CẬP NHẬT CSDL QLTHUVIEN (BƯỚC 0.3 TRONG PLAN.MD)
-- Dành cho trường hợp ĐÃ chạy QLThuVien1.sql cũ và cần cập nhật
-- mà KHÔNG CẦN XÓA DATABASE CŨ.
-- ===================================================================

USE QLThuVien;
GO

-- 1. Xóa 2 Trigger tự động để xử lý 100% tồn kho ở lớp BUS (tránh bị trừ/cộng 2 lần)
IF OBJECT_ID('trg_ChiTietPhieuMuon_Insert', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ChiTietPhieuMuon_Insert;
    PRINT N'- Đã xóa trigger trg_ChiTietPhieuMuon_Insert';
END
GO

IF OBJECT_ID('trg_ChiTietPhieuMuon_UpdateTraSach', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ChiTietPhieuMuon_UpdateTraSach;
    PRINT N'- Đã xóa trigger trg_ChiTietPhieuMuon_UpdateTraSach';
END
GO

-- 2. Tạo bảng thứ 17: Bảng ThamSo (Yêu cầu bắt buộc số 9 của Giáo viên)
IF OBJECT_ID('ThamSo', 'U') IS NULL
BEGIN
    CREATE TABLE ThamSo (
        MaTS VARCHAR(50) NOT NULL PRIMARY KEY,
        TenTS NVARCHAR(100) NOT NULL,
        GiaTri NVARCHAR(100) NOT NULL
    );

    INSERT INTO ThamSo (MaTS, TenTS, GiaTri) VALUES
    ('SO_SACH_MUON_TOI_DA', N'Số cuốn sách tối đa mỗi lần mượn', '5'),
    ('SO_NGAY_MUON_TOI_DA', N'Số ngày mượn tối đa mặc định', '7'),
    ('TIEN_PHAT_MOI_NGAY', N'Tiền phạt trễ hạn mỗi ngày (VNĐ)', '5000'),
    ('NAM_XB_TOI_THIEU', N'Năm xuất bản tối thiểu hợp lệ', '1900');

    PRINT N'- Đã tạo bảng ThamSo và chèn 4 tham số mặc định';
END
GO

-- 3. Chuyển cột ThanhTien trong ChiTietPhieuNhap thành cột thường để lớp BUS tự tính và lưu
IF EXISTS (SELECT 1 FROM sys.computed_columns WHERE name = 'ThanhTien' AND object_id = OBJECT_ID('ChiTietPhieuNhap'))
BEGIN
    ALTER TABLE ChiTietPhieuNhap DROP COLUMN ThanhTien;
    ALTER TABLE ChiTietPhieuNhap ADD ThanhTien DECIMAL(18, 2) NOT NULL DEFAULT 0;
    EXEC('UPDATE ChiTietPhieuNhap SET ThanhTien = DonGia * SoLuong;');
    PRINT N'- Đã chuyển cột ThanhTien trong ChiTietPhieuNhap thành DECIMAL(18,2) thường';
END
GO

PRINT N'==================================================';
PRINT N' CẬP NHẬT CSDL THÀNH CÔNG (CHUẨN 17 BẢNG)! ';
PRINT N'==================================================';
