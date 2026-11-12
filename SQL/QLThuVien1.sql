-- ===================================================================
-- CƠ SỞ DỮ LIỆU: QLTHUVIEN (PHIÊN BẢN CHUẨN DUY NHẤT - 17 BẢNG)
-- ĐỒ ÁN MÔN HỌC: QUẢN LÝ THƯ VIỆN - C# WINFORMS (.NET 10) & BLAZOR
-- GVHD: Cô Phùng Thái Thiên Trang (Đại học Sài Gòn)
--
-- TỔNG HỢP TOÀN BỘ CÁC BẢN SỬA LỖI & QUY TẮC CỦA GIÁO VIÊN:
-- 1. Chuẩn hóa đầy đủ 17 bảng (bổ sung bảng thứ 17 ThamSo - Yêu cầu 9).
-- 2. Đầy đủ cột Sach.TrangThai (0: Chưa mượn, 1: Đang mượn) & Sach.TinhTrang.
-- 3. ChiTietPhieuPhat có MaCTPP INT IDENTITY làm PK, UQ(MaPhieuPhat, MaSach, MaQD).
-- 4. ChiTietPhieuNhap.ThanhTien là kiểu DECIMAL thường để tầng BUS tự tính toán.
-- 5. Không dùng Trigger tồn kho (100% nghiệp vụ cộng/trừ tồn do BUS quản lý).
-- 6. Dữ liệu mẫu đồng bộ 100% giữa Tồn kho DauSach, Cuốn sách và Phiếu mượn.
-- ===================================================================

-- 1. KHỞI TẠO CSDL AN TOÀN (Không báo lỗi nếu DB đã tồn tại)
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'QLThuVien')
BEGIN
    CREATE DATABASE QLThuVien;
    PRINT N'- Đã tạo mới CSDL QLThuVien';
END
GO

USE QLThuVien;
GO

-------------------------------------------------------------------
-- 2. TẠO CÁC BẢNG DANH MỤC NỀN & THỰC THỂ ĐỘC LẬP
-------------------------------------------------------------------

-- Bảng Nhà Xuất Bản
IF OBJECT_ID('NhaXuatBan', 'U') IS NULL
CREATE TABLE NhaXuatBan (
    MaNXB VARCHAR(20) NOT NULL PRIMARY KEY,
    TenNXB NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200) NULL
);

-- Bảng Tác Giả
IF OBJECT_ID('TacGia', 'U') IS NULL
CREATE TABLE TacGia (
    MaTG VARCHAR(20) NOT NULL PRIMARY KEY,
    HoLot NVARCHAR(50) NULL,
    TenTG NVARCHAR(50) NOT NULL,
    NgaySinh DATE NULL,
    SDT VARCHAR(15) NULL
);

-- Bảng Thể Loại
IF OBJECT_ID('TheLoai', 'U') IS NULL
CREATE TABLE TheLoai (
    MaTL VARCHAR(20) NOT NULL PRIMARY KEY,
    TenTL NVARCHAR(100) NOT NULL
);

-- Bảng Nhà Cung Cấp
IF OBJECT_ID('NhaCungCap', 'U') IS NULL
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) NOT NULL PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL
);

-- Bảng Nhân Viên
IF OBJECT_ID('NhanVien', 'U') IS NULL
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) NOT NULL PRIMARY KEY,
    HoLot NVARCHAR(50) NULL,
    TenNV NVARCHAR(50) NOT NULL,
    ChucVu NVARCHAR(50) NULL
);

-- Bảng Loại Độc Giả
IF OBJECT_ID('LoaiDocGia', 'U') IS NULL
CREATE TABLE LoaiDocGia (
    MaLDG VARCHAR(20) NOT NULL PRIMARY KEY,
    TenLDG NVARCHAR(100) NOT NULL
);

