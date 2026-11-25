-- ===================================================================
-- SCRIPT BỔ SUNG DỮ LIỆU MẪU ĐẠT CHUẨN DEMO ĐỒ ÁN:
-- (1) Danh mục & Đầu sách, Độc giả, Nhân viên, Sách: >= 10 dòng/bảng
-- (2) Phiếu Nhập, Phiếu Mượn, Phiếu Phạt (Master): >= 20 dòng/bảng
-- (3) Chi tiết Phiếu Nhập, Mượn, Phạt (Detail): >= 30 dòng/bảng
-- Trải đều 4 Quý (Q1..Q4) năm 2026 và khớp 100% tồn kho & tổng tiền!
-- ===================================================================
SET NOCOUNT ON;
USE QLThuVien;
GO

-- 1. NhaXuatBan (10 dòng: NXB01..NXB10)
INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChi)
SELECT v.MaNXB, v.TenNXB, v.DiaChi
FROM (VALUES
('NXB01', N'Nhà Xuất Bản Trẻ', N'161 Lý Chính Thắng, Quận 3, TP.HCM'),
('NXB02', N'Nhà Xuất Bản Giáo Dục', N'231 Nguyễn Văn Cừ, Quận 5, TP.HCM'),
('NXB03', N'Nhà Xuất Bản Kim Đồng', N'248 Cống Quỳnh, Quận 1, TP.HCM'),
('NXB04', N'NXB Đại học Quốc gia TP.HCM', N'Khu phố 6, Linh Trung, Thủ Đức'),
('NXB05', N'NXB Khoa học và Kỹ thuật', N'70 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội'),
('NXB06', N'NXB Thông tin và Truyền thông', N'18 Nguyễn Du, Hai Bà Trưng, Hà Nội'),
('NXB07', N'NXB Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, Quận 1, TP.HCM'),
('NXB08', N'NXB Lao động', N'175 Giảng Võ, Đống Đa, Hà Nội'),
('NXB09', N'NXB Văn học', N'18 Nguyễn Trường Tộ, Ba Đình, Hà Nội'),
('NXB10', N'NXB Thế giới', N'46 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội')
) v(MaNXB, TenNXB, DiaChi)
WHERE NOT EXISTS (SELECT 1 FROM NhaXuatBan t WHERE t.MaNXB = v.MaNXB);

-- 2. TacGia (10 dòng: TG01..TG10)
INSERT INTO TacGia (MaTG, HoLot, TenTG, NgaySinh, SDT)
SELECT v.MaTG, v.HoLot, v.TenTG, v.NgaySinh, v.SDT
FROM (VALUES
('TG01', N'Nguyễn Nhật', N'Ánh', '1955-05-07', '0901234567'),
('TG02', N'Robert C.', N'Martin', '1952-12-05', '0918765432'),
('TG03', N'Andrew', N'Tanenbaum', '1944-03-16', '0922334455'),
('TG04', N'Martin', N'Fowler', '1963-12-18', '0903112233'),
('TG05', N'Thomas H.', N'Cormen', '1956-01-01', '0904223344'),
('TG06', N'Tô', N'Hoài', '1920-09-27', '0905334455'),
('TG07', N'Nguyễn', N'Du', '1766-01-03', '0906445566'),
('TG08', N'Eric', N'Freeman', '1965-05-12', '0907556677'),
('TG09', N'Bùi Thế', N'Tâm', '1958-08-19', '0908667788'),
('TG10', N'Trần Minh', N'Tuấn', '1980-11-20', '0909778899')
) v(MaTG, HoLot, TenTG, NgaySinh, SDT)
WHERE NOT EXISTS (SELECT 1 FROM TacGia t WHERE t.MaTG = v.MaTG);

-- 3. TheLoai (10 dòng: TL01..TL10)
INSERT INTO TheLoai (MaTL, TenTL)
SELECT v.MaTL, v.TenTL
FROM (VALUES
('TL01', N'Công nghệ thông tin'),
('TL02', N'Văn học thiếu nhi'),
('TL03', N'Kỹ thuật phần mềm'),
('TL04', N'Kinh tế & Quản trị'),
('TL05', N'Toán học ứng dụng'),
('TL06', N'Ngoại ngữ'),
('TL07', N'Kỹ năng sống'),
('TL08', N'Văn học Việt Nam'),
('TL09', N'Trí tuệ nhân tạo'),
('TL10', N'Cơ sở dữ liệu')
) v(MaTL, TenTL)
WHERE NOT EXISTS (SELECT 1 FROM TheLoai t WHERE t.MaTL = v.MaTL);

