# ĐỒ ÁN HỌC PHẦN: LẬP TRÌNH C#

## ĐỀ TÀI: PHẦN MỀM QUẢN LÝ THƯ VIỆN

---

**THÔNG TIN NHÓM THỰC HIỆN:**
- **Trường:** Đại học Sài Gòn (SGU)
- **Khoa:** Công nghệ Thông tin
- **Giảng viên hướng dẫn:** ThS. Phùng Thái Thiên Trang
- **Thời gian hoàn thành:** Tháng 12 Năm 2026

| STT | Thành viên | Mã nhánh Git | Phân hệ phụ trách |
| :---: | :--- | :--- | :--- |
| 1 | Thành viên 1 (TV1) | `feature/member-1` | Quản lý Đầu sách, Bản sao sách, Tác giả, Thể loại, Nhà xuất bản, Nhập/Xuất Excel Đầu sách, Tìm kiếm nâng cao |
| 2 | Thành viên 2 (TV2) | `feature/member-2` | Quản lý Độc giả, Loại độc giả, Mượn - Trả sách, Gia hạn phiếu mượn, Cấu hình Tham số hệ thống, Xuất PDF Phiếu mượn, Ứng dụng Web Blazor |
| 3 | Thành viên 3 (TV3) | `feature/member-3` | Quản lý Nhập sách, Chi tiết phiếu nhập, Nhà cung cấp, Phiếu phạt, Chi tiết phiếu phạt, Xuất PDF Phiếu nhập & Phiếu phạt |
| 4 | Thành viên 4 (TV4) | `feature/member-4` | Quản lý Nhân viên, Quy định phạt, Thống kê Thu - Chi & Báo cáo 4 Quý, Xuất PDF & Excel Thống kê |

*(Lưu ý định dạng khi xuất bản in/PDF: Phông chữ **Times New Roman**, cỡ chữ **13pt**, giãn dòng **1.15 - 1.5**, không chèn mục lục, không chụp ảnh code, tổng số trang $\le 20$ trang).*

---

## I. GIỚI THIỆU ĐỀ TÀI

### 1. Lý do chọn đề tài và mục tiêu hệ thống
Công tác quản lý thư viện tại các trường đại học đòi hỏi sự chính xác cao trong việc theo dõi danh mục đầu sách, quản lý từng cuốn sách vật lý (mã vạch, vị trí kệ, tình trạng sách), kiểm soát hạn mức mượn - trả của từng nhóm độc giả, cũng như hạch toán minh bạch các khoản chi nhập sách và thu tiền phạt vi phạm.