-- Bảng Độc Giả
IF OBJECT_ID('DocGia', 'U') IS NULL
CREATE TABLE DocGia (
    MaDG VARCHAR(20) NOT NULL PRIMARY KEY,
    HoLot NVARCHAR(50) NULL,
    TenDG NVARCHAR(50) NOT NULL,
    NgaySinh DATE NULL,
    SDT VARCHAR(15) NULL,
    MaLDG VARCHAR(20) NOT NULL,
    CONSTRAINT FK_DocGia_LoaiDocGia FOREIGN KEY (MaLDG) REFERENCES LoaiDocGia(MaLDG)
);

-- Bảng Quy Định Phạt
IF OBJECT_ID('QuyDinhPhat', 'U') IS NULL
CREATE TABLE QuyDinhPhat (
    MaQD VARCHAR(20) NOT NULL PRIMARY KEY,
    TenQD NVARCHAR(100) NOT NULL,
    LoaiPhat NVARCHAR(50) NULL,
    CachTinh NVARCHAR(100) NULL,
    DonViTinh NVARCHAR(20) NULL,
    MucPhat DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (MucPhat >= 0)
);

-- Bảng Tham Số (Bảng thứ 17 - Yêu cầu bắt buộc số 9 của Giáo viên: 2 cột Mã - Giá trị)
IF OBJECT_ID('ThamSo', 'U') IS NULL
CREATE TABLE ThamSo (
    MaTS VARCHAR(50) NOT NULL PRIMARY KEY,
    TenTS NVARCHAR(100) NOT NULL,
    GiaTri NVARCHAR(100) NOT NULL
);

-------------------------------------------------------------------
-- 3. TẠO BẢNG ĐẦU SÁCH VÀ CUỐN SÁCH
-------------------------------------------------------------------

-- Bảng Đầu Sách
IF OBJECT_ID('DauSach', 'U') IS NULL
CREATE TABLE DauSach (
    MaDauSach VARCHAR(20) NOT NULL PRIMARY KEY,
    MaNXB VARCHAR(20) NOT NULL,
    MaTG VARCHAR(20) NOT NULL,
    MaTL VARCHAR(20) NOT NULL,
    TenSach NVARCHAR(200) NOT NULL,
    NamXB INT NULL CHECK (NamXB <= YEAR(GETDATE())),
    SoLuong INT NOT NULL DEFAULT 0 CHECK (SoLuong >= 0),
    SLChoMuon INT NOT NULL DEFAULT 0 CHECK (SLChoMuon >= 0),
    SLConLai INT NOT NULL DEFAULT 0 CHECK (SLConLai >= 0),
    DonGia DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (DonGia >= 0),
    CONSTRAINT FK_DauSach_NhaXuatBan FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan(MaNXB),
    CONSTRAINT FK_DauSach_TacGia FOREIGN KEY (MaTG) REFERENCES TacGia(MaTG),
    CONSTRAINT FK_DauSach_TheLoai FOREIGN KEY (MaTL) REFERENCES TheLoai(MaTL)
);

-- Bảng Cuốn Sách (Sách vật lý)
-- Đầy đủ: TrangThai BIT (0: Chưa cho mượn, 1: Đã cho mượn), TinhTrang lưu chất lượng vật lý (Mới/Tốt/Cũ)
IF OBJECT_ID('Sach', 'U') IS NULL
CREATE TABLE Sach (
    MaSach VARCHAR(20) NOT NULL PRIMARY KEY,
    MaDauSach VARCHAR(20) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 0, -- 0: Chưa cho mượn, 1: Đã cho mượn
    TinhTrang NVARCHAR(50) NULL DEFAULT N'Mới', -- Chất lượng vật lý: Mới, Tốt, Cũ, Hỏng...
    ViTri NVARCHAR(100) NULL,
    CONSTRAINT FK_Sach_DauSach FOREIGN KEY (MaDauSach) REFERENCES DauSach(MaDauSach)
);