-- 4. NhaCungCap (10 dòng: NCC01..NCC10)
INSERT INTO NhaCungCap (MaNCC, TenNCC)
SELECT v.MaNCC, v.TenNCC
FROM (VALUES
('NCC01', N'Công ty Cổ phần Sách Fahasa'),
('NCC02', N'Nhà sách Phương Nam'),
('NCC03', N'Công ty Sách Tiki'),
('NCC04', N'Công ty Sách Alpha Books'),
('NCC05', N'Nhà sách Minh Khai'),
('NCC06', N'Công ty Văn hóa Nhã Nam'),
('NCC07', N'Nhà sách Cá Chép'),
('NCC08', N'Công ty Sách Thái Hà'),
('NCC09', N'Nhà sách Nguyễn Văn Cừ'),
('NCC10', N'Công ty Phát hành Sách Sài Gòn')
) v(MaNCC, TenNCC)
WHERE NOT EXISTS (SELECT 1 FROM NhaCungCap t WHERE t.MaNCC = v.MaNCC);

-- 5. NhanVien (10 dòng: NV01..NV10)
INSERT INTO NhanVien (MaNV, HoLot, TenNV, ChucVu)
SELECT v.MaNV, v.HoLot, v.TenNV, v.ChucVu
FROM (VALUES
('NV01', N'Đặng Phú', N'Thành', N'Thủ thư chính'),
('NV02', N'Lê Thị', N'Mai', N'Thủ thư'),
('NV03', N'Trần Văn', N'Nam', N'Quản lý kho'),
('NV04', N'Nguyễn Hoàng', N'Anh', N'Thủ thư'),
('NV05', N'Phạm Minh', N'Tuấn', N'Quản lý kho'),
('NV06', N'Võ Thị Thu', N'Hà', N'Thủ thư'),
('NV07', N'Huỳnh Quốc', N'Bảo', N'Thủ thư'),
('NV08', N'Đỗ Ngọc', N'Linh', N'Thủ thư chính'),
('NV09', N'Bùi Thanh', N'Tùng', N'Quản lý kho'),
('NV10', N'Phan Thị Mỹ', N'Duyên', N'Thủ thư')
) v(MaNV, HoLot, TenNV, ChucVu)
WHERE NOT EXISTS (SELECT 1 FROM NhanVien t WHERE t.MaNV = v.MaNV);

-- 6. LoaiDocGia (10 dòng: LDG01..LDG10)
INSERT INTO LoaiDocGia (MaLDG, TenLDG)
SELECT v.MaLDG, v.TenLDG
FROM (VALUES
('LDG01', N'Sinh viên đại học'),
('LDG02', N'Giảng viên'),
('LDG03', N'Nghiên cứu sinh'),
('LDG04', N'Học viên cao học'),
('LDG05', N'Sinh viên liên thông'),
('LDG06', N'Sinh viên văn bằng 2'),
('LDG07', N'Cán bộ công nhân viên'),
('LDG08', N'Cựu sinh viên'),
('LDG09', N'Sinh viên trao đổi'),
('LDG10', N'Độc giả vãng lai')
) v(MaLDG, TenLDG)
WHERE NOT EXISTS (SELECT 1 FROM LoaiDocGia t WHERE t.MaLDG = v.MaLDG);

-- 7. DocGia (10 dòng: DG01..DG10)
INSERT INTO DocGia (MaDG, HoLot, TenDG, NgaySinh, SDT, MaLDG)
SELECT v.MaDG, v.HoLot, v.TenDG, v.NgaySinh, v.SDT, v.MaLDG
FROM (VALUES
('DG01', N'Nguyễn Văn', N'An', '2004-03-15', '0938111222', 'LDG01'),
('DG02', N'Trần Thị', N'Bình', '2003-08-22', '0945333444', 'LDG01'),
('DG03', N'Phạm Hoàng', N'Cường', '1988-11-10', '0967555666', 'LDG02'),
('DG04', N'Lê Minh', N'Đức', '2004-05-19', '0912345001', 'LDG01'),
('DG05', N'Hoàng Thị', N'Giang', '2005-01-25', '0912345002', 'LDG01'),
('DG06', N'Vũ Quốc', N'Huy', '1995-07-12', '0912345003', 'LDG04'),
('DG07', N'Đặng Thị', N'Lan', '2003-09-30', '0912345004', 'LDG01'),
('DG08', N'Ngô Thành', N'Long', '1985-12-03', '0912345005', 'LDG02'),
('DG09', N'Dương Ngọc', N'Minh', '2004-04-14', '0912345006', 'LDG05'),
('DG10', N'Trương Thảo', N'Vy', '2005-10-08', '0912345007', 'LDG01')
) v(MaDG, HoLot, TenDG, NgaySinh, SDT, MaLDG)
WHERE NOT EXISTS (SELECT 1 FROM DocGia t WHERE t.MaDG = v.MaDG);