Đề tài **Phần mềm Quản lý Thư viện** được xây dựng nhằm:
- Chuẩn hóa quy trình nghiệp vụ thư viện theo kiến trúc **3 lớp (3-Layer Architecture: `GUI - BUS - DAO` kết hợp `DTO`)** trên nền tảng **.NET 10 (C# Windows Forms)** và mở rộng thêm cổng giao diện **ASP.NET Core Blazor Server (`QLThuVien.Web`)** dùng chung tầng nghiệp vụ.
- Tự động hóa hoàn toàn việc kiểm soát tồn kho đầu sách (`SoLuong`, `SLChoMuon`, `SLConLai`) ngay tại tầng nghiệp vụ (`BUS`) khi phát sinh giao dịch Nhập sách, Mượn sách, Trả sách hoặc báo Mất sách.
- Tối ưu tốc độ tra cứu thông qua bộ đệm dữ liệu trên bộ nhớ RAM (`ArrayList` / `List<DTO>` trong tầng `BUS`), hỗ trợ tìm kiếm nâng cao kết hợp toán tử logic (`AND`, `OR`, `NOT`) và toán tử so sánh (`=`, `<>`, `>`, `>=`, `<`, `<=`).
- Hỗ trợ nhập/xuất dữ liệu đa định dạng: Nhập/Xuất danh mục Đầu sách qua **Excel (`.xlsx`)**, in ấn chứng từ Phiếu nhập, Phiếu mượn, Phiếu phạt và Báo cáo thống kê ra **PDF** (`QuestPDF`) và **Excel nhiều Sheet** (`ClosedXML`).

---

## II. THIẾT KẾ MÔ HÌNH CƠ SỞ DỮ LIỆU

### 1. Sơ đồ quan hệ thực thể (ERD) tổng quát — 17 bảng dữ liệu
Cơ sở dữ liệu `QLThuVien` trên hệ quản trị **Microsoft SQL Server** gồm đúng **17 bảng** (16 bảng theo sơ đồ gốc của giảng viên và 1 bảng `ThamSo` mở rộng theo yêu cầu cấu hình động):

```
[NhaXuatBan] (1) ───┐
[TacGia]     (1) ───┼──< (N) [DauSach] (1) ──< (N) [Sach]
[TheLoai]    (1) ───┘           │                     │
                                │ (1)                 ├──< (N) [ChiTietPhieuMuon] >── (1) [PhieuMuon]
                                ▼ (N)                 │                                       │
                        [ChiTietPhieuNhap]            └──< (N) [ChiTietPhieuPhat]             │
                                │ (N)                                 │ (N)                   │
                                ▼ (1)                                 ▼ (1)                   │
[NhaCungCap] (1) ──────< (N) [PhieuNhap]                       [QuyDinhPhat]                  │
                                │ (N)                                                         │
                                └─────────> (1) [NhanVien] (1) <──────────────────────────────┤
                                                    │ (1)                                     │
                                                    ▼ (N)                                     │
[LoaiDocGia] (1) ──────< (N) [DocGia] (1) ──< (N) [PhieuPhat] <── (1..N) ─────────────────────┘

Bảng cấu hình độc lập: [ThamSo] (Lưu giới hạn mượn, số ngày mượn, tiền phạt mỗi ngày, năm XB tối thiểu)
```

### 2. Danh mục 17 bảng và giải thích mối quan hệ, ràng buộc toàn vẹn

| STT | Tên bảng | Khóa chính (PK) | Khóa ngoại (FK) | Vai trò và Ràng buộc toàn vẹn chính |
| :-: | :--- | :--- | :--- | :--- |
| 1 | `NhaXuatBan` | `MaNXB` | — | Lưu thông tin Nhà xuất bản (`MaNXB`, `TenNXB`, `DiaChi`). Chặn xóa nếu đã có `DauSach` tham chiếu. |
| 2 | `TacGia` | `MaTG` | — | Lưu thông tin Tác giả (`MaTG`, `HoLot`, `TenTG`, `NgaySinh`, `SDT`). Chặn xóa nếu đang gắn với `DauSach`. |
| 3 | `TheLoai` | `MaTL` | — | Lưu danh mục Thể loại sách (`MaTL`, `TenTL`). Chặn xóa nếu đã có `DauSach` thuộc thể loại này. |
| 4 | `DauSach` | `MaDauSach` | `MaNXB`, `MaTG`, `MaTL` | Quản lý thông tin chung của đầu sách và 3 chỉ số tồn kho: `SoLuong` (Tổng số cuốn đang lưu hành), `SLChoMuon` (Số cuốn đang mượn), `SLConLai` (Số cuốn có sẵn trên kệ). Chặn xóa khi `SLChoMuon > 0`. |
| 5 | `Sach` | `MaSach` | `MaDauSach` | Quản lý từng cuốn sách vật lý (`TrangThai`: `0` = Chưa cho mượn, `1` = Đã cho mượn; `TinhTrang`: Mới/Tốt/Hư hỏng/Mất; `ViTri`: Kệ lưu trữ). Chặn sửa/xóa khi `TrangThai = 1` hoặc đã phát sinh lịch sử mượn/phạt. |
| 6 | `LoaiDocGia` | `MaLDG` | — | Phân loại đối tượng độc giả (`LDG01`: Sinh viên, `LDG02`: Giảng viên...). Chặn xóa khi còn độc giả thuộc loại này. |
| 7 | `DocGia` | `MaDG` | `MaLDG` | Lưu hồ sơ độc giả (`HoLot`, `TenDG`, `NgaySinh`, `SDT`, `MaLDG`). Kiểm tra SĐT 10 chữ số, ngày sinh hợp lệ. Chặn xóa nếu đã có `PhieuMuon` hoặc `PhieuPhat`. |
| 8 | `NhanVien` | `MaNV` | — | Lưu thông tin thủ thư/nhân viên (`MaNV`, `HoLot`, `TenNV`, `ChucVu`). Chuẩn hóa chữ cái đầu họ tên, chặn xóa nếu đã lập phiếu nhập, phiếu mượn hoặc phiếu phạt. |
| 9 | `NhaCungCap` | `MaNCC` | — | Lưu danh mục đơn vị cung ứng sách (`MaNCC`, `TenNCC`). Chặn xóa khi đã có `PhieuNhap`. |
| 10 | `PhieuNhap` | `MaPN` | `MaNCC`, `MaNV` | Chứng từ Master ghi nhận đợt nhập sách (`NgayNhap`, `TongTien`). `TongTien` tự động cập nhật bằng tổng `ThanhTien` của bảng chi tiết. |
| 11 | `ChiTietPhieuNhap` | `(MaPN, MaDauSach)` | `MaPN`, `MaDauSach` | Khóa chính kép. Lưu `DonGia`, `SoLuong`, `ThanhTien`. Khi thêm chi tiết nhập mới, hệ thống tự động sinh mã cuốn sách vào bảng `Sach` và cộng dồn tồn kho `DauSach`. |
| 12 | `PhieuMuon` | `MaPM` | `MaDocGia`, `MaNV` | Chứng từ Master ghi nhận giao dịch mượn sách (`NgayMuon`, `HanTra`). `HanTra >= NgayMuon`. Chặn xóa phiếu khi còn sách chưa trả hoặc đã có chi tiết. |
| 13 | `ChiTietPhieuMuon` | `(MaPhieuMuon, MaSach)` | `MaPhieuMuon`, `MaSach` | Khóa chính kép. Lưu `TinhTrang` lúc mượn/trả và `NgayThucTra` (`NULL` khi đang mượn, có giá trị ngày khi đã trả). |
| 14 | `QuyDinhPhat` | `MaQD` | — | Danh mục mức phạt chuẩn (`QD01`: Trễ hạn tính theo ngày, `QD02`: Hư hỏng cố định, `QD03`: Mất sách đền 100% giá bìa). Đồng bộ 2 chiều mức phạt `QD01` với bảng `ThamSo`. |
| 15 | `PhieuPhat` | `MaPhieuPhat` | `MaDocGia`, `MaPhieuMuon`, `MaNV` | Chứng từ Master thu tiền phạt vi phạm (`NgayLap`, `TrangThai`, `TongTienPhat`). `TongTienPhat` tự động cộng từ các dòng `ChiTietPhieuPhat`. |
| 16 | `ChiTietPhieuPhat` | `MaCTPP` (`IDENTITY`) | `MaPhieuPhat`, `MaQD`, `MaSach` | Ràng buộc `UNIQUE (MaPhieuPhat, MaQD, MaSach)` chống phạt trùng 1 lỗi trên cùng 1 cuốn sách trong 1 phiếu. Lưu `DonGia`, `SoNgayTre`, `GhiChu`, `TienPhat`. |
| 17 | `ThamSo` | `MaTS` | — | Bảng cấu hình tham số động (`SO_SACH_MUON_TOI_DA`, `SO_NGAY_MUON_TOI_DA`, `TIEN_PHAT_MOI_NGAY`, `NAM_XB_TOI_THIEU`), giúp thay đổi quy định nghiệp vụ mà không cần sửa mã nguồn. |

---

## III. CÁC CHỨC NĂNG CHÍNH CỦA CHƯƠNG TRÌNH

### 1. Chức năng Quản lý Đầu sách và Bản sao sách (`frmDauSach`, `frmSach`)
- **a. Giới thiệu chức năng:**
  Quản lý danh mục đầu sách (thông tin biên mục, đơn giá, tồn kho) kết hợp hiển thị **Main - Sub (1-N)** danh sách các cuốn sách vật lý thuộc đầu sách đang chọn ngay trên cùng màn hình. Hỗ trợ chọn khóa ngoại (`MaTG`, `MaTL`, `MaNXB`) qua hộp thoại tra cứu có ô lọc nhanh (`...`), tìm kiếm cơ bản, tìm kiếm nâng cao, và nhập/xuất danh sách đầu sách qua file Excel (`.xlsx`).
- **b. Mô tả màn hình giao diện (Form), thành phần và nút gì bấm làm gì:**
  - **Vùng nhập liệu Master (`frmDauSach`):** Các `TextBox` (`txtMaDauSach`, `txtTenSach`, `txtMaTG`, `txtMaTL`, `txtMaNXB`, `txtNamXB`, `txtSoLuong`, `txtDonGia`), 3 nút `...` (`btnChonTG`, `btnChonTL`, `btnChonNXB`).
  - **Khu vực hiển thị Main - Sub:** Lưới trái `dgvDauSach` hiển thị danh sách đầu sách; Lưới phải `dgvSach` kèm `ComboBox` `cboTrangThai` (Tất cả / Chưa cho mượn / Đã cho mượn) tự động lọc các bản sao sách của đầu sách đang chọn.
  - **Chức năng từng nút bấm:**
    - `...` (`btnChonTG`, `btnChonTL`, `btnChonNXB`): Mở popup chọn nhanh Tác giả / Thể loại / NXB (gõ để lọc, nhấp đúp để điền mã).
    - `Thêm` (`btnThem`): Kiểm tra hợp lệ, chống trùng mã và thêm đầu sách mới.
    - `Sửa` (`btnSua`): Cập nhật thông tin đầu sách đang chọn và đồng bộ lại số lượng tồn.
    - `Xóa` (`btnXoa`): Xóa đầu sách nếu `SLChoMuon = 0`.
    - `Làm mới` (`btnLamMoi`): Xóa trắng các ô nhập để chuẩn bị thêm mới.
    - `Tìm kiếm` (`btnTimKiem`): Lọc nhanh theo mã hoặc tên sách trên `ArrayList`.
    - `Tìm nâng cao` (`btnTimNangCao`): Mở `frmTimKiemPopup` cho phép kết hợp `AND`/`OR`/`NOT` và toán tử `=`, `<>`, `>`, `>=`, `<`, `<=` cho Số lượng, Năm XB, Đơn giá.
    - `Nhập Excel` / `Xuất Excel`: Nhập danh sách đầu sách từ `.xlsx` hoặc xuất danh sách hiện tại ra `.xlsx`.
    - `Tải lại` (`btnTaiLai`): Nạp lại dữ liệu mới nhất từ SQL Server.
- **c. Thuyết minh công thức tính toán (Đồng bộ số lượng tồn kho Đầu sách):**
  Các chỉ số tồn kho của một đầu sách $d$ luôn thỏa mãn bất biến:
  $$\text{SoLuong}(d) = \left|\{s \in \text{Sach}(d) \mid \text{TinhTrang}(s) \neq \text{``Mất''}\}\right|$$
  $$\text{SLChoMuon}(d) = \left|\{s \in \text{Sach}(d) \mid \text{TrangThai}(s) = 1 \land \text{TinhTrang}(s) \neq \text{``Mất''}\}\right|$$
  $$\text{SLConLai}(d) = \text{SoLuong}(d) - \text{SLChoMuon}(d)$$
- **d. Đầu vào, Đầu ra và xử lý tính toán của các hàm chính:**

| Tên hàm (`BUS` / `DAO`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `DauSachBUS.DocDSDauSach()` | Không có | `List<DauSachDTO>` | Truy vấn toàn bộ bảng `DauSach`, nạp mới vào bộ đệm `ArrayList danhSachDauSach` trong RAM. |
| `DauSachBUS.Them(DauSachDTO)` | Đối tượng `DauSachDTO` | `void` (Ném ngoại lệ nếu lỗi) | Kiểm tra rỗng, kiểm tra `DonGia >= 0`, đối chiếu `NamXB >= NAM_XB_TOI_THIEU` từ `ThamSoBUS`, kiểm tra trùng mã trên RAM, lưu xuống CSDL và gọi `DongBoSoLuong`. |
| `DauSachBUS.Xoa(string maDauSach)` | `maDauSach` | `void` | Kiểm tra `SLChoMuon > 0` thì chặn xóa; nếu hợp lệ thì xóa bản sao `Sach` chưa mượn và xóa `DauSach` trong `SqlTransaction`. |
| `DauSachBUS.TimKiemNangCao(...)` | Tiêu chí tìm, toán tử `=`, `<>`, `>`, `>=`, `<`, `<=` và `phepLogic` (`AND`/`OR`/`NOT`) | `List<DauSachDTO>` | Duyệt trên `ArrayList` trong bộ nhớ `BUS`, kết hợp danh sách vị từ (`Func<DauSachDTO, bool>`) theo `All` (`AND`), `Any` (`OR`) hoặc `!All` (`NOT`). |
| `DauSachBUS.NhapExcel(...)` | Đường dẫn file `.xlsx` | Chuỗi kết quả + `soLuongThem`, `soLuongBoQua`, `List<string> loi` | Đọc file `.xlsx` qua `DauSachExcelService`, chuẩn hóa tiêu đề cột không dấu, kiểm tra khóa ngoại (`MaTG`, `MaTL`, `MaNXB`), bỏ qua mã trùng và ghi nhận chi tiết từng dòng lỗi. |
| `SachBUS.Xoa(maSach, maDauSach)` | `maSach`, `maDauSach` | `bool` | Chặn xóa nếu sách đang cho mượn (`TrangThai = 1`), đã có trong `ChiTietPhieuMuon` hoặc `ChiTietPhieuPhat`; xóa xong tự động gọi `DauSachBUS.CapNhatSoLuong`. |

---

### 2. Chức năng Quản lý Danh mục nền: Tác giả, Thể loại, Nhà xuất bản (`frmTacGia`, `frmTheLoai`, `frmNhaXuatBan`)
- **a. Giới thiệu chức năng:**
  Quản lý 3 danh mục nền tảng phục vụ biên mục đầu sách: Tác giả (`TacGia`), Thể loại (`TheLoai`), và Nhà xuất bản (`NhaXuatBan`). Đảm bảo tính toàn vẹn khóa ngoại trước khi xóa và hỗ trợ tìm kiếm tức thời trên bộ nhớ RAM.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - `frmTacGia`: Các `TextBox` (`txtMaTG`, `txtHoLot`, `txtTenTG`, `txtSDT`), `CheckBox` `chkNgaySinh` đi kèm `DateTimePicker` `dtpNgaySinh`, các nút `Thêm`, `Sửa`, `Xóa`, `Làm mới`, `Tìm kiếm`, `Tải lại` và bảng `dgvTacGia`.
  - `frmTheLoai`: `txtMaTL`, `txtTenTL`, nhóm nút CRUD + Tìm kiếm và bảng `dgvTheLoai`.
  - `frmNhaXuatBan`: `txtMaNXB`, `txtTenNXB`, `txtDiaChi`, nhóm nút CRUD + Tìm kiếm và bảng `dgvNXB`.
- **c. Thuyết minh công thức / Quy tắc kiểm tra:**
  - Kiểm tra định dạng số điện thoại tác giả (nếu nhập): gồm đúng 10 chữ số, bắt đầu bằng số `0` (`^0\d{9}$`), ngày sinh không vượt quá ngày hiện tại.
  - Kiểm tra ràng buộc toàn vẹn trước khi xóa: Đếm số đầu sách đang tham chiếu mã danh mục; nếu $\text{Count} > 0$ thì từ chối xóa và thông báo rõ số lượng đầu sách liên quan.
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `TacGiaBUS.Them(TacGiaDTO)` / `Sua(TacGiaDTO)` | `TacGiaDTO` | `void` | Kiểm tra rỗng `MaTG`, `TenTG`, kiểm tra Regex SĐT 10 số, kiểm tra trùng mã trên `ArrayList`, thực thi `INSERT`/`UPDATE` qua `SqlParameter`. |
| `TacGiaBUS.Xoa(string maTG)` | `maTG` | `void` | Kiểm tra `DauSachDAO.DemDauSachTheoTacGia(maTG) > 0`; nếu đang được sử dụng thì ném `InvalidOperationException`, ngược lại xóa khỏi CSDL và cập nhật RAM. |
| `TheLoaiBUS.Xoa(string maTL)` | `maTL` | `void` | Kiểm tra `DauSachDAO.DemDauSachTheoTheLoai(maTL) > 0` trước khi xóa. |
| `NXBBUS.Xoa(string maNXB)` | `maNXB` | `void` | Kiểm tra `DauSachDAO.DemDauSachTheoNXB(maNXB) > 0` trước khi xóa. |

---

### 3. Chức năng Quản lý Độc giả và Loại độc giả (`frmDocGia`)
- **a. Giới thiệu chức năng:**
  Thiết kế theo mô hình **Main - Sub (1-N)** trên cùng một màn hình: Khung bên trái quản lý danh mục **Loại độc giả (`LoaiDocGia`)**, khung bên phải quản lý hồ sơ **Độc giả (`DocGia`)**. Khi chọn một Loại độc giả ở bảng trái, hệ thống có thể lọc hoặc gán nhanh loại độc giả cho độc giả mới.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - **Khung trái (Master - Loại độc giả):** `txtMaLDG`, `txtTenLDG`, các nút `btnThemLDG`, `btnSuaLDG`, `btnXoaLDG`, `btnLamMoiLDG` và lưới `dgvLoaiDocGia`.
  - **Khung phải (Detail - Độc giả):** `txtMaDG`, `txtHoLot`, `txtTenDG`, `dtpNgaySinh`, `txtSDT`, `cboLoaiDocGia`, ô tìm kiếm `txtTimKiem`, các nút `btnThemDG`, `btnSuaDG`, `btnXoaDG`, `btnLamMoiDG`, `btnTimKiem`, `btnTatCa` và lưới `dgvDocGia`.
- **c. Thuyết minh công thức / Quy tắc kiểm tra:**
  - Số điện thoại độc giả bắt buộc 10 chữ số (`0xxxxxxxxx`), độ tuổi độc giả hợp lệ ($\ge 5$ tuổi, ngày sinh không ở tương lai).
  - Tự động sinh mã tiếp theo: Duyệt danh sách trên RAM tìm hậu tố số lớn nhất $\max(k)$ của tiền tố `DG` (hoặc `LDG`) và sinh mã $\text{Prefix} + (\max(k) + 1).\text{ToString("D2")}$.
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `LoaiDocGiaBUS.Xoa(maLDG, out error)` | `maLDG` | `bool` + chuỗi `error` | Kiểm tra `dao.CoDocGiaThuocLoai(maLDG)`; chặn xóa nếu đang có độc giả thuộc loại này. |
| `DocGiaBUS.Them(dg, out error)` | `DocGiaDTO` | `bool` + chuỗi `error` | Kiểm tra họ tên, tuổi $\ge 5$, SĐT 10 số, kiểm tra trùng mã trên `ArrayList dsDocGia_arr`, thêm vào CSDL và bộ nhớ RAM. |
| `DocGiaBUS.Xoa(maDG, out error)` | `maDG` | `bool` + chuỗi `error` | Kiểm tra `dao.DangCoGiaoDich(maDG)` trên cả 2 bảng `PhieuMuon` và `PhieuPhat`; chặn xóa nếu độc giả đã từng phát sinh giao dịch. |
| `DocGiaBUS.TimKiem(tuKhoa)` | `tuKhoa` | `ArrayList` | Lọc không phân biệt hoa thường trên `dsDocGia_arr` theo `MaDG`, `HoTen`, `SDT`, `TenLDG`. |

---

### 4. Chức năng Quản lý Mượn - Trả sách và Gia hạn phiếu mượn (`frmMuonTraSach`)
- **a. Giới thiệu chức năng:**
  Quản lý toàn bộ vòng đời giao dịch mượn sách theo mô hình **Main - Sub (1-N)**: Lập phiếu mượn (Master), thêm từng cuốn sách vào phiếu mượn (Detail), ghi nhận trả sách theo từng cuốn kèm tình trạng khi trả (`Tốt`, `Hư hỏng`, `Mất`), gia hạn hạn trả, xuất phiếu mượn ra **PDF**, và tự động điều hướng sang màn hình **Lập Phiếu Phạt** khi phát hiện trả trễ hạn hoặc làm hư hỏng/mất sách.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - **Khung Master (Thông tin Phiếu mượn):** `txtMaPM`, `cboDocGia` kèm nút `...` (`btnChonDocGia` mở `frmTimKiemDocGiaPopup`), `cboNhanVien`, `dtpNgayMuon`, `dtpHanTra`, các nút `btnThemPM`, `btnGiaHan`, `btnXoaPM`, `btnLamMoiPM`, `btnXuatPdf`, thanh tìm kiếm `txtTimKiem` và lưới `dgvPhieuMuon`.
  - **Khung Detail (Chi tiết sách mượn của phiếu đang chọn):** `cboSach` (chỉ nạp những cuốn sách đang có sẵn `TrangThai = 0` và `TinhTrang <> 'Mất'`), `txtTinhTrang`, nút `btnThemSach`, `cboTinhTrangTra` (`Tốt`, `Hư hỏng`, `Mất`), nút `btnTraSach` và lưới `dgvChiTietMuon`.
- **c. Thuyết minh công thức tính toán:**
  1. **Tự động tính Hạn trả mặc định khi lập phiếu:**
     $$\text{HanTra} = \text{NgayMuon} + \text{SO\_NGAY\_MUON\_TOI\_DA} \quad (\text{lấy từ bảng } \texttt{ThamSo})$$
  2. **Kiểm tra hạn mức số cuốn mượn tối đa trên 1 phiếu:**
     $$\text{SoSachDangMuonTrongPhieu} < \text{SO\_SACH\_MUON\_TOI\_DA} \quad (\text{lấy từ bảng } \texttt{ThamSo})$$
  3. **Cập nhật tồn kho khi Mượn 1 cuốn sách $s$ thuộc đầu sách $d$:**
     $$\text{TrangThai}(s) \leftarrow 1, \quad \text{SLChoMuon}(d) \leftarrow \text{SLChoMuon}(d) + 1, \quad \text{SLConLai}(d) \leftarrow \text{SLConLai}(d) - 1$$
  4. **Cập nhật tồn kho và tính số ngày trễ khi Trả 1 cuốn sách $s$ vào ngày $\text{NgayThucTra}$:**
     $$\text{SoNgayTre} = \max\left(0, (\text{NgayThucTra}.\text{Date} - \text{HanTra}.\text{Date}).\text{Days}\right)$$
     - Nếu $\text{TinhTrangTra} \in \{\text{``Tốt''}, \text{``Hư hỏng''}\}$ (sách quay về kho):
       $$\text{TrangThai}(s) \leftarrow 0, \quad \text{SLChoMuon}(d) \leftarrow \text{SLChoMuon}(d) - 1, \quad \text{SLConLai}(d) \leftarrow \text{SLConLai}(d) + 1$$
     - Nếu $\text{TinhTrangTra} = \text{``Mất''}$ (sách bị mất, không quay về kệ):
       $$\text{TrangThai}(s) \leftarrow 0, \quad \text{TinhTrang}(s) \leftarrow \text{``Mất''}, \quad \text{SoLuong}(d) \leftarrow \text{SoLuong}(d) - 1, \quad \text{SLChoMuon}(d) \leftarrow \text{SLChoMuon}(d) - 1$$
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `PhieuMuonBUS.ThemPhieuMuon(pm, out error)` | `PhieuMuonDTO` | `bool` + `error` | Kiểm tra `MaDocGia`, `MaNV`, `HanTra >= NgayMuon`, chặn độc giả đang có phiếu quá hạn chưa trả (`DocGiaCoPhieuQuaHan`), lưu CSDL và thêm vào `ArrayList dsPhieuMuon_arr`. |
| `PhieuMuonBUS.ThemChiTietMuon(ct, out error)` | `ChiTietPhieuMuonDTO` | `bool` + `error` | Kiểm tra giới hạn `SO_SACH_MUON_TOI_DA`, kiểm tra sách có sẵn, thực thi Transaction: thêm `ChiTietPhieuMuon`, đổi `Sach.TrangThai = 1`, tăng `SLChoMuon` và giảm `SLConLai` của `DauSach`. |
| `PhieuMuonBUS.TraSach(maPM, maSach, ngayTra, tinhTrangTra, out soNgayTre, out msg)` | `maPM`, `maSach`, `ngayTra`, `tinhTrangTra` | `bool` + `soNgayTre` + `msg` | Ghi nhận `NgayThucTra`, hoàn trả hoặc trừ tồn kho `DauSach` tùy theo sách còn hay mất, tính `soNgayTre` và gợi ý mã quy định phạt (`QD01`/`QD02`/`QD03`). |
| `PhieuMuonBUS.GiaHanPhieuMuon(maPM, out hanTraMoi, out msg)` | `maPM` | `bool` + `hanTraMoi` + `msg` | Kiểm tra phiếu còn sách chưa trả và chưa quá hạn, cộng thêm `SO_NGAY_MUON_TOI_DA` ngày vào `HanTra`. |

---

### 5. Chức năng Cấu hình Tham số hệ thống (`frmThamSo`)
- **a. Giới thiệu chức năng:**
  Quản lý bảng thứ 17 (`ThamSo`) theo yêu cầu mở rộng của giảng viên, cho phép thủ thư cấu hình trực tiếp các quy định vận hành thư viện và tự động đồng bộ 2 chiều với bảng `QuyDinhPhat`.
- **b. Mô tả màn hình giao diện và các thành phần:**
  Gồm các `TextBox` (`txtMaTS` ReadOnly, `txtTenTS`, `txtGiaTri`), các nút `Cập nhật giá trị`, `Khôi phục mặc định`, `Làm mới` và bảng `dgvThamSo`.
- **c. Thuyết minh quy tắc đồng bộ:**
  - Khi cập nhật tham số `TIEN_PHAT_MOI_NGAY` trên `frmThamSo`, tầng `DAO` tự động cập nhật cột `MucPhat` của quy định `QD01` trong bảng `QuyDinhPhat` (và ngược lại khi sửa `QD01` trên `frmQuyDinhPhat`, hệ thống tự động cập nhật `TIEN_PHAT_MOI_NGAY` trong bảng `ThamSo`).
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `ThamSoBUS.LayGiaTriSoNguyen(maTS, macDinh)` | `maTS`, `macDinh` | `int` | Tìm `MaTS` trong `ArrayList dsThamSo_arr` trên RAM và chuyển đổi sang số nguyên an toàn. |
| `ThamSoBUS.CapNhat(ts, out error)` | `ThamSoDTO` | `bool` + `error` | Kiểm tra giá trị là số nguyên dương $> 0$, cập nhật CSDL, đồng bộ `QuyDinhPhat` (`QD01`) và nạp lại bộ nhớ RAM. |

---

### 6. Chức năng Quản lý Nhập sách và Nhà cung cấp (`frmPhieuNhap`, `frmNhaCungCap`)
- **a. Giới thiệu chức năng:**
  Quản lý danh mục Nhà cung cấp (`frmNhaCungCap`) và quy trình Nhập sách mới về kho theo mô hình **Main - Sub (1-N)** (`frmPhieuNhap`). Khi lưu phiếu nhập hoặc thêm dòng chi tiết phiếu nhập, hệ thống tự động tính Thành tiền, Tổng tiền, tự động sinh các mã cuốn sách vật lý mới (`S01_04`, `S01_05`...) vào bảng `Sach` và đồng bộ tồn kho `DauSach`. Hỗ trợ xuất phiếu nhập ra **PDF**.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - `frmPhieuNhap`:
    - **Khung trái (Master - Danh sách Phiếu nhập):** Thanh tìm kiếm `txtTimKiem`, thông tin phiếu (`txtMaPN`, `dtpNgayNhap`, `txtMaNCC` kèm nút `...` chọn Nhà cung cấp, `txtMaNV` kèm nút `...` chọn Nhân viên, `txtTongTien` ReadOnly), các nút `Tạo phiếu mới`, `Lưu phiếu nhập`, `Xuất PDF`, `Hủy thao tác` và bảng `dgvPhieuNhap`.
    - **Khung dưới (Detail - Chi tiết các đầu sách trong phiếu & Lập phiếu mới):** Chọn đầu sách (`cboDauSach` kèm nút `...`), `txtDonGia` ReadOnly, `numSoLuong`, `txtThanhTien` ReadOnly, nút `➕ Thêm dòng`, và thanh tác vụ dưới cùng gồm nhãn `TỔNG TIỀN PHIẾU NHẬP`, nút `🔍 Xem chi tiết` (mở cửa sổ Popup xem bảng chi tiết phiếu nhập), `💾 LƯU PHIẾU`, `🖨️ Xuất PDF`, `🔄 Làm mới`.
- **c. Thuyết minh công thức tính toán:**
  1. **Thành tiền của từng dòng chi tiết nhập đầu sách $i$:**
     $$\text{ThanhTien}_i = \text{SoLuong}_i \times \text{DonGia}_i$$
  2. **Tổng tiền của Phiếu nhập $P$:**
     $$\text{TongTien}(P) = \sum_{i=1}^{n} \text{ThanhTien}_i = \sum_{i=1}^{n} \left(\text{SoLuong}_i \times \text{DonGia}_i\right)$$
  3. **Tự động sinh bản sao sách và cập nhật tồn kho đầu sách $d$ khi nhập số lượng $k = \text{SoLuong}_i$:**
     - Sinh $k$ bản ghi mới trong bảng `Sach` với `TrangThai = 0` (Chưa cho mượn), `TinhTrang = N'Mới'`.
     - Đồng bộ lại tồn kho bảng `DauSach`:
       $$\text{SoLuong}(d) \leftarrow \text{SoLuong}(d) + k, \quad \text{SLConLai}(d) \leftarrow \text{SLConLai}(d) + k$$
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS` / `DAO`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `PhieuNhapBUS.ThemPhieuNhap(phieu, chiTiets)` | `PhieuNhapDTO`, `List<ChiTietPhieuNhapDTO>` | `void` | Kiểm tra phiếu có ít nhất 1 dòng chi tiết, `SoLuong > 0`, `DonGia > 0`, gộp các dòng trùng `MaDauSach`, tính lại `ThanhTien` và `TongTien`, mở `SqlTransaction` lưu `PhieuNhap`, `ChiTietPhieuNhap`, tự sinh các bản ghi `Sach` và đồng bộ `DauSach`. |
| `PhieuNhapBUS.ThemChiTiet(ct)` / `SuaChiTiet(ct)` | `ChiTietPhieuNhapDTO` | `void` | Tính `ct.ThanhTien = ct.SoLuong * ct.DonGia`, cập nhật bảng `ChiTietPhieuNhap`, sinh bổ sung/thu hồi bản sao `Sach`, tính lại `PhieuNhap.TongTien` và đồng bộ tồn kho `DauSach`. |

---

### 7. Chức năng Quản lý Phiếu phạt và Chi tiết vi phạm (`frmPhieuPhat`, `frmChiTietPhieuPhat`)
- **a. Giới thiệu chức năng:**
  Quản lý chứng từ thu tiền phạt vi phạm của độc giả theo mô hình **Main - Sub (1-N)** ngay trên cùng một Form (`frmPhieuPhat`) và hỗ trợ mở rộng hộp thoại chuyên biệt (`frmChiTietPhieuPhat`). Tự động tính số ngày trễ hạn từ phiếu mượn, tự động lấy đơn giá bìa hoặc mức phạt quy định để tính tiền phạt chính xác cho từng loại vi phạm (`QD01` - Trễ hạn, `QD02` - Hư hỏng, `QD03` - Làm mất sách), tự động cộng tổng tiền phạt lên phiếu Master và xuất phiếu phạt ra **PDF**.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - **Khung 1 - Thông tin Phiếu phạt (Master):** `txtMaPhieuPhat` (tự sinh `PPxx`), `dtpNgayLap`, `cboTrangThai` (`Chưa thanh toán` / `Đã thanh toán`), `txtTongTienPhat` (ReadOnly), `cboDocGia` + nút `...`, `cboPhieuMuon` (tự lọc theo độc giả), `cboNhanVien` + nút `...`, các nút `Thêm phiếu`, `Sửa phiếu`, `Xóa phiếu`, `Làm mới`, `Xuất PDF`, `Mở popup chi tiết` và bảng `dgvPhieuPhat`.
  - **Khung 2 - Chi tiết vi phạm của Phiếu phạt đang chọn (Detail):** `cboSachDetail` (tự nạp danh sách cuốn sách thuộc phiếu mượn của phiếu phạt), `cboQuyDinhDetail` (`QD01`, `QD02`, `QD03`...), `txtSoNgayTreDetail`, `txtDonGiaDetail`, `txtTienPhatDetail`, `txtGhiChuDetail`, các nút `+ Thêm vi phạm`, `Sửa vi phạm`, `Xóa vi phạm`, `Làm mới` và bảng `dgvChiTiet`.
- **c. Thuyết minh công thức tính toán Tiền phạt và Tổng tiền phạt:**
  Với mỗi dòng chi tiết vi phạm $j$ áp dụng quy định phạt $q$ trên cuốn sách $s$:
  1. **Trường hợp 1 — Phạt trễ hạn trả sách (`QD01`, cách tính theo ngày trễ):**
     $$\text{SoNgayTre}_j = \max\left(0, (\text{NgayThucTraHoacNgayLap}.\text{Date} - \text{HanTra}.\text{Date}).\text{Days}\right)$$
     $$\text{DonGia}_j = \text{MucPhat}(\text{QD01}) = \text{TIEN\_PHAT\_MOI\_NGAY}$$
     $$\text{TienPhat}_j = \text{SoNgayTre}_j \times \text{DonGia}_j$$
  2. **Trường hợp 2 — Phạt làm hư hỏng/rách sách (`QD02`, mức phạt cố định):**
     $$\text{DonGia}_j = \text{DonGiaBia}(s), \quad \text{TienPhat}_j = \text{MucPhat}(\text{QD02})$$
  3. **Trường hợp 3 — Phạt làm mất sách (`QD03`, đền bù theo hệ số giá bìa sách):**
     $$\text{DonGia}_j = \text{DonGiaBia}(s), \quad \text{TienPhat}_j = \text{DonGiaBia}(s) \times \text{MucPhat}(\text{QD03}) \quad (\text{với } \text{MucPhat}(\text{QD03}) = 1.00 \Rightarrow 100\% \text{ giá bìa})$$
  4. **Tổng tiền phạt của Phiếu phạt Master $P$:**
     $$\text{TongTienPhat}(P) = \sum_{j=1}^{m} \text{TienPhat}_j$$
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS` / `DAO`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `PhieuPhatBUS.TinhChiTietViPham(maPM, maSach, maQD, ghiChu)` | `maPM`, `maSach`, `maQD`, `ghiChu` | `ChiTietPhieuPhatDTO` | Tự động tra cứu `HanTra`, `NgayThucTra`, `DonGia` của sách và `QuyDinhPhatDTO` để tính chính xác `SoNgayTre`, `DonGia`, `TienPhat`. |
| `PhieuPhatBUS.ThemChiTiet(ct)` / `SuaChiTiet(ct)` | `ChiTietPhieuPhatDTO` | `bool` | Kiểm tra trùng lỗi `(MaPhieuPhat, MaQD, MaSach)`, tính lại `TienPhat` theo công thức chuẩn, lưu CSDL và tự động gọi `CapNhatTongTienPhat(maPhieuPhat)` trong cùng `SqlTransaction`. |
| `PhieuPhatDAO.CapNhatTongTienPhat(conn, trans, maPP)` | Kết nối, Transaction, `maPP` | `void` | Thực thi `UPDATE PhieuPhat SET TongTienPhat = COALESCE((SELECT SUM(TienPhat) FROM ChiTietPhieuPhat WHERE MaPhieuPhat = @MaPP), 0)`. |

---

### 8. Chức năng Quản lý Nhân viên và Quy định phạt (`frmNhanVien`, `frmQuyDinhPhat`)
- **a. Giới thiệu chức năng:**
  Quản lý hồ sơ Nhân viên thư viện (`frmNhanVien`) và danh mục Quy định phạt (`frmQuyDinhPhat`). Tự động sinh mã kế tiếp (`NVxx`, `QDxx`), chuẩn hóa chữ cái đầu của họ tên nhân viên theo thời gian thực và bảo vệ ràng buộc khóa ngoại khi xóa.
- **b. Mô tả màn hình giao diện và các thành phần:**
  - `frmNhanVien`: `txtMaNV` (ReadOnly, tự sinh), `txtHoLot`, `txtTenNV`, `cboChucVu` (`Thủ thư chính`, `Thủ thư`, `Quản lý kho`...), thanh tìm kiếm `txtTimKiem`, các nút `Thêm`, `Sửa`, `Xóa`, `Làm mới`, `Tìm kiếm`, `Tất cả` và bảng `dgvNhanVien`.
  - `frmQuyDinhPhat`: `txtMaQD` (ReadOnly, tự sinh), `txtTenQD`, `cboLoaiPhat`, `cboCachTinh`, `cboDonViTinh`, `numMucPhat`, thanh tìm kiếm và bảng `dgvQuyDinhPhat`.
- **c. Thuyết minh quy tắc xử lý:**
  - Hàm `NhanVienBUS.ChuanHoaHoTen`: Loại bỏ khoảng trắng thừa, viết hoa ký tự đầu mỗi từ tiếng Việt có dấu (`"  đặng   phú thành "` $\rightarrow$ `"Đặng Phú Thành"`).
  - Ràng buộc xóa Nhân viên: Kiểm tra đồng thời trên 3 bảng `PhieuMuon`, `PhieuNhap`, `PhieuPhat`; nếu nhân viên đã đứng tên lập bất kỳ phiếu nào thì chặn xóa.
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `NhanVienBUS.Them(nv)` / `Sua(nv)` | `NhanVienDTO` | `bool` | Chuẩn hóa `HoLot` và `TenNV`, kiểm tra chỉ chứa chữ cái Unicode và khoảng trắng, tự sinh `MaNV` nếu rỗng, lưu CSDL và cập nhật bộ đệm `ArrayList`. |
| `NhanVienBUS.Xoa(maNV)` | `maNV` | `bool` | Gọi `dao.KiemTraKhoaNgoai(maNV)` kiểm tra 3 bảng giao dịch trước khi xóa. |
| `QuyDinhPhatBUS.Sua(qd)` | `QuyDinhPhatDTO` | `bool` | Kiểm tra `MucPhat > 0` (nếu là hệ số thì $0 < \text{MucPhat} \le 10$), cập nhật `QuyDinhPhat` và đồng bộ sang `ThamSo` nếu là `QD01`. |

---

### 9. Chức năng Thống kê Thu - Chi và Báo cáo theo 4 Quý (`frmThongKe`)
- **a. Giới thiệu chức năng:**
  Tổng hợp bức tranh tài chính và hoạt động mượn/nhập của thư viện trên 2 góc nhìn:
  1. **Tab Thu - Chi theo khoảng ngày (`tuNgay` – `denNgay`):** Hiển thị 3 thẻ tổng hợp (`Tổng thu`, `Tổng chi`, `Chênh lệch`) và bảng nhật ký giao dịch kết hợp giữa **Thu tiền phạt (`PhieuPhat`)** và **Chi nhập sách (`PhieuNhap`)**.
  2. **Tab Thống kê theo 4 Quý trong năm (`Q1`, `Q2`, `Q3`, `Q4`):** Gồm 3 bảng thống kê chéo theo Quý cho **Nhân viên** (Tiền phạt & Tiền nhập mỗi quý), **Độc giả** (Tiền phạt mỗi quý) và **Đầu sách** (Tiền nhập & Lượt mượn mỗi quý).
  Hỗ trợ xuất toàn bộ báo cáo ra **PDF** (`ThongKePdfExporter`) và **Excel 4 Sheet** (`ThongKeExcelExporter`).
- **b. Mô tả màn hình giao diện và các thành phần:**
  - Bộ lọc khoảng ngày: `dtpTuNgay`, `dtpDenNgay`, các nút `btnThongKe`, `btnLamMoi`, `btnXuatPdf`, `btnXuatExcel` và nhãn trạng thái `lblTrangThai`.
  - Khu vực thẻ chỉ số (`pnlTongHop`): 3 thẻ `lblTongThu`, `lblTongChi`, `lblChenhLech` (đổi màu xanh khi $\ge 0$, đỏ khi $< 0$).
  - Khu vực `TabControl` (`tabsThongKe`): Tab `Thu - Chi` (`dgvGiaoDich`) và Tab `Thống kê quý` chứa bộ chọn năm `numNamThongKe` cùng 3 tab con: `Nhân viên` (`dgvNhanVienTheoQuy`), `Độc giả` (`dgvDocGiaTheoQuy`), `Đầu sách` (`dgvDauSachTheoQuy`).
- **c. Thuyết minh công thức tính toán Thống kê:**
  1. **Công thức xác định Quý ($Q \in \{1, 2, 3, 4\}$) từ tháng của ngày giao dịch ($m \in \{1..12\}$):**
     $$Q(m) = \left\lfloor \frac{m - 1}{3} \right\rfloor + 1$$
  2. **Tổng thu, Tổng chi và Chênh lệch trong kỳ $[T_{\text{từ}}, T_{\text{đến}}]$:**
     $$\text{TongThu} = \sum_{p \in \text{PhieuPhat trong kỳ}} \text{TongTienPhat}(p)$$
     $$\text{TongChi} = \sum_{n \in \text{PhieuNhap trong kỳ}} \text{TongTien}(n)$$
     $$\text{ChenhLech} = \text{TongThu} - \text{TongChi}$$
  3. **Tổng hợp theo Quý cho từng Nhân viên, Độc giả, Đầu sách trong năm $Y$:**
     $$\text{TongNam} = \sum_{q=1}^{4} \text{GiaTriQuy}_q$$
- **d. Đầu vào, Đầu ra và xử lý của các hàm chính:**

| Tên hàm (`BUS`) | Đầu vào (Input) | Đầu ra (Output) | Xử lý tính toán & Nghiệp vụ |
| :--- | :--- | :--- | :--- |
| `ThongKeBUS.LayTongHop(tuNgay, denNgay)` | `tuNgay`, `denNgay` | `ThongKeTongHopDTO` | Kiểm tra `tuNgay <= denNgay`, truy vấn `UNION ALL` giữa `PhieuPhat` (Thu) và `PhieuNhap` (Chi), tính `TongThu`, `TongChi`, `ChenhLech`. |
| `ThongKeBUS.LayThongKeNhanVienTheoQuy(nam)` | `nam` | `List<ThongKeNhanVienQuyDTO>` | Nhóm dữ liệu `PhieuPhat` và `PhieuNhap` theo từng `MaNV` và từng quý $Q \in \{1..4\}$ bằng công thức `(Month - 1) / 3 + 1`. |
| `ThongKeBUS.LayThongKeDauSachTheoQuy(nam)` | `nam` | `List<ThongKeDauSachQuyDTO>` | Tổng hợp `ThanhTien` từ `ChiTietPhieuNhap` và đếm số lượt mượn từ `ChiTietPhieuMuon` theo từng `MaDauSach` trong 4 quý. |

---

### 10. Giao diện chính và Cổng thông tin Web Blazor (`frmMain`, `QLThuVien.Web`)
- **a. Giới thiệu chức năng:**
  - `frmMain`: Cửa sổ điều hướng trung tâm của ứng dụng Desktop WinForms với thanh Sidebar bên trái, vùng làm việc `pnlContent` nhúng mượt các Form con và thanh phím tắt chức năng (`F1`..`F10`, `Alt+T`, `Alt+C`).
  - `QLThuVien.Web`: Ứng dụng Web **ASP.NET Core Blazor Server** kết nối trực tiếp vào CSDL `QLThuVien`, gồm 5 trang chức năng hoàn chỉnh:
    1. **Tra cứu Đầu sách (`/`, `Home.razor`):** Tra cứu sách trực tuyến theo tên sách, tác giả, thể loại, xem số lượng còn sẵn trên kệ.
    2. **Quản lý Thể loại (`/quan-ly-the-loai`, `QuanLyTheLoai.razor`):** Thêm, sửa, xóa, tìm kiếm thể loại sách (có kiểm tra trùng mã và ràng buộc khóa ngoại).
    3. **Quản lý Đầu sách (`/quan-ly-dau-sach`, `QuanLyDauSach.razor`):** Thêm, sửa, xóa, tìm kiếm đầu sách kèm chọn Tác giả / Thể loại / NXB.
    4. **Đặt mượn sách (`/dat-muon-sach`, `DatMuonSach.razor`):** Độc giả chọn sách còn sẵn và gửi yêu cầu đặt mượn trực tuyến.
    5. **Admin Duyệt mượn (`/duyet-muon-sach`, `DuyetMuonSach.razor`):** Thủ thư bấm **Duyệt** sẽ tự động sinh `PhieuMuon` + `ChiTietPhieuMuon` trong SQL Server và trừ tồn kho `DauSach` đồng bộ với ứng dụng WinForms.
- **b. Nút bấm chính trên Web Blazor:**
  - `Tìm kiếm` / `Làm mới`: Lọc hoặc hiển thị lại toàn bộ danh sách.
  - `Thêm` / `Lưu cập nhật` / `Sửa` / `Xóa`: Thao tác CRUD trực tiếp xuống SQL Server qua `ThuVienWebService`.
  - `Gửi yêu cầu đặt mượn`: Tạo yêu cầu chờ duyệt ở trạng thái *Chờ duyệt*.
  - `✓ Duyệt` / `✕ Từ chối` (trang Admin): Xác nhận lập phiếu mượn + trừ tồn kho SQL Server hoặc từ chối yêu cầu.

---

## IV. HƯỚNG DẪN CÀI ĐẶT ĐỒ ÁN

### a. Cây thư mục của đồ án (Kiến trúc 3 lớp `GUI - BUS - DAO` + `DTO`)
```text
TestProject/
├── SQL/
│   ├── QLThuVien1.sql                 # Script khởi tạo CSDL 17 bảng & dữ liệu mẫu đồng bộ
│   ├── Patch_0.3_CapNhatCSDL.sql      # Script cập nhật bổ sung
│   └── Patch_Issue17_ChiTietPhieuPhat.sql
├── src/
│   ├── QLThuVien.sln                  # Solution tổng gồm WinForms App & Blazor Web App
│   ├── QLThuVien/                     # Đồ án chính: Windows Forms (.NET 10)
│   │   ├── DTO/                       # 20 lớp Data Transfer Object ánh xạ 17 bảng & thống kê
│   │   ├── DAO/                       # 20 lớp Data Access Object + MyConnection.cs
│   │   ├── BUS/                       # 20 lớp Business Logic + Bộ đệm ArrayList + Xuất PDF/Excel
│   │   ├── GUI/                       # 25 màn hình Form/Popup + FormSharedData + FormErrors
│   │   └── Program.cs                 # Điểm khởi chạy ứng dụng WinForms
│   └── QLThuVien.Web/                 # Cổng thông tin Web Blazor Server (.NET 10)
│       ├── Components/Pages/          # Các trang Razor (Tra cứu, Đầu sách, Mượn trả, Phạt...)
│       └── Data/TraCuuSachService.cs  # Dịch vụ kết nối tầng BUS/DAO lên giao diện Web
└── BAO_CAO_DO_AN.md                   # Báo cáo tổng kết đồ án cuối kỳ
```
- **Ý nghĩa các tầng:**
  - **`DTO` (Data Transfer Object):** Đóng gói dữ liệu truyền tải giữa các tầng dưới dạng thuộc tính (`Property`).
  - **`DAO` (Data Access Object):** Chứa các truy vấn SQL thông qua `MyConnection` và `SqlParameter[]`, hoàn toàn không chứa logic giao diện.
  - **`BUS` (Business Logic Layer):** Lưu trữ danh sách đối tượng trên bộ nhớ RAM (`ArrayList`), kiểm tra quy tắc nghiệp vụ, tính toán công thức (thành tiền, tổng tiền, tiền phạt, tồn kho) và điều phối gọi xuống `DAO`.
  - **`GUI` (Graphical User Interface):** Hiển thị dữ liệu lên `DataGridView`, bắt sự kiện người dùng trong khối `try-catch` và chỉ giao tiếp với `BUS`.

### b. Giải thích cách kết nối cơ sở dữ liệu
Toàn bộ kết nối SQL Server được quản lý tập trung tại lớp `src/QLThuVien/DAO/MyConnection.cs`:
- **Chuỗi kết nối mặc định:**
  `Server=.;Database=QLThuVien;Integrated Security=True;TrustServerCertificate=True;`
  *(Hỗ trợ ghi đè linh hoạt qua biến môi trường `QLTHUVIEN_CONNECTION_STRING` nếu máy cài SQL Express như `.\SQLEXPRESS`)*.
- **Cơ chế hoạt động an toàn:**
  - Cung cấp 4 phương thức dùng chung: `GetConnection()`, `ExecuteQuery(query, parameters)`, `ExecuteNonQuery(query, parameters)` và `ExecuteScalar(query, parameters)`.
  - Mọi kết nối đều đặt trong khối `using (SqlConnection conn = GetConnection())` giúp tự động giải phóng tài nguyên ngay khi truy vấn xong.
  - 100% tham số đầu vào được truyền qua mảng `SqlParameter[]` có định kiểu `SqlDbType` rõ ràng, triệt tiêu hoàn toàn nguy cơ tấn công **SQL Injection**.

### c. Công cụ và thư viện sử dụng (kèm link download)
1. **.NET 10.0 SDK (Hỗ trợ C# 13, WinForms & ASP.NET Core Blazor):**
   - Link tải: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
2. **Microsoft SQL Server & SQL Server Management Studio (SSMS):**
   - Link tải SQL Server: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Link tải SSMS: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
3. **Visual Studio 2022 / Visual Studio Code:**
   - Link tải: https://visualstudio.microsoft.com/downloads/
4. **Các thư viện NuGet sử dụng trong đồ án:**
   - **`Microsoft.Data.SqlClient` (v6.1.1)** — Trình điều khiển kết nối SQL Server chính thức cho .NET:
     https://www.nuget.org/packages/Microsoft.Data.SqlClient/6.1.1
   - **`ClosedXML` (v0.105.0)** — Thư viện đọc/ghi và định dạng báo cáo Excel (`.xlsx`):
     https://www.nuget.org/packages/ClosedXML/0.105.0
   - **`QuestPDF` (v2026.2.4)** — Thư viện kết xuất chứng từ Phiếu nhập, Phiếu mượn, Phiếu phạt và Báo cáo thống kê ra PDF:
     https://www.nuget.org/packages/QuestPDF/2026.2.4

---

## V. SẢN PHẨM ĐỒ ÁN

### 1. Liên kết kho mã nguồn GitHub của nhóm
- **GitHub Repository:** https://github.com/Call-me-Ren/C-_Project
- Kho lưu trữ bao gồm đầy đủ:
  - Toàn bộ mã nguồn `src/QLThuVien` (WinForms 3 lớp) và `src/QLThuVien.Web` (Blazor Server).
  - Kịch bản tạo CSDL 17 bảng và dữ liệu mẫu tại `SQL/QLThuVien1.sql`.
  - Lịch sử làm việc nhóm qua các nhánh `feature/member-1`, `feature/member-2`, `feature/member-3`, `feature/member-4` đã hợp nhất đồng bộ vào nhánh `main`.
  - Báo cáo đồ án cuối kỳ `BAO_CAO_DO_AN.md`.

### 2. Quy trình 3 bước khởi chạy nhanh khi chấm bài
1. **Khởi tạo CSDL:** Mở SSMS, kết nối vào SQL Server local (`.` hoặc `localhost`), mở file `SQL/QLThuVien1.sql` và nhấn **Execute (F5)** để tạo CSDL `QLThuVien` gồm đủ 17 bảng và dữ liệu mẫu.
2. **Biên dịch và chạy ứng dụng Desktop WinForms:**
   ```powershell
   dotnet build src/QLThuVien.sln
   dotnet run --project src/QLThuVien/QLThuVien.csproj
   ```
3. **Chạy cổng thông tin Web Blazor (Tùy chọn mở rộng):**
   ```powershell
   dotnet run --project src/QLThuVien.Web/QLThuVien.Web.csproj
   ```
