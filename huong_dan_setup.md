# Hướng Dẫn Setup Môi Trường Chạy Đồ Án Quản Lý Thư Viện

Tài liệu này hướng dẫn cách dựng lại SQL Server bằng Docker và kết nối SSMS trên máy mới để chạy thẳng code mà **không cần sửa chuỗi kết nối**.

---

## Bước 1: Mở Docker Desktop
1. Mở ứng dụng **Docker Desktop** trên Windows và chờ đến khi trạng thái báo **Engine running** (biểu tượng cá voi ở góc dưới bên trái chuyển sang màu xanh).
2. *(Khuyên dùng)* Vào **Settings → General**, tích chọn **Start Docker Desktop when you sign in to your computer** để mỗi lần bật máy Docker tự chạy.

---

## Bước 2: Tạo container SQL Server bằng Terminal (PowerShell)
Mở **PowerShell** (hoặc Terminal trong VS Code) và chạy lệnh sau (chỉ cần chạy **1 lần duy nhất**):

```powershell
docker run -d `
  --name sql-csharp `
  --restart unless-stopped `
  -e "ACCEPT_EULA=Y" `
  -e "MSSQL_SA_PASSWORD=987654321SSaa" `
  -p 1434:1433 `
  -v sql-csharp-data:/var/opt/mssql `
  mcr.microsoft.com/mssql/server:2022-latest
```

**Giải thích nhanh:**
- `--name sql-csharp`: Tên container.
- `--restart unless-stopped`: Tự động bật lại container mỗi khi mở lại máy/Docker.
- `-p 1434:1433`: Mở cổng `1434` trên Windows (khớp với `Server=localhost,1434` trong code).
- `-v sql-csharp-data:/var/opt/mssql`: Lưu dữ liệu database vào volume riêng để không bị mất khi tắt máy.

Kiểm tra container đang chạy:
```powershell
docker ps
```
Thấy `sql-csharp` có trạng thái `Up` và `0.0.0.0:1434->1433/tcp` là thành công.

---

## Bước 3: Mở SSMS và tạo kết nối
1. Mở **SQL Server Management Studio (SSMS)**.
2. Ở cửa sổ **Connect to Server**, điền đúng thông tin sau:
   - **Server type:** `Database Engine`
   - **Server name:** `localhost,1434` *(lưu ý dùng **dấu phẩy `,`**, không dùng dấu hai chấm `:`)*
   - **Authentication:** `SQL Server Authentication`
   - **Login:** `sa`
   - **Password:** `987654321SSaa`
   - **Trust server certificate:** Tích chọn *(nếu có)*
3. Nhấn **Connect**.

---

## Bước 4: Nạp Database và Dữ liệu mẫu
Trong SSMS, mở và chạy lần lượt **2 file SQL** trong thư mục `SQL/` của dự án:

1. Chạy file `SQL/QLThuVien1.sql` (nhấn **Execute** / `F5`) → Tạo database `QLThuVien` và đầy đủ 17 bảng.
2. Chạy file `SQL/Seed_Data_Chuan_Demo.sql` (nhấn **Execute** / `F5`) → Nạp đầy đủ dữ liệu mẫu chuẩn demo.

*(Không cần chạy 2 file `Patch_*.sql` nếu tạo mới từ đầu).*

---

## Bước 5: Chạy ứng dụng C#
Mở `src/QLThuVien.sln` bằng **Visual Studio** và nhấn **Start**, hoặc chạy bằng lệnh:

```powershell
dotnet run --project src/QLThuVien/QLThuVien.csproj
```

---

## Ghi chú (nếu máy khác KHÔNG cài Docker)
Nếu máy khác cài **SQL Server trực tiếp trên Windows** (không dùng Docker), chỉ cần mở SSMS kết nối vào SQL Server của máy đó, chạy 2 file SQL ở Bước 4, rồi sửa lại chuỗi kết nối trong:
- `src/QLThuVien/DAO/MyConnection.cs`
- `src/QLThuVien.Web/appsettings.json` *(nếu chạy Web)*

Ví dụ dùng Windows Authentication:
```text
Server=localhost;Database=QLThuVien;Integrated Security=True;TrustServerCertificate=True;
```
(hoặc `Server=.\SQLEXPRESS;...` nếu là bản SQL Server Express).
