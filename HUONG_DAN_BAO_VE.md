# HƯỚNG DẪN CHẠY DEMO, BẢO VỆ ĐỒ ÁN & ÔN TẬP GÓI ĐIỂM CÁ NHÂN

Tài liệu này bám sát **Mục VII — Quy trình chấm đồ án & Gói điểm cá nhân** trong [YEU_CAU_DO_AN.md](file:///d:/code/C%23/TestProject/YEU_CAU_DO_AN.md).

---

## PHẦN 1: KỊCH BẢN CHẠY DEMO ĐỒ ÁN (BƯỚC 1 — CHẤM CHUNG NHÓM)

> **Lưu ý thang điểm:** Bắt đầu từ **12 điểm**, cứ **1 lỗi bị trừ 1 điểm**. Không có Form Main-Sub (-1đ), không có Thống kê (-2đ), Database lủng (0đ).

### 1.1. Chuẩn bị trước khi lên bàn chấm
1. Đảm bảo dịch vụ **SQL Server (`MSSQLSERVER`)** đang chạy (`Running`).
2. Chạy lại script `SQL/QLThuVien1.sql` (nếu cần reset dữ liệu mẫu sạch đẹp).
3. Chạy sẵn cả 2 project để chuyển đổi mượt mà:
   - WinForms Desktop: `dotnet run --project src/QLThuVien/QLThuVien.csproj`
   - Web Blazor: `dotnet run --project src/QLThuVien.Web/QLThuVien.Web.csproj`

### 1.2. Kịch bản Demo từng bước (Test cả trường hợp Đúng & Sai)
1. **Kiểm tra Danh mục & Thêm/Sửa/Xóa + Ràng buộc:**
   - Mở **Thể loại** (`frmTheLoai`):
     - *Test lỗi trùng mã:* Nhập `TL01` $\rightarrow$ bấm **Thêm** $\rightarrow$ Phần mềm báo lỗi mã đã tồn tại.
     - *Test thêm mới:* Nhập `TL04` / `Kỹ năng sống` $\rightarrow$ bấm **Thêm** $\rightarrow$ Thành công.
     - *Test tìm kiếm:* Gõ `Công nghệ` (ra kết quả đẹp) và gõ `xyz999` (báo không có / lưới rỗng).
     - *Test khóa ngoại khi xóa:* Chọn `TL01` bấm **Xóa** $\rightarrow$ Hệ thống chặn xóa vì đang có Đầu sách tham chiếu.
2. **Kiểm tra Đầu sách + Nút `...` + Tìm kiếm nâng cao + Excel (`frmDauSach`):**
   - Nhập đầu sách mới `DS04`, bấm 3 nút `...` (`Tác giả`, `Thể loại`, `NXB`), gõ tên vào ô lọc nhanh và nhấp đúp để tự điền mã.
   - Bấm **Tìm nâng cao** (`frmTimKiemPopup`): chọn `AND`, `Đơn giá >= 150000` $\rightarrow$ Lọc ra các đầu sách thỏa điều kiện trên `ArrayList`.
   - Click vào từng dòng `DauSach` $\rightarrow$ Bảng **Sách** bên phải tự cập nhật các cuốn sách tương ứng (Form Main - Sub 1-N).
   - Bấm **Xuất Excel** lưu file `.xlsx`.
3. **Kiểm tra Nhập sách (`frmPhieuNhap`) $\rightarrow$ Tồn kho TĂNG:**
   - Nhớ số lượng hiện tại của `DS01` (VD: `SoLuong = 3, SLConLai = 2`).
   - Bấm **Tạo phiếu mới**, chọn Nhà cung cấp (`...`), Nhân viên (`...`), chọn Đầu sách `DS01` (`...`), nhập `SoLuong = 2` $\rightarrow$ `ThanhTien` và `TongTien` tự động tính.
   - Bấm **Lưu phiếu nhập** $\rightarrow$ Quay lại `frmDauSach` kiểm tra `DS01`: `SoLuong` đã tăng lên `5`, `SLConLai` tăng lên `4`, và bên phải tự sinh thêm 2 cuốn sách mới (`S01_04`, `S01_05`)!
   - Bấm **Xuất PDF** xem phiếu nhập.
4. **Kiểm tra Mượn sách (`frmMuonTraSach`) $\rightarrow$ Tồn kho GIẢM:**
   - Chọn Độc giả (`...`), Nhân viên, bấm **Thêm phiếu**.
   - Chọn cuốn sách có sẵn thuộc `DS01` $\rightarrow$ bấm **Thêm sách** $\rightarrow$ Quay lại `frmDauSach`: `SLChoMuon` tăng 1, `SLConLai` giảm 1.
5. **Kiểm tra Trả sách vi phạm $\rightarrow$ Lập Phiếu Phạt (`frmPhieuPhat`):**
   - Trên `frmMuonTraSach`, chọn phiếu `PM02` (có `HanTra = 2026-11-08`, đã quá hạn so với hiện tại), bấm **Trả sách** $\rightarrow$ Hệ thống thông báo số ngày trễ và hỏi mở nhanh sang **Phiếu Phạt**.
   - Trên `frmPhieuPhat`: Thêm chi tiết vi phạm `QD01` (trễ hạn) hoặc `QD02` (hư hỏng) hoặc `QD03` (mất sách = 100% giá bìa) $\rightarrow$ `TienPhat` và `TongTienPhat` tự tính chính xác.
   - Bấm **Xuất PDF** in phiếu phạt.
6. **Kiểm tra Tham số (`frmThamSo`) & Thống kê (`frmThongKe`):**
   - Trên `frmThamSo`: Đổi `TIEN_PHAT_MOI_NGAY` $\rightarrow$ mở `frmQuyDinhPhat` thấy `QD01` đã tự đồng bộ theo.
   - Trên `frmThongKe`: Xem Tab **Thu - Chi** (Tổng thu, Tổng chi, Chênh lệch) và Tab **Thống kê quý** (3 bảng chéo Nhân viên / Độc giả / Đầu sách theo `Q1..Q4`). Bấm **Xuất PDF** và **Xuất Excel**.

---

## PHẦN 2: HƯỚNG DẪN CHỌN & ÔN TẬP 3 GÓI ĐIỂM CÁ NHÂN (BƯỚC 2)

### Gói 1 (100% điểm) — Phải tự gõ được 1–2 hàm (gồm 1 hàm Thống kê + 1 hàm đơn giản)
**1. Hàm Thống kê theo năm/quý mẫu (`ThongKeBUS.cs`):**
```csharp
public List<ThongKeDocGiaQuyDTO> LayThongKeDocGiaTheoQuy(int nam)
{
    DateTime tuNgay = new DateTime(nam, 1, 1);
    DateTime denNgay = new DateTime(nam, 12, 31);
    List<ThongKePhieuPhatDTO> phieuPhat = dao.LayPhieuPhat(tuNgay, denNgay);

    return busDocGia.DocDSDocGia().Cast<DocGiaDTO>()
        .Select(dg =>
        {
            var ds = phieuPhat.Where(x => x.MaDocGia.Equals(dg.MaDG, StringComparison.OrdinalIgnoreCase)).ToList();
            return new ThongKeDocGiaQuyDTO
            {
                MaDocGia = dg.MaDG,
                TenDocGia = dg.HoTen,
                TienPhatQ1 = ds.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == 1).Sum(x => x.TongTienPhat),
                TienPhatQ2 = ds.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == 2).Sum(x => x.TongTienPhat),
                TienPhatQ3 = ds.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == 3).Sum(x => x.TongTienPhat),
                TienPhatQ4 = ds.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == 4).Sum(x => x.TongTienPhat),
                TongCong = ds.Sum(x => x.TongTienPhat)
            };
        }).ToList();
}
```
**2. Hàm đơn giản mẫu (Tìm kiếm trên `ArrayList` trong `BUS`):**
```csharp
public ArrayList TimKiem(string tuKhoa)
{
    ArrayList ketQua = new ArrayList();
    string kw = (tuKhoa ?? "").Trim().ToLower();
    foreach (DocGiaDTO dg in dsDocGia_arr)
    {
        if (dg.MaDG.ToLower().Contains(kw) || dg.HoTen.ToLower().Contains(kw))
            ketQua.Add(dg);
    }
    return ketQua;
}
```

### Gói 2 (70% điểm) — Nắm chắc 3 tầng `GUI - BUS - DAO` của Phiếu mượn / Phiếu phạt, Phiếu nhập & Thống kê
- **Phiếu nhập (`PhieuNhapBUS.cs` / `PhieuNhapDAO.cs` / `frmPhieuNhap.cs`):** Công thức `ThanhTien = SoLuong * DonGia`, `TongTien = SUM(ThanhTien)`, tự động sinh cuốn sách `Sach` và tăng `DauSach.SoLuong`, `DauSach.SLConLai`.
- **Phiếu mượn (`PhieuMuonBUS.cs` / `PhieuMuonDAO.cs` / `frmMuonTraSach.cs`):** Kiểm tra `SO_SACH_MUON_TOI_DA` từ `ThamSo`, trừ `SLConLai` / tăng `SLChoMuon` khi mượn, hoàn trả khi trả sách (hoặc trừ `SoLuong` nếu báo mất).
- **Phiếu phạt (`PhieuPhatBUS.cs` / `PhieuPhatDAO.cs` / `frmPhieuPhat.cs`):** Công thức tính phạt `QD01` (ngày trễ $\times$ mức phạt), `QD02` (cố định), `QD03` (giá bìa $\times$ hệ số).

### Gói 3 (50% điểm) — Cover 3 đối tượng tầng `BUS` và `DAO` (Thêm, Sửa, Xóa, Tìm kiếm)
- Chọn 3 đối tượng danh mục chuẩn sạch: `TheLoai`, `TacGia`, `NhaXuatBan` (hoặc `NhanVien`, `NhaCungCap`, `LoaiDocGia`).
- Nắm vững: `MyConnection.ExecuteQuery` / `ExecuteNonQuery` + `SqlParameter[]` trong `DAO`; kiểm tra rỗng + kiểm tra trùng mã trên `ArrayList` + chặn xóa khóa ngoại trong `BUS`.

---

## PHẦN 3: HƯỚNG DẪN VẤN ĐÁP WEB BLAZOR (5 PHÚT — CỘNG 1 ĐIỂM)

1. **Giải thích các cú pháp Razor (`@`) cô hay hỏi trong file `.razor`:**
   - `@page "/quan-ly-the-loai"`: Khai báo đường dẫn URL để truy cập trang này trên trình duyệt.
   - `@rendermode InteractiveServer`: Kích hoạt chế độ tương tác thời gian thực qua SignalR để các sự kiện `@onclick`, `@bind` hoạt động mà không cần tải lại trang.
   - `@inject ThuVienWebService WebService`: Tiêm dịch vụ kết nối SQL Server (đã đăng ký trong `Program.cs`) vào trang Razor để gọi các hàm lấy/lưu dữ liệu.
   - `@bind="formModel.TenTL"`: Ràng buộc dữ liệu hai chiều (Two-way binding) giữa ô `<input>` trên HTML và biến C# trong `@code { ... }`.
   - `@foreach (var item in danhSach) { ... }`: Vòng lặp C# duyệt danh sách kết quả để sinh các dòng `<tr>` trong bảng HTML.
   - `@onclick="ThemMoi"` hoặc `@onclick="() => Xoa(item.MaTL)"`: Gắn sự kiện click nút HTML vào hàm xử lý C#.
2. **Cách mỗi thành viên chạy/trình bày phần Web cá nhân:**
   - `QLThuVien.Web` đã tích hợp sẵn **5 trang độc lập** tương ứng với từng phần việc:
     - **TV1:** `Home.razor` (Tra cứu) + `QuanLyDauSach.razor` (CRUD Đầu sách).
     - **TV2:** `DatMuonSach.razor` (Đặt mượn sách) + `DuyetMuonSach.razor` (Admin duyệt lập `PhieuMuon` & trừ tồn kho).
     - **TV3:** `QuanLyTheLoai.razor` (CRUD Thể loại) hoặc nhân bản sang `QuanLyNhaCungCap`.
     - **TV4:** Mở rộng tương tự cho `NhanVien` / `QuyDinhPhat` nếu cần tách thành 4 project riêng khi chấm cá nhân.
