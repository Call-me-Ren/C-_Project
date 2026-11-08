# TỔNG HỢP TOÀN BỘ YÊU CẦU ĐỒ ÁN: QUẢN LÝ THƯ VIỆN (LẬP TRÌNH C#)

> Tài liệu này tổng hợp toàn bộ yêu cầu từ các nguồn chính thức của Giáo viên hướng dẫn (**Cô Phùng Thái Thiên Trang** — Đại học Sài Gòn) và các tài liệu thiết kế của nhóm trong thư mục `requirement_input/`, đồng thời **HIGHLIGHT (Làm nổi bật)** các điểm mâu thuẫn giữa các tài liệu và phương án giải quyết đã chốt.

---

## I. THÔNG TIN CHUNG & MỐC THỜI GIAN (TIMELINE)

- **Học phần:** Lập trình C# (12 tuần).
- **Giảng viên hướng dẫn:** Cô Phùng Thái Thiên Trang.
- **Đề tài:** Phần mềm Quản lý Thư viện.
- **Quy mô nhóm:** 4 sinh viên.
- **Nền tảng công nghệ:**
  - **Giao diện chính:** C# Windows Forms (.NET 10) trên Visual Studio 2022 + VS Code.
  - **Hệ quản trị CSDL:** SQL Server (thao tác qua **SQL Server Management Studio - SSMS**), kết nối bằng thư viện `Microsoft.Data.SqlClient`.
  - **Giao diện mở rộng (Điểm cộng):** Tối thiểu 1 trang Web bằng **ASP.NET Core / Blazor**.
- **Mốc thời gian quan trọng:**
  - **Tuần 9 + 10:** Kiểm tra giấy 10 phút trên lớp (viết tay 1–2 hàm trong đồ án của mình để chứng minh tự code và hiểu bài). Hoàn thiện toàn bộ phần mềm trong **Tuần 10**.
  - **Tuần 11 + 12:** Bốc thăm thứ tự chấm đồ án, vấn đáp trực tiếp trên code và nộp Báo cáo Word cuối kỳ (Tháng 12/2026).

---

## II. CÁC QUY TẮC SỐNG CÒN CỦA GIÁO VIÊN (VI PHẠM LÀ TRỪ ĐIỂM NẶNG / 0 ĐIỂM)

> [!CAUTION]
> **CẢNH BÁO TỪ GHI CHÚ CỦA GIÁO VIÊN (`Version 1 viết trong DAO.md` & `Note.md`):**
> 1. **"Database mà lủng là 0 điểm"**: Cơ sở dữ liệu phải đảm bảo đầy đủ khóa chính (PK), khóa ngoại (FK), ràng buộc miền giá trị (CHECK), không được xóa cứng các dữ liệu danh mục đã phát sinh giao dịch.
> 2. **Quy tắc 1 (Luồng gọi 3 lớp):** `GUI` gọi `BUS`, `BUS` gọi `DAO`. **Tuyệt đối cấm nhảy bước** (`GUI` không bao giờ được gọi trực tiếp `DAO` hay `MyConnection`).
> 3. **Quy tắc 2 (Vị trí câu lệnh SQL):** **Tuyệt đối cấm viết câu truy vấn SQL ở lớp `GUI` hoặc `BUS`**. Câu truy vấn SQL chỉ được phép nằm trong lớp `DAO`.
> 4. **Quy tắc 3 (Mô hình 3-Layer Version 2 - Class `MyConnection`):** Phải thiết kế 1 class `MyConnection` dùng chung duy nhất thực hiện khai báo kết nối, mở/đóng kết nối và thực thi truy vấn. Mỗi lớp `DAO` gọi `MyConnection` để tái sử dụng (viết 1 lần, dùng nhiều nơi).
> 5. **Quy tắc 4 (Tối ưu truy xuất CSDL & `ArrayList` ở `BUS`):**
>    - Hạn chế truy xuất CSDL bừa bãi. Khi đọc danh sách lên `GUI`, dữ liệu phải được lưu vào **`ArrayList` (hoặc `List<DTO>`) trong lớp `BUS`**.
>    - **Không được chạy câu lệnh `SELECT` xuống CSDL liên tục mỗi khi tìm kiếm**, mà phải viết hàm tìm kiếm duyệt trên `ArrayList` trong lớp `BUS`.
>    - Các thao tác **Thêm (`INSERT`), Sửa (`UPDATE`), Xóa (`DELETE`)** thì bắt buộc phải chạy câu truy vấn xuống CSDL và đồng bộ lại `ArrayList`.
> 6. **Quy tắc 5 (Vị trí xử lý Nghiệp vụ & Tính toán):** Mọi kiểm tra tính hợp lệ, kiểm tra trùng mã (tính duy nhất), tính thành tiền, tổng tiền, tiền phạt, cộng/trừ số lượng tồn kho **bắt buộc phải viết ở lớp `BUS`** (không viết ở `GUI` hay `DAO`).