-------------------------------------------------------------------
-- 4. TẠO CÁC BẢNG GIAO DỊCH (NHẬP, MƯỢN, PHẠT)
-------------------------------------------------------------------

-- Bảng Phiếu Nhập
IF OBJECT_ID('PhieuNhap', 'U') IS NULL
CREATE TABLE PhieuNhap (
    MaPN VARCHAR(20) NOT NULL PRIMARY KEY,
    MaNCC VARCHAR(20) NOT NULL,
    MaNV VARCHAR(20) NOT NULL,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (TongTien >= 0),
    CONSTRAINT FK_PhieuNhap_NhaCungCap FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PhieuNhap_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng Chi Tiết Phiếu Nhập
-- Lưu ý: Cột ThanhTien để kiểu DECIMAL(18,2) thường để tầng BUS tự tính toán và lưu xuống (không dùng computed PERSISTED)
IF OBJECT_ID('ChiTietPhieuNhap', 'U') IS NULL
CREATE TABLE ChiTietPhieuNhap (
    MaPN VARCHAR(20) NOT NULL,
    MaDauSach VARCHAR(20) NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL CHECK (DonGia >= 0),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    ThanhTien DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (ThanhTien >= 0),
    PRIMARY KEY (MaPN, MaDauSach),
    CONSTRAINT FK_CTPN_PhieuNhap FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN) ON DELETE CASCADE,
    CONSTRAINT FK_CTPN_DauSach FOREIGN KEY (MaDauSach) REFERENCES DauSach(MaDauSach)
);

-- Bảng Phiếu Mượn
IF OBJECT_ID('PhieuMuon', 'U') IS NULL
CREATE TABLE PhieuMuon (
    MaPM VARCHAR(20) NOT NULL PRIMARY KEY,
    MaDocGia VARCHAR(20) NOT NULL,
    MaNV VARCHAR(20) NOT NULL,
    NgayMuon DATETIME NOT NULL DEFAULT GETDATE(),
    HanTra DATE NOT NULL,
    CONSTRAINT FK_PhieuMuon_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDG),
    CONSTRAINT FK_PhieuMuon_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    CONSTRAINT CK_PhieuMuon_HanTra CHECK (HanTra >= CAST(NgayMuon AS DATE))
);

-- Bảng Chi Tiết Phiếu Mượn
IF OBJECT_ID('ChiTietPhieuMuon', 'U') IS NULL
CREATE TABLE ChiTietPhieuMuon (
    MaPhieuMuon VARCHAR(20) NOT NULL,
    MaSach VARCHAR(20) NOT NULL,
    TinhTrang NVARCHAR(50) NULL,
    NgayThucTra DATE NULL,
    PRIMARY KEY (MaPhieuMuon, MaSach),
    CONSTRAINT FK_CTPM_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPM) ON DELETE CASCADE,
    CONSTRAINT FK_CTPM_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng Phiếu Phạt