-- 8. DauSach (10 dòng: DS01..DS10)
INSERT INTO DauSach (MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, SoLuong, SLChoMuon, SLConLai, DonGia)
SELECT v.MaDauSach, v.MaNXB, v.MaTG, v.MaTL, v.TenSach, v.NamXB, 0, 0, 0, v.DonGia
FROM (VALUES
('DS01', 'NXB01', 'TG01', 'TL02', N'Mắt Biếc', 2021, 110000.00),
('DS02', 'NXB02', 'TG02', 'TL03', N'Clean Code (Mã Sạch)', 2020, 250000.00),
('DS03', 'NXB03', 'TG03', 'TL01', N'Mạng Máy Tính Cơ Bản', 2022, 180000.00),
('DS04', 'NXB04', 'TG04', 'TL03', N'Refactoring - Tái Cấu Trúc Mã', 2021, 280000.00),
('DS05', 'NXB05', 'TG05', 'TL01', N'Introduction to Algorithms', 2022, 350000.00),
('DS06', 'NXB03', 'TG06', 'TL02', N'Dế Mèn Phiêu Lưu Ký', 2019, 85000.00),
('DS07', 'NXB09', 'TG07', 'TL08', N'Truyện Kiều (Chú giải)', 2020, 120000.00),
('DS08', 'NXB06', 'TG08', 'TL03', N'Head First Design Patterns', 2023, 290000.00),
('DS09', 'NXB04', 'TG09', 'TL05', N'Giải Tích & Đại Số Tuyến Tính', 2021, 145000.00),
('DS10', 'NXB06', 'TG10', 'TL10', N'Hệ Quản Trị Cơ Sở Dữ Liệu SQL', 2024, 195000.00)
) v(MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, DonGia)
WHERE NOT EXISTS (SELECT 1 FROM DauSach t WHERE t.MaDauSach = v.MaDauSach);

-- 9. Sach (Tối thiểu 4 cuốn mỗi đầu sách = 40 cuốn: S01_01..S10_04)
INSERT INTO Sach (MaSach, MaDauSach, TrangThai, TinhTrang, ViTri)
SELECT v.MaSach, v.MaDauSach, 0, v.TinhTrang, v.ViTri
FROM (VALUES
('S01_01', 'DS01', N'Tốt', N'Kệ A1 - Tầng 1'), ('S01_02', 'DS01', N'Tốt', N'Kệ A1 - Tầng 1'), ('S01_03', 'DS01', N'Mới', N'Kệ A1 - Tầng 1'), ('S01_04', 'DS01', N'Mới', N'Kệ A1 - Tầng 1'),
('S02_01', 'DS02', N'Mới', N'Kệ B2 - Tầng 2'), ('S02_02', 'DS02', N'Mới', N'Kệ B2 - Tầng 2'), ('S02_03', 'DS02', N'Tốt', N'Kệ B2 - Tầng 2'), ('S02_04', 'DS02', N'Mới', N'Kệ B2 - Tầng 2'),
('S03_01', 'DS03', N'Tốt', N'Kệ C1 - Tầng 3'), ('S03_02', 'DS03', N'Mới', N'Kệ C1 - Tầng 3'), ('S03_03', 'DS03', N'Tốt', N'Kệ C1 - Tầng 3'), ('S03_04', 'DS03', N'Mới', N'Kệ C1 - Tầng 3'),
('S04_01', 'DS04', N'Mới', N'Kệ B3 - Tầng 2'), ('S04_02', 'DS04', N'Tốt', N'Kệ B3 - Tầng 2'), ('S04_03', 'DS04', N'Mới', N'Kệ B3 - Tầng 2'), ('S04_04', 'DS04', N'Mới', N'Kệ B3 - Tầng 2'),
('S05_01', 'DS05', N'Mới', N'Kệ C2 - Tầng 3'), ('S05_02', 'DS05', N'Tốt', N'Kệ C2 - Tầng 3'), ('S05_03', 'DS05', N'Mới', N'Kệ C2 - Tầng 3'), ('S05_04', 'DS05', N'Mới', N'Kệ C2 - Tầng 3'),
('S06_01', 'DS06', N'Tốt', N'Kệ A2 - Tầng 1'), ('S06_02', 'DS06', N'Mới', N'Kệ A2 - Tầng 1'), ('S06_03', 'DS06', N'Tốt', N'Kệ A2 - Tầng 1'), ('S06_04', 'DS06', N'Mới', N'Kệ A2 - Tầng 1'),
('S07_01', 'DS07', N'Tốt', N'Kệ A3 - Tầng 1'), ('S07_02', 'DS07', N'Mới', N'Kệ A3 - Tầng 1'), ('S07_03', 'DS07', N'Tốt', N'Kệ A3 - Tầng 1'), ('S07_04', 'DS07', N'Mới', N'Kệ A3 - Tầng 1'),
('S08_01', 'DS08', N'Mới', N'Kệ B4 - Tầng 2'), ('S08_02', 'DS08', N'Tốt', N'Kệ B4 - Tầng 2'), ('S08_03', 'DS08', N'Mới', N'Kệ B4 - Tầng 2'), ('S08_04', 'DS08', N'Mới', N'Kệ B4 - Tầng 2'),
('S09_01', 'DS09', N'Tốt', N'Kệ D1 - Tầng 3'), ('S09_02', 'DS09', N'Mới', N'Kệ D1 - Tầng 3'), ('S09_03', 'DS09', N'Tốt', N'Kệ D1 - Tầng 3'), ('S09_04', 'DS09', N'Mới', N'Kệ D1 - Tầng 3'),
('S10_01', 'DS10', N'Mới', N'Kệ C3 - Tầng 3'), ('S10_02', 'DS10', N'Tốt', N'Kệ C3 - Tầng 3'), ('S10_03', 'DS10', N'Mới', N'Kệ C3 - Tầng 3'), ('S10_04', 'DS10', N'Mới', N'Kệ C3 - Tầng 3')
) v(MaSach, MaDauSach, TinhTrang, ViTri)
WHERE NOT EXISTS (SELECT 1 FROM Sach t WHERE t.MaSach = v.MaSach);