---

## III. TỔNG HỢP 16 YÊU CẦU KỸ THUẬT & GIAO DIỆN CỦA GIÁO VIÊN

Dưới đây là toàn bộ 16 yêu cầu từ file `C-w7-YeuCauDoAn.md`, `Version 1 viết trong DAO.md` và `Note.md` đã được ánh xạ cụ thể vào đề tài **Quản lý Thư viện**:

| STT | Yêu cầu của Giáo viên | Ánh xạ cụ thể vào Đồ án Quản lý Thư viện |
| :-: | :--- | :--- |
| **1** | **Mô hình 3-Layer ver 2 (`MyConnection`)** | Tách 4 thư mục `DTO`, `DAO`, `BUS`, `GUI`. Class `MyConnection` quản lý kết nối SQL Server tập trung. |
| **2** | **Tìm kiếm trên `ArrayList` viết trong `BUS`** | Mỗi class `BUS` giữ một `ArrayList` chứa các đối tượng `DTO`. Hàm tìm kiếm lọc trực tiếp trên `ArrayList` này. |
| **3** | **Form Main - Sub (Quan hệ 1 - N)** | Liên kết 2 bảng (2 `ListView` hoặc `DataGridView`) trên cùng 1 màn hình: Click 1 dòng ở bảng Cha $\rightarrow$ hiển thị $N$ dòng ở bảng Con. Áp dụng cho: <br>1. `PhieuMuon` (1) $\rightarrow$ `ChiTietPhieuMuon` (N)<br>2. `PhieuNhap` (1) $\rightarrow$ `ChiTietPhieuNhap` (N)<br>3. `PhieuPhat` (1) $\rightarrow$ `ChiTietPhieuPhat` (N)<br>4. `DauSach` (1) $\rightarrow$ `Sach` (N) |
| **4** | **Nút `...` hỗ trợ tìm kiếm & điền Form qua biến `static`** | Cạnh các ô nhập Mã khóa ngoại (`MaDG`, `MaDauSach`, `MaSach`, `MaTG`, `MaTL`, `MaNXB`, `MaNCC`), có nút `...` mở Form Popup tìm kiếm theo tên. Khi click chọn 1 dòng trên Popup và đóng lại, mã được truyền về qua **biến `static`** và tự động điền vào ô Mã kèm hiển thị Tên/Đơn giá. |
| **5** | **Kiểm tra tính duy nhất của dữ liệu (Unique ID)** | Viết hàm kiểm tra trùng mã (`MaDauSach`, `MaSach`, `MaDG`, `MaNV`, `MaPM`, `MaPN`, `MaPhieuPhat`...) tại lớp `BUS` trước khi thêm mới. |
| **6** | **Các field tự động thay đổi dữ liệu tồn kho** | Xử lý tại lớp `BUS` và cập nhật xuống CSDL:<br>- **Nhập sách (`PhieuNhap`):** Tăng `SoLuong` và `SLConLai` của `DauSach`, đồng thời sinh các cuốn `Sach` mới.<br>- **Mượn sách (`PhieuMuon`):** Giảm `SLConLai` đi 1, tăng `SLChoMuon` lên 1, đổi `Sach.TinhTrang` sang *"Đang mượn"*.<br>- **Trả sách:** Tăng `SLConLai` lên 1, giảm `SLChoMuon` đi 1, cập nhật `NgayThucTra`. |
| **7** | **Các field dữ liệu chỉ xem (`ReadOnly` / `Enabled = false`)** | Người dùng không được tự gõ sửa các trường tự động hoặc tra cứu như: `TenSach`, `DonGia` (khi hiển thị trên phiếu), `ThanhTien`, `TongTien`, `TongTienPhat`, `SoNgayTre`, `SLChoMuon`, `SLConLai`. |
| **8** | **Tìm kiếm nâng cao (Advanced Search)** | Hỗ trợ kết hợp các phép toán logic `AND`, `OR`, `NOT` và toán tử so sánh `>`, `<`, `>=`, `<=`, `<>` trên 1 hoặc nhiều trường.<br>*Ví dụ:* Tìm đầu sách có `DonGia >= 100000 AND DonGia <= 300000`, hoặc tìm độc giả thuộc loại `Sinh viên OR Giảng viên`, hoặc tìm phiếu nhập có `TongTien > 500000`. |
| **9** | **Bảng Tham Số (`ThamSo`)** | Tổ chức 1 bảng `ThamSo` gồm Mã tham số và Giá trị để lưu các hằng số cấu hình rời rạc của thư viện (VD: `SO_SACH_MUON_TOI_DA = 5`, `SO_NGAY_MUON_TOI_DA = 7`, `TIEN_PHAT_MOI_NGAY = 5000`, `TY_LE_DEN_MAT_SACH = 100`). |
| **10** | **Thống kê: Thu - Chi & 3 Bảng chéo theo Quý** | - **Bảng Thu - Chi tổng hợp:** Tổng thu (`PhieuPhat`), Tổng chi (`PhieuNhap`), Chênh lệch theo khoảng *Từ ngày $\rightarrow$ Đến ngày* và theo *Tháng / Quý / Năm*.<br>- **3 Bảng chéo (Pivot Quý 1, Quý 2, Quý 3, Quý 4, Tổng cộng - TC):**<br>  1. Theo **Nhân viên** × Quý (`Q1, Q2, Q3, Q4, TC`)<br>  2. Theo **Độc giả** × Quý (`Q1, Q2, Q3, Q4, TC`)<br>  3. Theo **Đầu sách** × Quý (`Q1, Q2, Q3, Q4, TC`) |
| **11** | **Export - Import file Excel (`.xls` / `.xlsx`) *(1 điểm)*** | Cho phép xuất danh sách (Sách, Độc giả, Phiếu nhập, Thống kê...) ra file Excel và đọc dữ liệu từ file Excel nạp vào chương trình. |
| **12** | **Xuất Report ra PDF *(1 điểm)*** | Cho phép in / xuất ra file PDF cho Phiếu mượn, Phiếu nhập, Phiếu phạt và Bảng báo cáo thống kê. |
| **13** | **Mẫu màn hình chính Desktop (Menu trái + Form con)** | Giao diện chính (`frmMain`) có thanh điều hướng bên trái (Left Sidebar Menu) và vùng Panel bên phải để nhúng các Form con. Màu sắc hài hòa, lịch sự, không lòe loẹt (chỉ dùng màu đỏ cho 1–2 nút cảnh báo/xóa). |
| **14** | **Báo cáo Word cuối kỳ** | Tối đa **20 trang**, font **Times New Roman cỡ 13**, **không có trang mục lục**, đầy đủ trang bìa và 5 phần theo mẫu của cô. |
| **15** | **Hỗ trợ lưu trữ Tiếng Việt (UTF-8 / Unicode)** | Sử dụng kiểu dữ liệu `NVARCHAR` trong SQL Server và tiền tố `N'...'` hoặc `SqlParameter` kiểu `SqlDbType.NVarChar` để không bao giờ bị lỗi font chữ tiếng Việt. |
| **16** | **Trang Web ASP.NET Core / Blazor *(Bổ sung từ `Note.md`)*** | Làm tối thiểu 1 trang web quản lý/tra cứu đối tượng trong đồ án (chú ý cú pháp `@` trong Razor/Blazor). Làm thêm trang đặt mượn sách / admin duyệt sẽ được cộng điểm. |

