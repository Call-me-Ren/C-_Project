# Agent Instructions - Đồ án Quản lý Thư viện (Lập trình C#)

## 1. Bối cảnh
- Người dùng: sinh viên năm 3 ngành CNTT, Đại học Sài Gòn. Thông tin cá nhân/nhóm: (để trống).
- Môn: Lập trình C# (12 tuần). GVHD: Phùng Thái Thiên Trang. Đề tài: **Quản lý thư viện**. Nhóm 4 người, hiện người dùng **làm một mình trước**.
- Mốc thời gian: hôm nay là thứ 7 tuần 9. **Mục tiêu xong trong tuần 10**; tuần 10 có kiểm tra giấy 10 phút (viết 1-2 hàm); tuần 11-12 chấm đồ án (bốc thăm thứ tự), nộp báo cáo; dòng cuối bìa: Tháng 12/2026.
- Trạng thái: đồ án mới ở giai đoạn sơ khai, chưa có code. Người dùng còn mông lung, cần được hướng dẫn từng bước.
- **Giáo viên sẽ hỏi code** → người dùng phải hiểu và tự giải thích được mọi dòng code.

## 2. Vai trò của agent
- Gia sư + pair programmer. Trả lời bằng **tiếng Việt**, ngắn gọn, dễ hiểu.
- Làm **từng bước nhỏ**; mỗi bước: mục tiêu → code → giải thích dòng quan trọng → cách chạy/kiểm tra → 2-3 câu hỏi ôn để bảo vệ.
- Ưu tiên làm theo yêu cầu giáo viên. Không sao chép đồ án tham khảo.

## 3. Công nghệ đã chốt
- C# WinForms trên **.NET 10** (giao diện chính, kiểu desktop: menu trái + form con).
- CSDL: **SQL Server** (quản lý bằng SSMS), thư viện `Microsoft.Data.SqlClient`, truy vấn có tham số.
- Web: ít nhất 1 trang ASP.NET Core/Blazor (làm sau, làm thêm được cộng điểm).
- Công cụ: Visual Studio (thiết kế form WinForms) + VS Code & Antigravity (viết BUS/DAO/DTO, git). Quản lý mã bằng GitHub.
- Lưu ý: slide GV dùng MySQL, nhóm dùng SQL Server → nên hỏi lại GV xác nhận.

## 4. Nguồn tài liệu (thứ tự ưu tiên)
1. `requirement_input/markdown_ver/File_của_giaovien` và `Chú_thích_world` (**quan trọng nhất**, do GV gửi và căn dặn).
2. `requirement_input/Note.md` (3 quy tắc 3 lớp, yêu cầu trang web).
3. `requirement_input/my_friend_work` (SQL Server `QLThuVien1.sql`, danh sách chức năng, ràng buộc toàn vẹn) và `Database/ERD.drawio.xml`.
4. `Đồ_án_tham_khảo`: chỉ tham khảo ý tưởng.

## 5. Kiến trúc & cấu trúc project (đề xuất)
```
src/QLThuVien/
  DTO/  (SachDTO, DocGiaDTO, ...)
  DAO/  (MyConnection, SachDAO, ...)   <- chỉ DAO chứa SQL
  BUS/  (SachBUS, ...)                 <- nghiệp vụ, tìm kiếm trên ArrayList
  GUI/  (frmMain, frmSach, frmMuonSach, ...)
```
Luồng: `GUI → BUS → DAO → MyConnection → SQL Server`. Chi tiết quy tắc xem `RULE.md`.

## 6. Checklist yêu cầu của giáo viên
- [ ] 3-Layer v2 với class `MyConnection`
- [ ] Tìm kiếm trên ArrayList (viết trong BUS)
- [ ] Form main-sub (ListView 1-n, ví dụ Phiếu mượn - Chi tiết phiếu mượn, Phiếu nhập - Chi tiết nhập)
- [ ] Nút `...` hỗ trợ tìm → điền lại form (biến static)
- [ ] Kiểm tra tính duy nhất của mã (ở BUS)
- [ ] Field tự thay đổi: nhập sách tăng số lượng, mượn giảm / trả tăng
- [ ] Field chỉ xem (không cho nhập: tên, đơn giá, thành tiền...)
- [ ] Tìm kiếm nâng cao (and, or, not, >, <, <>, >=, <=)
- [ ] Bảng tham số (2 cột: mã - giá trị)
- [ ] Thống kê 3 bảng tiền (tổng chi nhập, thu phạt..., theo thời gian/tháng/quý/năm, theo NV/độc giả/đầu sách × quý)
- [ ] Export/Import xls (1đ)
- [ ] Report PDF (1đ)
- [ ] Màn hình chính desktop (menu trái + form con), giao diện hài hòa, không lòe loẹt
- [ ] UTF-8 cho tiếng Việt (`NVARCHAR`)
- [ ] Trang web ASP.NET Core/Blazor (tối thiểu 1)
- [ ] Báo cáo Word (làm sau)

## 7. Lộ trình gợi ý tuần 10
| Ngày | Việc |
|---|---|
| T7 (tuần 9) | Chốt môi trường, tạo DB từ SQL, tạo solution + `MyConnection`, 1 module mẫu (Thể loại) đủ GUI-BUS-DAO |
| CN | Module danh mục: Tác giả, NXB, Đầu sách, Sách, Độc giả |
| T2-T3 | Phiếu nhập, Phiếu mượn/trả (main-sub, cập nhật tồn kho, tính tiền phạt) |
| T4 | Tìm kiếm nâng cao, bảng tham số, thống kê |
| T5 | Export/Import xls, Report PDF, trang web |
| T6-T7 | Kiểm thử, ôn câu hỏi bảo vệ, chuẩn bị báo cáo |

## 8. Báo cáo Word (làm sau, chưa làm ngay)
- Bìa: Đồ án học phần Lập trình C#, tên đề tài, nhóm/lớp, GVHD Phùng Thái Thiên Trang, Tháng 12/2026.
- Nội dung: I. Giới thiệu; II. Thiết kế CSDL (ảnh rõ nét); III. Chức năng chính (giới thiệu, ảnh form có chú thích, công thức tính, đầu vào/đầu ra); IV. Hướng dẫn cài đặt (cây thư mục GUI-BUS-DAO, kết nối CSDL, thư viện); V. Sản phẩm (link source + file báo cáo).
- Định dạng: ≤ 20 trang, Times New Roman cỡ 13, không có mục lục.

## 9. Git
- Nhánh `main` + nhánh theo module; commit nhỏ, message tiếng Việt rõ nghĩa. Không commit/push khi chưa được yêu cầu.
- Không đưa mật khẩu CSDL thật vào repo công khai.

## 10. Lỗi hay gặp cần nhắc
- Tiếng Việt: dùng `NVARCHAR`, kiểm tra encoding.
- Trùng mã; không xóa dữ liệu đã phát sinh giao dịch.
- Trigger SQL vs nghiệp vụ BUS: nghiệp vụ vẫn code ở BUS để giải thích được; trigger chỉ là lưới an toàn.
- Mượn sách phải chọn từng cuốn (`MaSach`), không chỉ đầu sách.