-- 10. PhieuNhap (20 dòng: PN01..PN20 trải đều Q1, Q2, Q3, Q4 năm 2026)
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien)
SELECT v.MaPN, v.MaNCC, v.MaNV, v.NgayNhap, 0
FROM (VALUES
('PN01', 'NCC01', 'NV03', '2026-01-12 09:00:00'),
('PN02', 'NCC02', 'NV05', '2026-01-25 14:30:00'),
('PN03', 'NCC03', 'NV03', '2026-02-10 10:00:00'),
('PN04', 'NCC04', 'NV09', '2026-02-22 15:00:00'),
('PN05', 'NCC05', 'NV03', '2026-03-08 08:45:00'),
('PN06', 'NCC06', 'NV05', '2026-04-05 09:30:00'),
('PN07', 'NCC07', 'NV09', '2026-04-18 11:00:00'),
('PN08', 'NCC08', 'NV03', '2026-05-09 14:00:00'),
('PN09', 'NCC09', 'NV05', '2026-05-24 16:00:00'),
('PN10', 'NCC10', 'NV09', '2026-06-14 09:15:00'),
('PN11', 'NCC01', 'NV03', '2026-07-07 10:30:00'),
('PN12', 'NCC02', 'NV05', '2026-07-20 13:45:00'),
('PN13', 'NCC03', 'NV09', '2026-08-03 08:30:00'),
('PN14', 'NCC04', 'NV03', '2026-08-19 15:20:00'),
('PN15', 'NCC05', 'NV05', '2026-09-11 09:50:00'),
('PN16', 'NCC06', 'NV09', '2026-10-01 09:00:00'),
('PN17', 'NCC07', 'NV03', '2026-10-15 14:30:00'),
('PN18', 'NCC08', 'NV05', '2026-10-28 10:15:00'),
('PN19', 'NCC09', 'NV09', '2026-11-05 11:30:00'),
('PN20', 'NCC10', 'NV03', '2026-11-18 16:10:00')
) v(MaPN, MaNCC, MaNV, NgayNhap)
WHERE NOT EXISTS (SELECT 1 FROM PhieuNhap t WHERE t.MaPN = v.MaPN);