---

## IV. DANH SÁCH 14 NHÓM CHỨC NĂNG NGHIỆP VỤ & 17 BẢNG CSDL

### 1. Danh sách 17 Bảng Cơ sở dữ liệu (16 bảng trong `QLThuVien1.sql` + 1 bảng `ThamSo` bổ sung)

| Nhóm | STT | Tên Bảng | Khóa chính (PK) | Các cột chính & Khóa ngoại (FK) |
| :--- | :-: | :--- | :--- | :--- |
| **Danh mục nền (8 bảng)** | 1 | `NhaXuatBan` | `MaNXB` | `TenNXB`, `DiaChi` |
| | 2 | `TacGia` | `MaTG` | `HoLot`, `TenTG`, `NgaySinh`, `SDT` |
| | 3 | `TheLoai` | `MaTL` | `TenTL` |
| | 4 | `NhaCungCap` | `MaNCC` | `TenNCC` |
| | 5 | `NhanVien` | `MaNV` | `HoLot`, `TenNV`, `ChucVu` |
| | 6 | `LoaiDocGia` | `MaLDG` | `TenLDG` |
| | 7 | `DocGia` | `MaDG` | `HoLot`, `TenDG`, `NgaySinh`, `SDT`, `MaLDG` (FK $\rightarrow$ `LoaiDocGia`) |
| | 8 | `QuyDinhPhat` | `MaQD` | `TenQD`, `LoaiPhat`, `CachTinh`, `DonViTinh`, `MucPhat` ($\ge 0$) |
| **Ấn phẩm (2 bảng)** | 9 | `DauSach` | `MaDauSach` | `MaNXB` (FK), `MaTG` (FK), `MaTL` (FK), `TenSach`, `NamXB` ($\le$ năm hiện tại), `SoLuong`, `SLChoMuon`, `SLConLai` (`SoLuong = SLChoMuon + SLConLai`), `DonGia` ($\ge 0$) |
| | 10 | `Sach` | `MaSach` | `MaDauSach` (FK $\rightarrow$ `DauSach`), `TinhTrang` (*Mới/Tốt/Đang mượn/Hư hỏng/Mất*), `ViTri` |
| **Giao dịch 1-N (6 bảng)** | 11 | `PhieuNhap` | `MaPN` | `MaNCC` (FK), `MaNV` (FK), `NgayNhap`, `TongTien` ($\ge 0$) |
| | 12 | `ChiTietPhieuNhap` | `(MaPN, MaDauSach)` | `DonGia` ($\ge 0$), `SoLuong` ($> 0$), `ThanhTien` (`= DonGia * SoLuong`) |
| | 13 | `PhieuMuon` | `MaPM` | `MaDocGia` (FK), `MaNV` (FK), `NgayMuon`, `HanTra` ($\ge$ `NgayMuon`) |
| | 14 | `ChiTietPhieuMuon` | `(MaPhieuMuon, MaSach)` | `TinhTrang`, `NgayThucTra` ($\ge$ `NgayMuon` hoặc `NULL` khi chưa trả) |
| | 15 | `PhieuPhat` | `MaPhieuPhat` | `MaDocGia` (FK), `MaPhieuMuon` (FK), `MaNV` (FK), `NgayLap`, `TrangThai`, `TongTienPhat` ($\ge 0$) |
| | 16 | `ChiTietPhieuPhat` | `(MaPhieuPhat, MaQD, MaSach)`* | `DonGia`, `SoNgayTre` ($\ge 0$), `GhiChu`, `TienPhat` ($\ge 0$) |
| **Cấu hình (1 bảng mới)** | 17 | `ThamSo` *(Mới)* | `MaTS` | `TenTS`, `GiaTri` *(Đáp ứng yêu cầu số 9 của Giáo viên)* |

