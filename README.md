# Đồ Án Quản Lý Thư Viện (C# WinForms)

## Thông tin đề tài
- **Môn học:** Lập trình C#
- **GVHD:** Phùng Thái Thiên Trang
- **Đề tài:** Phần mềm Quản lý Thư viện
- **Nền tảng:** C# WinForms (.NET 10), SQL Server, Visual Studio 2022 / VS Code

## Cấu trúc thư mục mã nguồn (`src/QLThuVien/`)
Tuân thủ nghiêm ngặt mô hình 3 lớp (3-Layer Architecture):
- **DTO (Data Transfer Object):** Chứa các lớp định nghĩa đối tượng dữ liệu.
- **DAO (Data Access Object):** Chứa các lớp tương tác với CSDL qua `MyConnection`.
- **BUS (Business Logic Layer):** Chứa các quy tắc nghiệp vụ, kiểm tra tính hợp lệ, tính toán và xử lý tìm kiếm trên danh sách.
- **GUI (Graphical User Interface):** Chứa các Form giao diện WinForms tương tác với người dùng.

## Cách mở và chạy dự án
1. Mở file solution `src/QLThuVien.sln` bằng Visual Studio 2022 (hoặc mở thư mục bằng VS Code).
2. Chạy lệnh build:
   ```bash
   dotnet build src/QLThuVien.sln
   ```
3. Chạy ứng dụng:
   ```bash
   dotnet run --project src/QLThuVien/QLThuVien.csproj
   ```