-- 11. ChiTietPhieuNhap (30 dòng trải đều trên 20 phiếu PN01..PN20)
INSERT INTO ChiTietPhieuNhap (MaPN, MaDauSach, DonGia, SoLuong, ThanhTien)
SELECT v.MaPN, v.MaDauSach, ds.DonGia, v.SoLuong, ds.DonGia * v.SoLuong
FROM (VALUES
('PN01', 'DS01', 2), ('PN01', 'DS02', 2),
('PN02', 'DS03', 2), ('PN02', 'DS04', 2),
('PN03', 'DS05', 2), ('PN03', 'DS06', 2),
('PN04', 'DS07', 2), ('PN04', 'DS08', 2),
('PN05', 'DS09', 2), ('PN05', 'DS10', 2),
('PN06', 'DS01', 2), ('PN06', 'DS03', 2),
('PN07', 'DS02', 2), ('PN07', 'DS05', 2),
('PN08', 'DS04', 2), ('PN08', 'DS06', 2),
('PN09', 'DS07', 2), ('PN09', 'DS09', 2),
('PN10', 'DS08', 2), ('PN10', 'DS10', 2),
('PN11', 'DS01', 1), ('PN12', 'DS02', 1),
('PN13', 'DS03', 1), ('PN14', 'DS04', 1),
('PN15', 'DS05', 1), ('PN16', 'DS06', 1),
('PN17', 'DS07', 1), ('PN18', 'DS08', 1),
('PN19', 'DS09', 1), ('PN20', 'DS10', 1)
) v(MaPN, MaDauSach, SoLuong)
INNER JOIN DauSach ds ON ds.MaDauSach = v.MaDauSach
WHERE NOT EXISTS (SELECT 1 FROM ChiTietPhieuNhap t WHERE t.MaPN = v.MaPN AND t.MaDauSach = v.MaDauSach);

-- Đồng bộ TongTien cho PhieuNhap = SUM(ThanhTien)
UPDATE pn
SET TongTien = COALESCE((SELECT SUM(ct.ThanhTien) FROM ChiTietPhieuNhap ct WHERE ct.MaPN = pn.MaPN), 0)
FROM PhieuNhap pn;

-- 12. PhieuMuon (20 dòng: PM01..PM20 trải đều Q1..Q4 năm 2026)
INSERT INTO PhieuMuon (MaPM, MaDocGia, MaNV, NgayMuon, HanTra)
SELECT v.MaPM, v.MaDocGia, v.MaNV, v.NgayMuon, DATEADD(DAY, 7, v.NgayMuon)
FROM (VALUES
('PM01', 'DG01', 'NV01', CAST('2026-01-15' AS DATE)),
('PM02', 'DG02', 'NV02', CAST('2026-01-28' AS DATE)),
('PM03', 'DG03', 'NV04', CAST('2026-02-12' AS DATE)),
('PM04', 'DG04', 'NV06', CAST('2026-02-25' AS DATE)),
('PM05', 'DG05', 'NV07', CAST('2026-03-10' AS DATE)),
('PM06', 'DG06', 'NV08', CAST('2026-04-04' AS DATE)),
('PM07', 'DG07', 'NV10', CAST('2026-04-19' AS DATE)),
('PM08', 'DG08', 'NV01', CAST('2026-05-06' AS DATE)),
('PM09', 'DG09', 'NV02', CAST('2026-05-21' AS DATE)),
('PM10', 'DG10', 'NV04', CAST('2026-06-15' AS DATE)),
('PM11', 'DG01', 'NV06', CAST('2026-07-08' AS DATE)),
('PM12', 'DG02', 'NV07', CAST('2026-07-22' AS DATE)),
('PM13', 'DG03', 'NV08', CAST('2026-08-09' AS DATE)),
('PM14', 'DG04', 'NV10', CAST('2026-08-24' AS DATE)),
('PM15', 'DG05', 'NV01', CAST('2026-09-12' AS DATE)),
('PM16', 'DG06', 'NV02', CAST('2026-10-03' AS DATE)),
('PM17', 'DG07', 'NV04', CAST('2026-10-18' AS DATE)),
('PM18', 'DG08', 'NV06', CAST('2026-11-01' AS DATE)),
('PM19', 'DG09', 'NV07', CAST('2026-11-08' AS DATE)),
('PM20', 'DG10', 'NV01', CAST('2026-11-15' AS DATE))
) v(MaPM, MaDocGia, MaNV, NgayMuon)
WHERE NOT EXISTS (SELECT 1 FROM PhieuMuon t WHERE t.MaPM = v.MaPM);