---

## V. 🔥 HIGHLIGHT CÁC ĐIỂM MÂU THUẪN GIỮA CÁC TÀI LIỆU & PHƯƠNG ÁN ĐÃ CHỐT

Trong quá trình phân tích chéo toàn bộ tài liệu trong `requirement_input/`, có **7 điểm mâu thuẫn / bất cập** rất quan trọng dưới đây. Bạn cần nắm rõ để thống nhất với nhóm và trả lời giáo viên:

> [!WARNING]
> ### 🔴 MÂU THUẪN 1: Hệ quản trị CSDL (MySQL trong bài giảng của Cô vs. SQL Server trong file của nhóm)
> - **Chi tiết mâu thuẫn:**
>   - Trong file slide `c-3-layer-.md` và `Version 1 viết trong DAO.md` (Mục 15), giáo viên hướng dẫn cài gói `MySql.Data` / `MySqlConnector` và dặn nối thêm chuỗi `"?useUnicode=yes&characterEncoding=UTF-8"` (đây là cú pháp riêng của MySQL).
>   - Ngược lại, file `QLThuVien1.sql` và `RBTV.md` của bạn cùng nhóm lại viết hoàn toàn bằng **T-SQL cho SQL Server Management Studio (SSMS)** (sử dụng lệnh `GO`, `NVARCHAR`, `GETDATE()`, `RAISERROR`).
> - **Phương án đã chốt:**
>   - Sử dụng **SQL Server (trên SSMS)** và thư viện `Microsoft.Data.SqlClient` trong C# để tận dụng toàn bộ 16 bảng và dữ liệu mẫu nhóm đã làm.
>   - Trong SQL Server, kiểu `NVARCHAR` đã hỗ trợ sẵn Unicode tiếng Việt mà không cần chuỗi `?useUnicode=yes`.
>   - **Lưu ý hành động:** Khi đi học trên lớp, bạn nên hỏi xác nhận nhanh với cô Trang: *"Thưa cô, nhóm em dùng SQL Server (SSMS) thay cho MySQL được không ạ?"*.

