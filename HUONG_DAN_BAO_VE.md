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

## PHẦN 2: ÔN TẬP GÓI ĐIỂM CÁ NHÂN — GÓI 3 (GÓI 50% ĐIỂM ĐỒ ÁN)

> **Mục tiêu Gói 3 (50% điểm):** Cover được **3 đối tượng** thuộc tầng **`BUS` và `DAO`** (bỏ `GUI`, không cần hàm Thống kê); cụ thể là viết/giải thích được các hàm **Đọc danh sách, Thêm, Sửa, Xóa, Tìm kiếm** của `BUS` và `DAO`.

### 1. Chọn sẵn 3 đối tượng dễ nhất, sạch nhất để ôn & gõ tay
1. **`TheLoai`** (`TheLoaiDAO.cs` + `TheLoaiBUS.cs`) — ít cột nhất (`MaTL`, `TenTL`).
2. **`NhaXuatBan`** (`NXBDAO.cs` + `NXBBUS.cs`) — 3 cột (`MaNXB`, `TenNXB`, `DiaChi`).
3. **`TacGia`** (`TacGiaDAO.cs` + `TacGiaBUS.cs`) — 5 cột (`MaTG`, `HoLot`, `TenTG`, `NgaySinh`, `SDT`).
*(Cả 3 đối tượng này đều có cấu trúc `DAO` và `BUS` giống hệt nhau, chỉ cần học thuộc khuôn mẫu của `TheLoai` là suy ra được cả 3!)*

---

### 2. Code mẫu chuẩn tầng `DAO` (Viết bằng `MyConnection` + `SqlParameter[]`)
```csharp
// 1. ĐỌC DANH SÁCH (SELECT)
public List<TheLoaiDTO> DocDSTL()
{
    List<TheLoaiDTO> dsTL = new List<TheLoaiDTO>();
    string query = "SELECT * FROM TheLoai";
    using (SqlDataReader reader = MyConnection.ExecuteReader(query))
    {
        while (reader.Read())
        {
            dsTL.Add(new TheLoaiDTO
            {
                MaTL = reader["MaTL"].ToString(),
                TenTL = reader["TenTL"].ToString()
            });
        }
    }
    return dsTL;
}

// 2. THÊM (INSERT)
public void Them(TheLoaiDTO tl)
{
    const string query = "INSERT INTO TheLoai (MaTL, TenTL) VALUES (@MaTL, @TenTL)";
    SqlParameter[] parameters =
    {
        new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = tl.MaTL },
        new SqlParameter("@TenTL", SqlDbType.NVarChar) { Value = tl.TenTL }
    };
    MyConnection.ExecuteNonQuery(query, parameters);
}

// 3. SỬA (UPDATE)
public void Sua(TheLoaiDTO tl)
{
    const string query = "UPDATE TheLoai SET TenTL = @TenTL WHERE MaTL = @MaTL";
    SqlParameter[] parameters =
    {
        new SqlParameter("@TenTL", SqlDbType.NVarChar) { Value = tl.TenTL },
        new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = tl.MaTL }
    };
    if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
        throw new InvalidOperationException("Không tìm thấy thể loại cần cập nhật.");
}

// 4. XÓA (DELETE) - Bắt lỗi khóa ngoại SQL Error 547
public void Xoa(string maTL)
{
    try
    {
        const string query = "DELETE FROM TheLoai WHERE MaTL = @MaTL";
        SqlParameter[] parameters = { new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = maTL } };
        if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
            throw new InvalidOperationException("Không tìm thấy thể loại cần xóa.");
    }
    catch (SqlException ex) when (ex.Number == 547)
    {
        throw new InvalidOperationException("Không thể xóa vì đang được dùng trong Đầu sách.", ex);
    }
}
```

---

### 3. Code mẫu chuẩn tầng `BUS` (`ArrayList`, kiểm tra trùng mã, Thêm/Sửa/Xóa, Tìm kiếm)
```csharp
private readonly ArrayList danhSachTheLoai = new ArrayList();

// 1. Đọc danh sách từ DAO và nạp vào ArrayList trên RAM
public List<TheLoaiDTO> DocDSTL()
{
    TheLoaiDAO dao = new TheLoaiDAO();
    List<TheLoaiDTO> ds = dao.DocDSTL();
    danhSachTheLoai.Clear();
    danhSachTheLoai.AddRange(ds);
    return ds;
}

// 2. Kiểm tra trùng mã (Duyệt trên ArrayList, KHÔNG query SQL)
public bool KiemTraTrungMa(string maTL)
{
    foreach (TheLoaiDTO item in danhSachTheLoai)
    {
        if (item.MaTL.Equals(maTL.Trim(), StringComparison.OrdinalIgnoreCase))
            return true;
    }
    return false;
}

// 3. Thêm (Kiểm tra rỗng + trùng mã -> Gọi DAO -> Nạp lại RAM)
public void Them(TheLoaiDTO tl)
{
    if (string.IsNullOrWhiteSpace(tl.MaTL) || string.IsNullOrWhiteSpace(tl.TenTL))
        throw new ArgumentException("Mã và tên thể loại không được để trống.");
    if (KiemTraTrungMa(tl.MaTL))
        throw new InvalidOperationException($"Mã thể loại '{tl.MaTL}' đã tồn tại.");
    new TheLoaiDAO().Them(tl);
    DocDSTL();
}

// 4. Sửa & Xóa
public void Sua(TheLoaiDTO tl)
{
    if (string.IsNullOrWhiteSpace(tl.MaTL) || string.IsNullOrWhiteSpace(tl.TenTL))
        throw new ArgumentException("Mã và tên thể loại không được để trống.");
    new TheLoaiDAO().Sua(tl);
    DocDSTL();
}

public void Xoa(string maTL)
{
    new TheLoaiDAO().Xoa(maTL);
    DocDSTL();
}

// 5. Tìm kiếm trên ArrayList (KHÔNG chạy SELECT xuống CSDL)
public List<TheLoaiDTO> timKiem(string maTheLoai, string tenTheLoai)
{
    List<TheLoaiDTO> ketQua = new List<TheLoaiDTO>();
    foreach (TheLoaiDTO tl in danhSachTheLoai)
    {
        if ((!string.IsNullOrEmpty(maTheLoai) && tl.MaTL.Contains(maTheLoai, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(tenTheLoai) && tl.TenTL.Contains(tenTheLoai, StringComparison.OrdinalIgnoreCase)))
        {
            ketQua.Add(tl);
        }
    }
    return ketQua;
}
```

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