-- 13. ChiTietPhieuMuon (30 dòng trên PM01..PM20; PM19 & PM20 có 2 cuốn đang mượn TrangThai=1, các cuốn khác đã trả)
INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, TinhTrang, NgayThucTra)
SELECT v.MaPM, v.MaSach, v.TinhTrang, v.NgayThucTra
FROM (VALUES
('PM01', 'S01_01', N'Tốt', CAST('2026-01-25' AS DATE)),
('PM01', 'S02_01', N'Tốt', CAST('2026-01-22' AS DATE)),
('PM02', 'S03_01', N'Tốt', CAST('2026-02-07' AS DATE)),
('PM02', 'S04_01', N'Tốt', CAST('2026-02-04' AS DATE)),
('PM03', 'S05_01', N'Tốt', CAST('2026-02-22' AS DATE)),
('PM03', 'S06_01', N'Tốt', CAST('2026-02-19' AS DATE)),
('PM04', 'S07_01', N'Tốt', CAST('2026-03-07' AS DATE)),
('PM04', 'S08_01', N'Tốt', CAST('2026-03-03' AS DATE)),
('PM05', 'S09_01', N'Hư hỏng', CAST('2026-03-19' AS DATE)),
('PM05', 'S10_01', N'Tốt', CAST('2026-03-17' AS DATE)),
('PM06', 'S01_02', N'Tốt', CAST('2026-04-14' AS DATE)),
('PM06', 'S02_02', N'Tốt', CAST('2026-04-10' AS DATE)),
('PM07', 'S03_02', N'Hư hỏng', CAST('2026-04-28' AS DATE)),
('PM07', 'S04_02', N'Tốt', CAST('2026-04-25' AS DATE)),
('PM08', 'S05_02', N'Tốt', CAST('2026-05-16' AS DATE)),
('PM08', 'S06_02', N'Tốt', CAST('2026-05-12' AS DATE)),
('PM09', 'S07_02', N'Hư hỏng', CAST('2026-05-30' AS DATE)),
('PM09', 'S08_02', N'Tốt', CAST('2026-05-27' AS DATE)),
('PM10', 'S09_02', N'Tốt', CAST('2026-06-26' AS DATE)),
('PM10', 'S10_02', N'Tốt', CAST('2026-06-21' AS DATE)),
('PM11', 'S01_03', N'Tốt', CAST('2026-07-18' AS DATE)),
('PM12', 'S02_03', N'Tốt', CAST('2026-08-01' AS DATE)),
('PM13', 'S03_03', N'Tốt', CAST('2026-08-20' AS DATE)),
('PM14', 'S04_03', N'Tốt', CAST('2026-09-05' AS DATE)),
('PM15', 'S05_03', N'Hư hỏng', CAST('2026-09-21' AS DATE)),
('PM16', 'S06_03', N'Tốt', CAST('2026-10-14' AS DATE)),
('PM17', 'S07_03', N'Tốt', CAST('2026-10-29' AS DATE)),
('PM18', 'S08_03', N'Tốt', CAST('2026-11-12' AS DATE)),
('PM19', 'S01_02', N'Đang mượn', NULL),
('PM20', 'S02_01', N'Đang mượn', NULL)
) v(MaPM, MaSach, TinhTrang, NgayThucTra)
WHERE NOT EXISTS (SELECT 1 FROM ChiTietPhieuMuon t WHERE t.MaPhieuMuon = v.MaPM AND t.MaSach = v.MaSach);

-- Đồng bộ trạng thái sách đang mượn (TrangThai = 1 nếu có NgayThucTra IS NULL)
UPDATE s
SET TrangThai = CASE WHEN EXISTS (SELECT 1 FROM ChiTietPhieuMuon ct WHERE ct.MaSach = s.MaSach AND ct.NgayThucTra IS NULL) THEN 1 ELSE 0 END
FROM Sach s;

-- Đồng bộ SLChoMuon, SoLuong, SLConLai cho toàn bộ DauSach
UPDATE ds
SET SoLuong = (SELECT COUNT(*) FROM Sach s WHERE s.MaDauSach = ds.MaDauSach AND ISNULL(s.TinhTrang, N'') <> N'Mất'),
    SLChoMuon = (SELECT COUNT(*) FROM Sach s WHERE s.MaDauSach = ds.MaDauSach AND s.TrangThai = 1 AND ISNULL(s.TinhTrang, N'') <> N'Mất'),
    SLConLai = (SELECT COUNT(*) FROM Sach s WHERE s.MaDauSach = ds.MaDauSach AND s.TrangThai = 0 AND ISNULL(s.TinhTrang, N'') <> N'Mất')