> [!CAUTION]
> ### 🔴 MÂU THUẪN 2: Xử lý tồn kho ở Trigger SQL vs. Bắt buộc viết ở lớp BUS (Nguy cơ cộng/trừ 2 lần!)
> - **Chi tiết mâu thuẫn:**
>   - Trong `QLThuVien1.sql` (dòng 190–239), bạn của bạn đã tạo 2 Trigger là `trg_ChiTietPhieuMuon_Insert` và `trg_ChiTietPhieuMuon_UpdateTraSach` để tự động cộng/trừ `SLChoMuon` và `SLConLai` trong CSDL khi mượn/trả. Tuy nhiên lại **không hề có Trigger cho Phiếu Nhập sách**!
>   - Trong khi đó, giáo viên quy định rất gắt tại `Version 1 viết trong DAO.md` (Mục 5 & 6): **"Nghiệp vụ viết ở lớp BUS để gọi hàm đó bất cứ lúc nào (không được viết ở lớp GUI và DAO, tính toán xử lý dữ liệu tính ở lớp BUS hết)"**. Khi kiểm tra giấy 10 phút hoặc vấn đáp, cô sẽ chấm hàm C# trong lớp `BUS`.
>   - **Nguy hiểm:** Nếu vừa giữ Trigger trong SQL Server vừa viết hàm cộng/trừ tồn kho trong lớp `BUS`, mỗi lần mượn 1 cuốn sách hệ thống sẽ bị **trừ tồn kho 2 lần** (`SLConLai` giảm 2)!
> - **Phương án đã chốt:**
>   - **Xóa 2 Trigger trong SQL Server** (bằng lệnh `DROP TRIGGER` trên SSMS) và **chuyển 100% logic kiểm tra điều kiện, cộng/trừ tồn kho (Nhập sách, Mượn sách, Trả sách) và tính toán thành tiền/tổng tiền/tiền phạt vào lớp `BUS`**. Cách này vừa tránh lỗi trừ 2 lần, vừa có đầy đủ code C# trong lớp `BUS` để trình bày khi cô hỏi.

