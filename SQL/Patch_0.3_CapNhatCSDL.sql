-- ===================================================================
-- BẢN VÁ TỔNG HỢP CẬP NHẬT CSDL QLTHUVIEN (ALL-IN-ONE PATCH)
-- Dành cho trường hợp ĐÃ chạy CSDL cũ trên SSMS và cần cập nhật
-- lên cấu trúc mới nhất mà KHÔNG CẦN XÓA DATABASE CŨ.
--
-- CÁC VẤN ĐỀ ĐƯỢC KHẮC PHỤC TRONG PATCH NÀY:
-- 1. Xóa 2 Trigger tự động tồn kho để BUS kiểm soát 100%.
-- 2. Tạo bảng thứ 17 ThamSo (Yêu cầu 9 của GV).
-- 3. Đổi ChiTietPhieuNhap.ThanhTien thành DECIMAL(18,2) thường.
-- 4. Bổ sung Sach.TrangThai BIT (0: Chưa mượn, 1: Đang mượn).
-- 5. Nâng cấp ChiTietPhieuPhat có MaCTPP INT IDENTITY làm PK (Issue #17).
-- ===================================================================

USE QLThuVien;
GO

PRINT N'>>> BẮT ĐẦU CẬP NHẬT CSDL LÊN BẢN CHUẨN MỚI NHẤT...';
GO

-- 1. Xóa 2 Trigger tự động tồn kho
IF OBJECT_ID('trg_ChiTietPhieuMuon_Insert', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ChiTietPhieuMuon_Insert;
    PRINT N'[1/5] Đã xóa trigger trg_ChiTietPhieuMuon_Insert';
END
ELSE
    PRINT N'[1/5] Trigger trg_ChiTietPhieuMuon_Insert không tồn tại (Đạt)';
GO

IF OBJECT_ID('trg_ChiTietPhieuMuon_UpdateTraSach', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ChiTietPhieuMuon_UpdateTraSach;
    PRINT N'[1/5] Đã xóa trigger trg_ChiTietPhieuMuon_UpdateTraSach';
END
ELSE
    PRINT N'[1/5] Trigger trg_ChiTietPhieuMuon_UpdateTraSach không tồn tại (Đạt)';
GO

-- 2. Tạo bảng thứ 17: ThamSo
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

    PRINT N'[2/5] Đã tạo bảng ThamSo và chèn 4 tham số cấu hình';
END
ELSE
    PRINT N'[2/5] Bảng ThamSo đã tồn tại (Đạt)';
GO

-- 3. Chuyển cột ThanhTien trong ChiTietPhieuNhap thành cột DECIMAL thường
IF EXISTS (SELECT 1 FROM sys.computed_columns WHERE name = 'ThanhTien' AND object_id = OBJECT_ID('ChiTietPhieuNhap'))
BEGIN
    ALTER TABLE ChiTietPhieuNhap DROP COLUMN ThanhTien;
    ALTER TABLE ChiTietPhieuNhap ADD ThanhTien DECIMAL(18, 2) NOT NULL DEFAULT 0;
    EXEC('UPDATE ChiTietPhieuNhap SET ThanhTien = DonGia * SoLuong;');
    PRINT N'[3/5] Đã chuyển ChiTietPhieuNhap.ThanhTien thành DECIMAL(18,2) thường';
END
ELSE IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = 'ThanhTien' AND object_id = OBJECT_ID('ChiTietPhieuNhap'))
BEGIN
    ALTER TABLE ChiTietPhieuNhap ADD ThanhTien DECIMAL(18, 2) NOT NULL DEFAULT 0;
    PRINT N'[3/5] Đã bổ sung cột ChiTietPhieuNhap.ThanhTien';
END
ELSE
    PRINT N'[3/5] Cột ChiTietPhieuNhap.ThanhTien đã chuẩn (Đạt)';
GO

-- 4. Bổ sung cột TrangThai vào bảng Sach (nếu thiếu)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = 'TrangThai' AND object_id = OBJECT_ID('Sach'))
BEGIN
    ALTER TABLE Sach ADD TrangThai BIT NOT NULL DEFAULT 0;
    -- Đồng bộ trạng thái theo phiếu mượn hiện hành
    EXEC('UPDATE Sach SET TrangThai = 1 WHERE MaSach IN (SELECT MaSach FROM ChiTietPhieuMuon WHERE NgayThucTra IS NULL);');
    PRINT N'[4/5] Đã thêm cột TrangThai BIT vào bảng Sach và đồng bộ trạng thái';
END
ELSE
    PRINT N'[4/5] Cột Sach.TrangThai đã tồn tại (Đạt)';
GO

-- 5. Nâng cấp ChiTietPhieuPhat có MaCTPP INT IDENTITY làm PK (Issue #17)
IF COL_LENGTH(N'dbo.ChiTietPhieuPhat', N'MaCTPP') IS NULL
BEGIN
    -- Xóa PK cũ nếu có
    DECLARE @PkName SYSNAME;
    SELECT @PkName = name FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat') AND type = N'PK';
    IF @PkName IS NOT NULL
    BEGIN
        EXEC(N'ALTER TABLE dbo.ChiTietPhieuPhat DROP CONSTRAINT ' + @PkName);
    END;

    -- Đảm bảo MaSach NOT NULL
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat') AND name = N'MaSach' AND is_nullable = 1)
        ALTER TABLE dbo.ChiTietPhieuPhat ALTER COLUMN MaSach VARCHAR(20) NOT NULL;

    -- Thêm cột IDENTITY MaCTPP làm PK
    ALTER TABLE dbo.ChiTietPhieuPhat ADD MaCTPP INT IDENTITY(1,1) NOT NULL;
    ALTER TABLE dbo.ChiTietPhieuPhat ADD CONSTRAINT PK_ChiTietPhieuPhat PRIMARY KEY (MaCTPP);

    -- Thêm Unique constraint (MaPhieuPhat, MaSach, MaQD)
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat') AND name = N'UQ_ChiTietPhieuPhat_Phieu_Sach_QuyDinh')
        ALTER TABLE dbo.ChiTietPhieuPhat ADD CONSTRAINT UQ_ChiTietPhieuPhat_Phieu_Sach_QuyDinh UNIQUE (MaPhieuPhat, MaSach, MaQD);

    PRINT N'[5/5] Đã nâng cấp ChiTietPhieuPhat với MaCTPP IDENTITY làm PK';
END
ELSE
    PRINT N'[5/5] Cột ChiTietPhieuPhat.MaCTPP đã chuẩn (Đạt)';
GO

PRINT N'==================================================';
PRINT N' CẬP NHẬT CSDL THÀNH CÔNG (100% ĐỒNG BỘ MÃ NGUỒN)! ';
PRINT N'==================================================';