FROM DauSach ds;

-- 14. PhieuPhat (20 dòng: PP01..PP20 trải đều Q1..Q4 năm 2026)
INSERT INTO PhieuPhat (MaPhieuPhat, MaDocGia, MaPhieuMuon, MaNV, NgayLap, TrangThai, TongTienPhat)
SELECT v.MaPP, v.MaDG, v.MaPM, v.MaNV, v.NgayLap, v.TrangThai, 0
FROM (VALUES
('PP01', 'DG01', 'PM01', 'NV01', '2026-01-25 10:00:00', N'Đã thanh toán'),
('PP02', 'DG02', 'PM02', 'NV02', '2026-02-07 11:00:00', N'Đã thanh toán'),
('PP03', 'DG03', 'PM03', 'NV04', '2026-02-22 14:00:00', N'Đã thanh toán'),
('PP04', 'DG04', 'PM04', 'NV06', '2026-03-07 15:00:00', N'Đã thanh toán'),
('PP05', 'DG05', 'PM05', 'NV07', '2026-03-19 09:30:00', N'Đã thanh toán'),
('PP06', 'DG06', 'PM06', 'NV08', '2026-04-14 10:15:00', N'Đã thanh toán'),
('PP07', 'DG07', 'PM07', 'NV10', '2026-04-28 16:00:00', N'Đã thanh toán'),
('PP08', 'DG08', 'PM08', 'NV01', '2026-05-16 09:00:00', N'Đã thanh toán'),
('PP09', 'DG09', 'PM09', 'NV02', '2026-05-30 14:30:00', N'Đã thanh toán'),
('PP10', 'DG10', 'PM10', 'NV04', '2026-06-26 11:20:00', N'Đã thanh toán'),
('PP11', 'DG01', 'PM11', 'NV06', '2026-07-18 10:45:00', N'Đã thanh toán'),
('PP12', 'DG02', 'PM12', 'NV07', '2026-08-01 13:15:00', N'Đã thanh toán'),
('PP13', 'DG03', 'PM13', 'NV08', '2026-08-20 15:40:00', N'Đã thanh toán'),
('PP14', 'DG04', 'PM14', 'NV10', '2026-09-05 09:10:00', N'Đã thanh toán'),
('PP15', 'DG05', 'PM15', 'NV01', '2026-09-21 16:25:00', N'Đã thanh toán'),
('PP16', 'DG06', 'PM16', 'NV02', '2026-10-14 10:00:00', N'Đã thanh toán'),
('PP17', 'DG07', 'PM17', 'NV04', '2026-10-29 14:00:00', N'Đã thanh toán'),
('PP18', 'DG08', 'PM18', 'NV06', '2026-11-12 11:30:00', N'Đã thanh toán'),
('PP19', 'DG09', 'PM19', 'NV07', '2026-11-19 15:00:00', N'Chưa thanh toán'),
('PP20', 'DG10', 'PM20', 'NV01', '2026-11-24 10:30:00', N'Chưa thanh toán')
) v(MaPP, MaDG, MaPM, MaNV, NgayLap, TrangThai)
WHERE NOT EXISTS (SELECT 1 FROM PhieuPhat t WHERE t.MaPhieuPhat = v.MaPP);