> [!IMPORTANT]
> ### 🟡 MÂU THUẪN 3: Thiếu "Bảng Tham Số" (`ThamSo`) trong CSDL hiện tại
> - **Chi tiết mâu thuẫn:**
>   - Yêu cầu số 9 của giáo viên (`C-w7-YeuCauDoAn.md` & `Version 1 viết trong DAO.md`) bắt buộc phải có **1 Table Tham Số** (chứa Mã tham số và Giá trị) lưu các hằng số rời rạc.
>   - Trong 16 bảng của `QLThuVien1.sql` và sơ đồ `ERD.drawio.xml` **chưa có bảng này**.
> - **Phương án đã chốt:**
>   - Bổ sung bảng thứ 17 là **`ThamSo (MaTS VARCHAR(50) PRIMARY KEY, TenTS NVARCHAR(100), GiaTri NVARCHAR(100))`** vào CSDL trên SSMS.

> [!WARNING]
> ### 🟡 MÂU THUẪN 4: Cột tự tính `ThanhTien` của `ChiTietPhieuNhap` & Khóa chính của `ChiTietPhieuPhat`
> - **Chi tiết mâu thuẫn:**
>   1. Trong `QLThuVien1.sql` (dòng 126), cột `ThanhTien` của bảng `ChiTietPhieuNhap` được khai báo là `AS (DonGia * SoLuong) PERSISTED` (cột tự tính của SQL). Điều này khiến lớp `BUS` không thể tự tính `ThanhTien` rồi `INSERT` xuống `DAO` như yêu cầu thuyết minh công thức tính của cô.
>   2. Trong `ChiTietPhieuPhat` (dòng 178), khóa chính đang là `PRIMARY KEY (MaPhieuPhat, MaQD)`. Nếu trong 1 lần trả sách, độc giả làm hỏng **2 cuốn sách khác nhau** (cùng mã quy định `QD02`), CSDL sẽ báo lỗi trùng khóa chính `(MaPhieuPhat, MaQD)`!
> - **Phương án đã chốt:**
>   - Chạy script cập nhật nhỏ trên SSMS để đổi `ChiTietPhieuNhap.ThanhTien` thành cột `DECIMAL(18, 2)` bình thường (tính ở `BUS` rồi truyền xuống `DAO`), và cập nhật bảng `ChiTietPhieuPhat` để cho phép lưu nhiều dòng phạt trong cùng một phiếu phạt.

> [!NOTE]
> ### 🔵 MÂU THUẪN 5: Đề bài Thống kê viết theo bài toán Bán hàng vs. Đề tài Quản lý Thư viện
> - **Chi tiết mâu thuẫn:**
>   - Trong `C-w7-YeuCauDoAn.md` (Mục 10), cô viết mẫu thống kê theo bài toán Bán hàng: *Tổng thu (hóa đơn), Tổng chi (phiếu nhập), Lợi nhuận; và 3 bảng Quý (Q1–Q4, TC) theo Nhân viên, Khách hàng, Sản phẩm*.
>   - Nhưng đề tài nhóm là **Quản lý Thư viện** (không có bảng Hóa đơn bán hàng, Khách hàng hay Sản phẩm).
> - **Phương án đã chốt (Ánh xạ chuẩn 1-1 sang Thư viện):**
>   - **Tổng thu** = Tổng tiền phạt thu được (`PhieuPhat.TongTienPhat`); **Tổng chi** = Tổng tiền nhập sách (`PhieuNhap.TongTien`); **Chênh lệch Thu - Chi** (lọc từ ngày $\rightarrow$ đến ngày, theo Tháng/Quý/Năm).
>   - **Bảng Quý 1 (Q1, Q2, Q3, Q4, TC theo Nhân viên):** Thống kê tổng tiền phiếu nhập / phiếu phạt theo từng `NhanVien`.
>   - **Bảng Quý 2 (Q1, Q2, Q3, Q4, TC theo Độc giả — thay Khách hàng):** Thống kê tổng tiền phạt (hoặc số sách mượn) theo từng `DocGia`.
>   - **Bảng Quý 3 (Q1, Q2, Q3, Q4, TC theo Đầu sách — thay Sản phẩm):** Thống kê tổng tiền nhập sách (hoặc số lượt mượn) theo từng `DauSach`.