IF OBJECT_ID('PhieuPhat', 'U') IS NULL
CREATE TABLE PhieuPhat (
    MaPhieuPhat VARCHAR(20) NOT NULL PRIMARY KEY,
    MaDocGia VARCHAR(20) NOT NULL,
    MaPhieuMuon VARCHAR(20) NULL,
    MaNV VARCHAR(20) NOT NULL,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) NULL,
    TongTienPhat DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (TongTienPhat >= 0),
    CONSTRAINT FK_PhieuPhat_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDG),
    CONSTRAINT FK_PhieuPhat_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPM),
    CONSTRAINT FK_PhieuPhat_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng Chi Tiết Phiếu Phạt (Đã tích hợp bản vá Issue #17)
-- MaCTPP INT IDENTITY làm PK; UNIQUE(MaPhieuPhat, MaSach, MaQD) cho phép 1 phiếu phạt nhiều cuốn sách cùng lỗi
IF OBJECT_ID('ChiTietPhieuPhat', 'U') IS NULL
CREATE TABLE ChiTietPhieuPhat (
    MaCTPP INT IDENTITY(1,1) NOT NULL,
    MaPhieuPhat VARCHAR(20) NOT NULL,
    MaQD VARCHAR(20) NOT NULL,
    MaSach VARCHAR(20) NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL DEFAULT 0 CHECK (DonGia >= 0),
    SoNgayTre INT NULL CHECK (SoNgayTre >= 0),
    GhiChu NVARCHAR(200) NULL,
    TienPhat DECIMAL(18, 2) NOT NULL CHECK (TienPhat >= 0),
    CONSTRAINT PK_ChiTietPhieuPhat PRIMARY KEY (MaCTPP),
    CONSTRAINT UQ_ChiTietPhieuPhat_Phieu_Sach_QuyDinh UNIQUE (MaPhieuPhat, MaSach, MaQD),
    CONSTRAINT FK_CTPP_PhieuPhat FOREIGN KEY (MaPhieuPhat) REFERENCES PhieuPhat(MaPhieuPhat) ON DELETE CASCADE,
    CONSTRAINT FK_CTPP_QuyDinhPhat FOREIGN KEY (MaQD) REFERENCES QuyDinhPhat(MaQD),
    CONSTRAINT FK_CTPP_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);
GO

-------------------------------------------------------------------
-- 5. LƯU Ý VỀ TRIGGER TỒN KHO (QUY TẮC BẮT BUỘC TỪ GIÁO VIÊN)
-- Nghiệp vụ cộng/trừ tồn kho (khi nhập, mượn, trả sách) được xử lý 100% tại tầng BUS
-- theo đúng yêu cầu kiểm tra code của GV (tránh trừ/cộng 2 lần).
-- Do đó, CSDL không dùng trigger tự động trừ/cộng SLChoMuon/SLConLai.
-------------------------------------------------------------------
IF OBJECT_ID('trg_ChiTietPhieuMuon_Insert', 'TR') IS NOT NULL DROP TRIGGER trg_ChiTietPhieuMuon_Insert;
IF OBJECT_ID('trg_ChiTietPhieuMuon_UpdateTraSach', 'TR') IS NOT NULL DROP TRIGGER trg_ChiTietPhieuMuon_UpdateTraSach;
GO

-------------------------------------------------------------------
-- 6. CHÈN DỮ LIỆU MẪU (CHUẨN 17 BẢNG ĐỒNG BỘ 100%)
-------------------------------------------------------------------

PRINT N'==================================================';
PRINT N'     BẮT ĐẦU CHÈN DỮ LIỆU MẪU VÀO 17 BẢNG';
PRINT N'==================================================';

-- 1. Danh mục nền
IF NOT EXISTS (SELECT 1 FROM NhaXuatBan WHERE MaNXB = 'NXB01')
INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChi) VALUES
('NXB01', N'Nhà Xuất Bản Trẻ', N'161 Lý Chính Thắng, Phường 7, Quận 3, TP.HCM'),
('NXB02', N'Nhà Xuất Bản Giáo Dục', N'231 Nguyễn Văn Cừ, Quận 5, TP.HCM'),
('NXB03', N'Nhà Xuất Bản Kim Đồng', N'248 Cống Quỳnh, Quận 1, TP.HCM');

IF NOT EXISTS (SELECT 1 FROM TacGia WHERE MaTG = 'TG01')
INSERT INTO TacGia (MaTG, HoLot, TenTG, NgaySinh, SDT) VALUES
('TG01', N'Nguyễn Nhật', N'Ánh', '1955-05-07', '0901234567'),
('TG02', N'Robert C.', N'Martin', '1952-12-05', '0918765432'),
('TG03', N'Andrew', N'Tanenbaum', '1944-03-16', '0922334455');

IF NOT EXISTS (SELECT 1 FROM TheLoai WHERE MaTL = 'TL01')
INSERT INTO TheLoai (MaTL, TenTL) VALUES
('TL01', N'Công nghệ thông tin'),
('TL02', N'Văn học thiếu nhi'),
('TL03', N'Kỹ thuật phần mềm');

IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = 'NCC01')
INSERT INTO NhaCungCap (MaNCC, TenNCC) VALUES
('NCC01', N'Công ty Cổ phần Sách Fahasa'),
('NCC02', N'Nhà sách Phương Nam'),
('NCC03', N'Công ty Sách Tiki');

IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'NV01')
INSERT INTO NhanVien (MaNV, HoLot, TenNV, ChucVu) VALUES
('NV01', N'Đặng Phú', N'Thành', N'Thủ thư chính'),
('NV02', N'Lê Thị',  N'Mai', N'Thủ thư'),
('NV03', N'Trần Văn', N'Nam', N'Quản lý kho');

IF NOT EXISTS (SELECT 1 FROM LoaiDocGia WHERE MaLDG = 'LDG01')
INSERT INTO LoaiDocGia (MaLDG, TenLDG) VALUES
('LDG01', N'Sinh viên đại học'),
('LDG02', N'Giảng viên'),
('LDG03', N'Nghiên cứu sinh');

IF NOT EXISTS (SELECT 1 FROM DocGia WHERE MaDG = 'DG01')
INSERT INTO DocGia (MaDG, HoLot, TenDG, NgaySinh, SDT, MaLDG) VALUES
('DG01', N'Nguyễn Văn', N'An', '2004-03-15', '0938111222', 'LDG01'),
('DG02', N'Trần Thị', N'Bình', '2003-08-22', '0945333444', 'LDG01'),
('DG03', N'Phạm Hoàng', N'Cường', '1988-11-10', '0967555666', 'LDG02');

IF NOT EXISTS (SELECT 1 FROM QuyDinhPhat WHERE MaQD = 'QD01')
INSERT INTO QuyDinhPhat (MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat) VALUES
('QD01', N'Phạt trễ hạn trả sách', N'Phạt tiền', N'Theo ngày trễ', N'Ngày', 5000.00),
('QD02', N'Phạt rách/hỏng trang sách', N'Bồi thường', N'Cố định', N'Cuốn', 30000.00),
('QD03', N'Phạt làm mất sách', N'Bồi thường', N'100% Giá bìa sách', N'Hệ số', 1.00);

-- Bảng ThamSo (Bảng thứ 17 - Yêu cầu bắt buộc số 9 của GV)
IF NOT EXISTS (SELECT 1 FROM ThamSo WHERE MaTS = 'SO_SACH_MUON_TOI_DA')
INSERT INTO ThamSo (MaTS, TenTS, GiaTri) VALUES
('SO_SACH_MUON_TOI_DA', N'Số cuốn sách tối đa mỗi lần mượn', '5'),
('SO_NGAY_MUON_TOI_DA', N'Số ngày mượn tối đa mặc định', '7'),
('TIEN_PHAT_MOI_NGAY', N'Tiền phạt trễ hạn mỗi ngày (VNĐ)', '5000'),
('NAM_XB_TOI_THIEU', N'Năm xuất bản tối thiểu hợp lệ', '1900');

-- 2. Đầu sách & Sách
IF NOT EXISTS (SELECT 1 FROM DauSach WHERE MaDauSach = 'DS01')
INSERT INTO DauSach (MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, SoLuong, SLChoMuon, SLConLai, DonGia) VALUES
('DS01', 'NXB01', 'TG01', 'TL02', N'Mắt Biếc', 2021, 3, 1, 2, 110000.00),
('DS02', 'NXB02', 'TG02', 'TL03', N'Clean Code (Mã Sạch)', 2020, 2, 1, 1, 250000.00),
('DS03', 'NXB03', 'TG03', 'TL01', N'Mạng Máy Tính Cơ Bản', 2022, 2, 0, 2, 180000.00);

