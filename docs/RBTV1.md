# TÀI LIỆU RÀNG BUỘC TOÀN VẸN (INTEGRITY CONSTRAINTS)
## HỆ THỐNG CƠ SỞ DỮ LIỆU QUẢN LÝ THƯ VIỆN - QLTHUVIEN (CHUẨN 17 BẢNG)

> **Môn học:** Lập trình C# - Đại học Sài Gòn (GVHD: Cô Phùng Thái Thiên Trang)  
> **Kiến trúc áp dụng:** Mô hình 3 lớp chuẩn (GUI - BUS - DAO - DTO)  
> **Phiên bản:** 1.0 (Đồng bộ 100% với file CSDL `QLThuVien_2.sql` và Bộ yêu cầu đồ án `YEU_CAU_DO_AN.md`)

---

## I. NGUYÊN TẮC THIẾT KẾ & PHÂN ĐỊNH VỊ TRÍ KIỂM SOÁT

Tuân thủ nghiêm ngặt **Quy tắc sống còn của Giáo viên hướng dẫn**:
1. **"Database mà lủng là 0 điểm":** Đảm bảo đầy đủ toàn vẹn dữ liệu từ tầng CSDL (Khóa chính, Khóa ngoại, CHECK, NOT NULL) đến tầng Nghiệp vụ (BUS). Tuyệt đối không xóa dữ liệu danh mục khi đã phát sinh lịch sử giao dịch.
2. **Quy tắc vị trí xử lý:** Mọi phép tính toán (thành tiền, tổng tiền, tiền phạt), cộng/trừ tồn kho và kiểm tra quy tắc mượn/trả **bắt buộc xử lý 100% tại tầng BUS (C#)**. CSDL **không kích hoạt Trigger tồn kho** để tránh lỗi trừ 2 lần và đảm bảo sinh viên tự viết code để chấm điểm.
3. **Cấu hình động qua bảng `ThamSo`:** Các giới hạn (số sách mượn, số ngày mượn, tiền phạt) không được fix cứng trong code mà phải đọc từ bảng `ThamSo` (Bảng thứ 17 theo Yêu cầu số 9 của GV).

---

## II. CHI TIẾT CÁC NHÓM RÀNG BUỘC TOÀN VẸN

### A. Ràng buộc Miền giá trị (Domain Constraints)

Được cài đặt trực tiếp bằng câu lệnh DDL (`NOT NULL`, `CHECK`, kiểu dữ liệu) trong SQL Server:

1. **Ràng buộc Khóa chính & Không rỗng (`NOT NULL`):**
   - Tất cả các mã định danh khóa chính (`MaNXB`, `MaTG`, `MaTL`, `MaNCC`, `MaNV`, `MaLDG`, `MaDG`, `MaQD`, `MaTS`, `MaDauSach`, `MaSach`, `MaPN`, `MaPM`, `MaPhieuPhat`) bắt buộc khác `NULL`.
   - Các thông tin nghiệp vụ bắt buộc: `TenSach`, `TenDG`, `TenNV`, `NgayMuon`, `HanTra`, `NgayNhap`, `NgayLap`, `DonGia`, `SoLuong` không được phép để trống (`NOT NULL`).

2. **Ràng buộc Giá trị số không âm (`CHECK >= 0`):**
   - Bảng `DauSach`: `SoLuong >= 0`, `SLChoMuon >= 0`, `SLConLai >= 0`, `DonGia >= 0`.
   - Bảng `ChiTietPhieuNhap`: `DonGia >= 0`, `SoLuong > 0`, `ThanhTien >= 0`.
   - Bảng `PhieuNhap`: `TongTien >= 0`.
   - Bảng `QuyDinhPhat`: `MucPhat >= 0`.
   - Bảng `PhieuPhat`: `TongTienPhat >= 0`.
   - Bảng `ChiTietPhieuPhat`: `DonGia >= 0`, `SoNgayTre >= 0`, `TienPhat >= 0`.

3. **Ràng buộc Thời gian (`CHECK`):**
   - Bảng `DauSach`: Năm xuất bản không được lớn hơn năm hiện tại:  
     $$\text{NamXB} \le \text{YEAR(GETDATE())}$$
   - Bảng `PhieuMuon`: Hạn trả sách phải lớn hơn hoặc bằng ngày mượn:  
     $$\text{HanTra} \ge \text{CAST(NgayMuon AS DATE)}$$

4. **Ràng buộc Trạng thái cuốn sách (`Sach.TrangThai`):**
   - Cột `TrangThai` có kiểu `BIT` với giá trị mặc định là `0`:
     - `0`: Sách đang có sẵn trên kệ (chưa mượn).
     - `1`: Sách đang được mượn (nằm trong phiếu mượn chưa trả).
   - Cột `TinhTrang`: Lưu trữ phẩm chất vật lý của sách (`N'Mới'`, `N'Tốt'`, `N'Cũ'`, `N'Rách bìa'`, `N'Hỏng'`).

---

### B. Ràng buộc Thực thể (Entity Integrity)

Đảm bảo tính duy nhất và định danh của từng dòng dữ liệu trong 17 bảng:

1. **Khóa chính đơn (`PRIMARY KEY` - 14 bảng):**
   - Danh mục độc lập: `NhaXuatBan(MaNXB)`, `TacGia(MaTG)`, `TheLoai(MaTL)`, `NhaCungCap(MaNCC)`, `NhanVien(MaNV)`, `LoaiDocGia(MaLDG)`.
   - Thực thể người dùng & quy định: `DocGia(MaDG)`, `QuyDinhPhat(MaQD)`, `ThamSo(MaTS)`.
   - Ấn phẩm: `DauSach(MaDauSach)`, `Sach(MaSach)`.
   - Giao dịch cha: `PhieuNhap(MaPN)`, `PhieuMuon(MaPM)`, `PhieuPhat(MaPhieuPhat)`.

2. **Khóa chính phức hợp (`COMPOSITE PRIMARY KEY` - 3 bảng chi tiết):**
   - `ChiTietPhieuNhap(MaPN, MaDauSach)`: Mỗi đầu sách chỉ xuất hiện 1 lần trong 1 phiếu nhập.
   - `ChiTietPhieuMuon(MaPhieuMuon, MaSach)`: Mỗi cuốn sách cụ thể chỉ mượn 1 lần trong 1 phiếu mượn.
   - `ChiTietPhieuPhat(MaPhieuPhat, MaQD)`: Định danh dòng phạt theo phiếu và loại lỗi vi phạm.

3. **Kiểm tra tính duy nhất (Unique ID) tại tầng BUS:**
   - Trước khi thực hiện `INSERT`, lớp `BUS` luôn duyệt qua danh sách nạp sẵn trong bộ nhớ (`ArrayList` / `List<DTO>`) để kiểm tra mã không được trùng lặp.

---

### C. Ràng buộc Tham chiếu (Referential Integrity)

Thiết lập mối quan hệ giữa các bảng thông qua Khóa ngoại (`FOREIGN KEY`):

| Bảng Chứa FK | Cột Khóa Ngoại | Bảng Tham Chiếu (PK) | Hành vi `ON DELETE` |
| :--- | :--- | :--- | :--- |
| `DocGia` | `MaLDG` | `LoaiDocGia(MaLDG)` | `NO ACTION` (Chặn xóa danh mục đang dùng) |
| `DauSach` | `MaNXB` | `NhaXuatBan(MaNXB)` | `NO ACTION` |
| `DauSach` | `MaTG` | `TacGia(MaTG)` | `NO ACTION` |
| `DauSach` | `MaTL` | `TheLoai(MaTL)` | `NO ACTION` |
| `Sach` | `MaDauSach` | `DauSach(MaDauSach)` | `NO ACTION` |
| `PhieuNhap` | `MaNCC` | `NhaCungCap(MaNCC)` | `NO ACTION` |
| `PhieuNhap` | `MaNV` | `NhanVien(MaNV)` | `NO ACTION` |
| `ChiTietPhieuNhap` | `MaPN` | `PhieuNhap(MaPN)` | `CASCADE` (Xóa phiếu xóa luôn chi tiết) |
| `ChiTietPhieuNhap` | `MaDauSach`| `DauSach(MaDauSach)` | `NO ACTION` |
| `PhieuMuon` | `MaDocGia` | `DocGia(MaDG)` | `NO ACTION` |
| `PhieuMuon` | `MaNV` | `NhanVien(MaNV)` | `NO ACTION` |
| `ChiTietPhieuMuon`| `MaPhieuMuon`| `PhieuMuon(MaPM)` | `CASCADE` |
| `ChiTietPhieuMuon`| `MaSach` | `Sach(MaSach)` | `NO ACTION` |
| `PhieuPhat` | `MaDocGia` | `DocGia(MaDG)` | `NO ACTION` |
| `PhieuPhat` | `MaPhieuMuon`| `PhieuMuon(MaPM)` | `NO ACTION` |
| `PhieuPhat` | `MaNV` | `NhanVien(MaNV)` | `NO ACTION` |
| `ChiTietPhieuPhat`| `MaPhieuPhat`| `PhieuPhat(MaPhieuPhat)`| `CASCADE` |
| `ChiTietPhieuPhat`| `MaQD` | `QuyDinhPhat(MaQD)` | `NO ACTION` |
| `ChiTietPhieuPhat`| `MaSach` | `Sach(MaSach)` | `NO ACTION` |

---

### D. Ràng buộc Nghiệp vụ & Cấu hình Tham số (Business Rules - BUS Layer)

Toàn bộ các quy tắc dưới đây được xử lý **100% bằng code C# tại tầng BUS**, đáp ứng đúng chuẩn kiến trúc 3 lớp:

#### 1. Ràng buộc cấu hình theo Bảng `ThamSo` (Yêu cầu bắt buộc số 9 của GV)
Hệ thống nạp các tham số sau từ bảng `ThamSo` vào bộ nhớ khi khởi động để kiểm tra hợp lệ:
* **`SO_SACH_MUON_TOI_DA` (Mặc định: 5):** Một độc giả không được mượn vượt quá số sách này trên các phiếu mượn đang mở (chưa trả).
* **`SO_NGAY_MUON_TOI_DA` (Mặc định: 7):** Hạn trả không được vượt quá số ngày mượn cho phép:  
  $$\text{HanTra} - \text{NgayMuon} \le \text{SO\_NGAY\_MUON\_TOI\_DA}$$
* **`TIEN_PHAT_MOI_NGAY` (Mặc định: 5,000 VNĐ):** Đơn giá phạt trễ hạn được nhân với số ngày trễ.
* **`NAM_XB_TOI_THIEU` (Mặc định: 1900):** Năm xuất bản của sách phải thỏa mãn:  
  $$\text{NamXB} \ge \text{NAM\_XB\_TOI\_THIEU}$$

#### 2. Ràng buộc Quản lý Tồn kho & Trạng thái Sách
* **Bảo toàn số lượng đầu sách:**  
  $$\text{DauSach.SoLuong} = \text{DauSach.SLChoMuon} + \text{DauSach.SLConLai}$$
* **Khi Mượn sách (`PhieuMuon`):**
  - Điều kiện kiểm tra tại `BUS`: Độc giả còn quyền mượn, đầu sách có $\text{SLConLai} > 0$, cuốn sách chọn mượn có $\text{Sach.TrangThai} = 0$.
  - Cập nhật khi tạo phiếu thành công:
    - $\text{DauSach.SLChoMuon} \leftarrow \text{DauSach.SLChoMuon} + 1$
    - $\text{DauSach.SLConLai} \leftarrow \text{DauSach.SLConLai} - 1$
    - $\text{Sach.TrangThai} \leftarrow 1$ (Đang mượn)
* **Khi Trả sách (`ChiTietPhieuMuon`):**
  - Điều kiện kiểm tra tại `BUS`: $\text{NgayThucTra} \ge \text{PhieuMuon.NgayMuon}$.
  - Cập nhật khi trả:
    - Ghi nhận `NgayThucTra`.
    - $\text{DauSach.SLChoMuon} \leftarrow \text{DauSach.SLChoMuon} - 1$
    - $\text{DauSach.SLConLai} \leftarrow \text{DauSach.SLConLai} + 1$
    - $\text{Sach.TrangThai} \leftarrow 0$ (Sẵn sàng trên kệ)
* **Khi Nhập sách (`PhieuNhap`):**
  - $\text{DauSach.SoLuong} \leftarrow \text{DauSach.SoLuong} + \text{SoLuongNhap}$
  - $\text{DauSach.SLConLai} \leftarrow \text{DauSach.SLConLai} + \text{SoLuongNhap}$
  - Tự động sinh ra $N$ bản ghi cuốn `Sach` mới tương ứng với `TrangThai = 0`.

#### 3. Ràng buộc Tính toán Tài chính (Tiền tệ)
* **Thành tiền chi tiết nhập sách:**  
  $$\text{ChiTietPhieuNhap.ThanhTien} = \text{DonGia} \times \text{SoLuong}$$
  *(BUS tính toán và lưu xuống cột `ThanhTien` dạng DECIMAL thường).*
* **Tổng tiền phiếu nhập:**  
  $$\text{PhieuNhap.TongTien} = \sum (\text{ChiTietPhieuNhap.ThanhTien})$$
* **Tính phạt quá hạn trả sách:**  
  Nếu $\text{NgayThucTra} > \text{HanTra}$:
  $$\text{SoNgayTre} = \text{DATEDIFF}(\text{day}, \text{HanTra}, \text{NgayThucTra})$$
  $$\text{TienPhat} = \text{SoNgayTre} \times \text{TIEN\_PHAT\_MOI\_NGAY}$$
* **Tổng tiền phiếu phạt:**  
  $$\text{PhieuPhat.TongTienPhat} = \sum (\text{ChiTietPhieuPhat.TienPhat})$$

#### 4. Quy tắc "CSDL không được lủng" (Bảo toàn Dữ liệu)
* **Chặn xóa Độc giả:** Không được xóa độc giả nếu đang còn sách mượn chưa trả (`NgayThucTra IS NULL`) hoặc đang có phiếu phạt chưa thanh toán.
* **Chặn xóa Cuốn sách:** Không được xóa cuốn sách đang có trạng thái $\text{TrangThai} = 1$ (Đang mượn).
* **Chặn xóa Danh mục nền:** Các đối tượng `NhaXuatBan`, `TacGia`, `TheLoai`, `NhaCungCap`, `NhanVien`, `DauSach` đã phát sinh dữ liệu trong các bảng giao dịch (`PhieuNhap`, `PhieuMuon`, `PhieuPhat`) thì tầng `BUS` ngăn chặn thao tác `DELETE`, chỉ cho phép cập nhật thông tin hoặc chuyển trạng thái ngưng hoạt động.

---

## III. MA TRẬN PHÂN ĐỊNH VỊ TRÍ KIỂM SOÁT

| Nhóm Ràng Buộc | Nội dung ràng buộc | Cài đặt tại SQL Server | Cài đặt tại Tầng BUS (C#) |
| :--- | :--- | :---: | :---: |
| **Miền giá trị** | Khóa chính, NOT NULL, CHECK $\ge 0$ |  (DDL Constraints) |  (Validate Form/Model) |
| **Miền giá trị** | Năm XB $\le$ Năm hiện tại |  (CHECK constraint) |  (Kiểm tra trước khi lưu) |
| **Miền giá trị** | Hạn trả $\ge$ Ngày mượn |  (CHECK constraint) |  (Kiểm tra trên DateTimePicker) |
| **Miền giá trị** | Ngày thực trả $\ge$ Ngày mượn | ❌ (Không hỗ trợ cross-table) |  (Kiểm tra logic trả sách) |
| **Thực thể** | Tính duy nhất của Mã (PK) |  (PRIMARY KEY) |  (Duyệt ArrayList / List) |
| **Tham chiếu** | Khóa ngoại (FK) hợp lệ |  (FOREIGN KEY) |  (Chọn qua Popup / ComboBox) |
| **Nghiệp vụ** | Tham số mượn/phạt (`ThamSo`) |  (Lưu trữ 4 tham số) |  (Đọc tham số & áp dụng logic) |
| **Nghiệp vụ** | Cập nhật tồn kho mượn/trả | ❌ (Cấm dùng Trigger) |  (Cập nhật SL, TrangThai sách) |
| **Nghiệp vụ** | Tính Thành tiền, Tổng tiền, Phạt | ❌ (Không dùng Computed Col) |  (Tính toán & truyền xuống DAO)|
| **Bảo toàn** | Chống xóa dữ liệu liên kết |  (NO ACTION chặn xóa) |  (Báo lỗi chi tiết cho người dùng) |
