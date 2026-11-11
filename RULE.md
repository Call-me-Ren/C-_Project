# Workspace Rules & Scope Constraints

## 1. Phạm vi thao tác (Scope of Operation)
- **Khu vực được phép:**
  - `requirement_input/`: Được phép đọc, phân tích và thao tác với các file trong thư mục này.
  - `docs/`: Được phép đọc, cập nhật, tạo mới và thao tác với các file tài liệu trong thư mục này( chỉ khi tôi yêu cầu )

## 2. Giới hạn nghiêm ngặt (Strict Restrictions)
- **KHÔNG ĐƯỢC PHÉP** tự ý đọc, chỉnh sửa, tạo mới hoặc xóa bất kỳ file/thư mục nào nằm ngoài hai thư mục `requirement_input/` và `docs/`.
- **Ngoại lệ duy nhất:** Chỉ được can thiệp vào các khu vực khác bên ngoài khi người dùng có yêu cầu hoặc chỉ thị trực tiếp trong cuộc hội thoại.

## 3. Quy tắc kiến trúc bắt buộc (từ giáo viên - Note.md & tài liệu GV)
- Mô hình 3 lớp: GUI → BUS → DAO; DTO dùng để truyền dữ liệu. **Cấm nhảy lớp** (GUI không được gọi thẳng DAO).
- **Cấm viết câu truy vấn SQL ở GUI và BUS.** SQL chỉ nằm ở DAO.
- Kết nối CSDL chỉ viết 1 lần trong class `MyConnection`; các DAO gọi lại, không tự mở kết nối riêng.
- Nghiệp vụ, kiểm tra hợp lệ, tính toán (tồn kho, thành tiền, tổng tiền, tiền phạt, tính duy nhất của mã) chỉ viết ở BUS.
- Tìm kiếm thực hiện trên ArrayList/List đã nạp ở BUS, không chạy SELECT tìm kiếm liên tục.
- Thêm/sửa/xóa bắt buộc chạy câu lệnh SQL thật xuống CSDL.
- Truy vấn dùng tham số (`SqlParameter`), không nối chuỗi.
- CSDL không được "lủng" (GV: lủng là 0 điểm): giữ PK/FK/CHECK, không xóa dữ liệu đã phát sinh giao dịch.
- **Cấu trúc Form ở tầng GUI:** Bắt buộc phải có **2 file** tách biệt theo chuẩn WinForms:
  1. Bản form thường (`frmXxx.cs`): Chứa logic sự kiện, tương tác BUS và điều khiển giao diện.
  2. Bản thiết kế (`frmXxx.Designer.cs`): Chứa định nghĩa thành phần, layout và khởi tạo control (`InitializeComponent`). Tuyệt đối không gộp chung vào 1 file.
- **Quy định bắt lỗi `try - catch`:**
  - Ở **GUI**: Mọi hàm xử lý sự kiện (`Form_Load`, Click Thêm, Sửa, Xóa, Tìm kiếm, CellClick...) **bắt buộc phải bọc trong khối `try ... catch`** để bắt các lỗi nghiệp vụ và CSDL, hiển thị thông báo qua `FormErrors.Show(ex)` hoặc `MessageBox.Show`, tránh để ứng dụng bị crash.
  - Ở **DAO**: Thao tác mở kết nối và thực thi SQL phải đảm bảo giải phóng tài nguyên (`using` hoặc `finally`) và có `try ... catch` xử lý lỗi kết nối/câu lệnh.
- Ưu tiên làm theo yêu cầu của giáo viên; khi mâu thuẫn với nguồn khác (bạn bè, đồ án tham khảo) thì theo giáo viên.

## 4. Quy tắc làm việc (code control)
- Chỉ làm 1 bước nhỏ mỗi lần; sau mỗi bước phải giải thích code bằng tiếng Việt.
- Không tự sinh hàng loạt code mà người dùng chưa hiểu; ưu tiên code đơn giản, đúng phong cách giáo trình của GV.
- Không tự thêm thư viện/công nghệ ngoài danh sách đã chốt khi chưa hỏi.

## 5. Phạm vi mở rộng (đã được người dùng đồng ý)
- Thư mục mã nguồn `src/`, cùng `.gitignore`, `README.md` ở gốc: được phép tạo/sửa để làm đồ án.
- Các file tài liệu kế hoạch, yêu cầu và quy trình ở thư mục gốc (`PLAN.md`, `YEU_CAU_DO_AN.md`, `GIT_WORKFLOW.md`): được phép tạo và cập nhật.
- `requirement_input/` chỉ đọc, không sửa tài liệu gốc của GV.
- Không commit/push git hoặc xóa file khi chưa được yêu cầu.