-- Bảng SACH: TrangThai (0: Chưa cho mượn, 1: Đã cho mượn)
IF NOT EXISTS (SELECT 1 FROM Sach WHERE MaSach = 'S01_01')
INSERT INTO Sach (MaSach, MaDauSach, TrangThai, TinhTrang, ViTri) VALUES
('S01_01', 'DS01', 0, N'Tốt', N'Kệ A1 - Tầng 1'),
('S01_02', 'DS01', 1, N'Tốt', N'Kệ A1 - Tầng 1'), -- Đang được mượn ở PM03
('S01_03', 'DS01', 0, N'Mới', N'Kệ A1 - Tầng 1'),
('S02_01', 'DS02', 1, N'Mới', N'Kệ B2 - Tầng 2'), -- Đang được mượn ở PM02
('S02_02', 'DS02', 0, N'Mới', N'Kệ B2 - Tầng 2'),
('S03_01', 'DS03', 0, N'Tốt', N'Kệ C1 - Tầng 3'),
('S03_02', 'DS03', 0, N'Mới', N'Kệ C1 - Tầng 3');

-- 3. Phiếu Nhập
IF NOT EXISTS (SELECT 1 FROM PhieuNhap WHERE MaPN = 'PN01')
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES
('PN01', 'NCC01', 'NV03', '2026-10-01 09:00:00', 830000.00),
('PN02', 'NCC02', 'NV03', '2026-10-15 14:30:00', 360000.00);

IF NOT EXISTS (SELECT 1 FROM ChiTietPhieuNhap WHERE MaPN = 'PN01' AND MaDauSach = 'DS01')
INSERT INTO ChiTietPhieuNhap (MaPN, MaDauSach, DonGia, SoLuong, ThanhTien) VALUES
('PN01', 'DS01', 110000.00, 3, 330000.00),
('PN01', 'DS02', 250000.00, 2, 500000.00),
('PN02', 'DS03', 180000.00, 2, 360000.00);

-- 4. Phiếu Mượn
IF NOT EXISTS (SELECT 1 FROM PhieuMuon WHERE MaPM = 'PM01')
INSERT INTO PhieuMuon (MaPM, MaDocGia, MaNV, NgayMuon, HanTra) VALUES
('PM01', 'DG01', 'NV01', '2026-10-20', '2026-10-27'),
('PM02', 'DG02', 'NV02', '2026-11-01', '2026-11-08'),
('PM03', 'DG03', 'NV01', '2026-11-05', '2026-11-12');

IF NOT EXISTS (SELECT 1 FROM ChiTietPhieuMuon WHERE MaPhieuMuon = 'PM01' AND MaSach = 'S01_01')
INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, TinhTrang, NgayThucTra) VALUES
('PM01', 'S01_01', N'Tốt', '2026-11-02'),
('PM02', 'S02_01', N'Đang mượn', NULL),
('PM03', 'S01_02', N'Đang mượn', NULL);

-- 5. Phiếu Phạt
IF NOT EXISTS (SELECT 1 FROM PhieuPhat WHERE MaPhieuPhat = 'PP01')
INSERT INTO PhieuPhat (MaPhieuPhat, MaDocGia, MaPhieuMuon, MaNV, NgayLap, TrangThai, TongTienPhat) VALUES
('PP01', 'DG01', 'PM01', 'NV01', '2026-11-02 10:15:00', N'Chưa thanh toán', 30000.00);

IF NOT EXISTS (SELECT 1 FROM ChiTietPhieuPhat WHERE MaPhieuPhat = 'PP01' AND MaQD = 'QD01')
INSERT INTO ChiTietPhieuPhat (MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat) VALUES
('PP01', 'QD01', 'S01_01', 5000.00, 6, N'Trả trễ 6 ngày', 30000.00);

GO

PRINT N'==================================================';
PRINT N' HOÀN TẤT TẠO CSDL VÀ CHÈN DỮ LIỆU THÀNH CÔNG! ';
PRINT N'==================================================';