-- 15. ChiTietPhieuPhat (30 dòng trên PP01..PP20)
INSERT INTO ChiTietPhieuPhat (MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat)
SELECT v.MaPP, v.MaQD, v.MaSach, v.DonGia, v.SoNgayTre, v.GhiChu, v.TienPhat
FROM (VALUES
('PP01', 'QD01', 'S01_01', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP01', 'QD02', 'S01_01', 30000.00, NULL, N'Làm quăn góc trang sách', 30000.00),
('PP02', 'QD01', 'S03_01', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP02', 'QD02', 'S03_01', 30000.00, NULL, N'Rách bìa nhẹ', 30000.00),
('PP03', 'QD01', 'S05_01', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP03', 'QD02', 'S05_01', 30000.00, NULL, N'Ghi chú bằng bút mực', 30000.00),
('PP04', 'QD01', 'S07_01', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP04', 'QD02', 'S07_01', 30000.00, NULL, N'Ướt trang đầu', 30000.00),
('PP05', 'QD01', 'S09_01', 5000.00, 2, N'Trả trễ 2 ngày', 10000.00),
('PP05', 'QD02', 'S09_01', 30000.00, NULL, N'Hư hỏng gáy sách', 30000.00),
('PP06', 'QD01', 'S01_02', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP06', 'QD02', 'S01_02', 30000.00, NULL, N'Rách 1 trang', 30000.00),
('PP07', 'QD01', 'S03_02', 5000.00, 2, N'Trả trễ 2 ngày', 10000.00),
('PP07', 'QD02', 'S03_02', 30000.00, NULL, N'Hư hỏng nhẹ', 30000.00),
('PP08', 'QD01', 'S05_02', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP08', 'QD02', 'S05_02', 30000.00, NULL, N'Bẩn trang sách', 30000.00),
('PP09', 'QD01', 'S07_02', 5000.00, 2, N'Trả trễ 2 ngày', 10000.00),
('PP09', 'QD02', 'S07_02', 30000.00, NULL, N'Rách bìa sau', 30000.00),
('PP10', 'QD01', 'S09_02', 5000.00, 4, N'Trả trễ 4 ngày', 20000.00),
('PP10', 'QD02', 'S09_02', 30000.00, NULL, N'Cong gáy sách', 30000.00),
('PP11', 'QD01', 'S01_03', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP12', 'QD01', 'S02_03', 5000.00, 3, N'Trả trễ 3 ngày', 15000.00),
('PP13', 'QD01', 'S03_03', 5000.00, 4, N'Trả trễ 4 ngày', 20000.00),
('PP14', 'QD01', 'S04_03', 5000.00, 5, N'Trả trễ 5 ngày', 25000.00),
('PP15', 'QD01', 'S05_03', 5000.00, 2, N'Trả trễ 2 ngày', 10000.00),
('PP16', 'QD01', 'S06_03', 5000.00, 4, N'Trả trễ 4 ngày', 20000.00),
('PP17', 'QD01', 'S07_03', 5000.00, 4, N'Trả trễ 4 ngày', 20000.00),
('PP18', 'QD01', 'S08_03', 5000.00, 4, N'Trả trễ 4 ngày', 20000.00),
('PP19', 'QD01', 'S01_02', 5000.00, 4, N'Trễ hạn 4 ngày', 20000.00),
('PP20', 'QD01', 'S02_01', 5000.00, 2, N'Trễ hạn 2 ngày', 10000.00)
) v(MaPP, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat)
WHERE NOT EXISTS (SELECT 1 FROM ChiTietPhieuPhat t WHERE t.MaPhieuPhat = v.MaPP AND t.MaQD = v.MaQD AND t.MaSach = v.MaSach);

-- Đồng bộ TongTienPhat cho PhieuPhat = SUM(TienPhat)
UPDATE pp
SET TongTienPhat = COALESCE((SELECT SUM(ct.TienPhat) FROM ChiTietPhieuPhat ct WHERE ct.MaPhieuPhat = pp.MaPhieuPhat), 0)
FROM PhieuPhat pp;
GO

-- Kiểm tra lại số lượng dòng của tất cả 17 bảng:
SELECT 'NhaXuatBan' AS Bang, COUNT(*) AS SoDong FROM NhaXuatBan
UNION ALL SELECT 'TacGia', COUNT(*) FROM TacGia
UNION ALL SELECT 'TheLoai', COUNT(*) FROM TheLoai
UNION ALL SELECT 'NhaCungCap', COUNT(*) FROM NhaCungCap
UNION ALL SELECT 'NhanVien', COUNT(*) FROM NhanVien
UNION ALL SELECT 'LoaiDocGia', COUNT(*) FROM LoaiDocGia
UNION ALL SELECT 'DocGia', COUNT(*) FROM DocGia
UNION ALL SELECT 'QuyDinhPhat', COUNT(*) FROM QuyDinhPhat
UNION ALL SELECT 'ThamSo', COUNT(*) FROM ThamSo
UNION ALL SELECT 'DauSach', COUNT(*) FROM DauSach
UNION ALL SELECT 'Sach', COUNT(*) FROM Sach
UNION ALL SELECT 'PhieuNhap', COUNT(*) FROM PhieuNhap
UNION ALL SELECT 'ChiTietPhieuNhap', COUNT(*) FROM ChiTietPhieuNhap
UNION ALL SELECT 'PhieuMuon', COUNT(*) FROM PhieuMuon
UNION ALL SELECT 'ChiTietPhieuMuon', COUNT(*) FROM ChiTietPhieuMuon
UNION ALL SELECT 'PhieuPhat', COUNT(*) FROM PhieuPhat
UNION ALL SELECT 'ChiTietPhieuPhat', COUNT(*) FROM ChiTietPhieuPhat;
GO