> [!NOTE]
> ### 🔵 MÂU THUẪN 6: Quan hệ Đầu sách - Tác giả trong mô tả chức năng vs. ERD/SQL
> - **Chi tiết mâu thuẫn:**
>   - Trong `Chuc_nang_quan_ly_thu_vien.md` (Mục 1) ghi *"Gán và quản lý tác giả của đầu sách"*, nhưng trong `ERD.drawio.xml` và `QLThuVien1.sql`, bảng `DauSach` chỉ có 1 cột khóa ngoại `MaTG` (quan hệ 1 Tác giả – Nhiều Đầu sách).
>   - Ngoài ra trong `ERD.drawio.xml` có 2 lỗi chính tả nhỏ: `PhieuNhap.TongTienn` (dư chữ n), `PhieuPhat.TongTien` (trong SQL là `TongTienPhat`) và `ChiTietPheuPhat` (thiếu chữ i).
> - **Phương án đã chốt:**
>   - Giữ nguyên quan hệ 1-N (`MaTG` nằm trong `DauSach`) và tên cột chuẩn như trong file `QLThuVien1.sql`. Khi chụp hình ERD đưa vào báo cáo Word thì sửa lại 3 chữ bị gõ nhầm trên sơ đồ cho khớp 100% với SQL.

> [!NOTE]
> ### 🔵 MÂU THUẪN 7: Yêu cầu WinForms Desktop vs. Yêu cầu Web ASP.NET Core / Blazor
> - **Chi tiết mâu thuẫn:**
>   - File `C-w7-YeuCauDoAn.md` và `Chuc_nang_quan_ly_thu_vien.md` mô tả ứng dụng WinForms Desktop, trong khi `Note.md` ghi thêm yêu cầu làm tối thiểu 1 trang Web `ASP.NET Core / Blazor`.
> - **Phương án đã chốt:**
>   - Xây dựng hoàn chỉnh ứng dụng **WinForms Desktop** làm sản phẩm chính để chấm toàn bộ 16 mục yêu cầu. Sau đó tạo thêm 1 project **ASP.NET Core Blazor** nhỏ trong cùng Solution (gọi lại chính các lớp `BUS`, `DAO`, `DTO` đã viết) để đáp ứng yêu cầu trang Web và lấy điểm cộng.

---

## VI. YÊU CẦU BÁO CÁO WORD CUỐI KỲ (CHUẨN BỊ CHO TUẦN 11–12)

- **Hình thức:** Tối đa **20 trang**, font **Times New Roman**, size **13**, **KHÔNG có trang mục lục**.
- **Trang bìa:**
  1. Đồ án học phần Lập trình C#
  2. Tên đề tài: Phần mềm Quản lý Thư viện
  3. Thông tin nhóm sinh viên, lớp
  4. GVHD: Phùng Thái Thiên Trang
  5. Dòng cuối: Tháng 12/2026
- **Nội dung 5 phần bắt buộc:**
  - **I. Giới thiệu đề tài.**
  - **II. Thiết kế mô hình CSDL:** Chụp màn hình sơ đồ CSDL (ảnh phải rõ nét).
  - **III. Các chức năng chính:** Liệt kê 1, 2, 3..., các chức năng con a, b, c... Mỗi chức năng trình bày:
    - a. Giới thiệu chức năng.
    - b. Chụp màn hình Form, chú thích các thành phần trên Form.
    - c. Thuyết minh công thức tính toán: thành tiền, tổng tiền, tiền phạt, số lượng tồn...
    - d. Mỗi hàm (chức năng): xác định Đầu vào (Input), Đầu ra (Output), xử lý tính toán.
  - **IV. Hướng dẫn cài đặt đồ án:**
    - a. Chụp màn hình cây thư mục chứa source code, thấy rõ phân lớp `GUI - BUS - DAO - DTO`.
    - b. Cách kết nối CSDL.
    - c. Cách cài đặt và link download các tool hoặc thư viện sử dụng trong đồ án.
  - **V. Sản phẩm đồ án:** Link chứa source code (GitHub/Drive) và file báo cáo đồ án (DOC/PDF).
