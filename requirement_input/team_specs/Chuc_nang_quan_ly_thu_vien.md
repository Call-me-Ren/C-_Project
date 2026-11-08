**DANH SÁCH CHỨC NĂNG\
PHẦN MỀM QUẢN LÝ THƯ VIỆN**

*(Định hướng triển khai bằng C# Windows Forms)*

Tài liệu này tổng hợp các nhóm chức năng đề xuất cho hệ thống quản lý
thư viện, được xây dựng dựa trên mô hình dữ liệu gồm đầu sách, sách, tác
giả, thể loại, nhà xuất bản, nhập sách, độc giả, mượn -- trả sách và
phiếu phạt.

# 1. Quản lý đầu sách

- Thêm đầu sách mới.

- Sửa thông tin đầu sách.

- Xóa đầu sách (chỉ khi chưa phát sinh dữ liệu liên quan).

- Tìm kiếm đầu sách theo mã hoặc tên đầu sách.

- Xem danh sách đầu sách.

- Gán và quản lý tác giả của đầu sách.

- Chọn thể loại và nhà xuất bản cho đầu sách.

# 2. Quản lý sách

- Thêm từng cuốn sách vào thư viện.

- Sửa thông tin cuốn sách.

- Xóa cuốn sách (theo điều kiện nghiệp vụ).

- Tìm kiếm sách theo mã sách hoặc mã đầu sách.

- Cập nhật tình trạng sách.

- Cập nhật vị trí lưu trữ sách.

- Xem danh sách sách có sẵn, đang được mượn, hư hỏng hoặc bị mất.

# 3. Quản lý tác giả

- Thêm tác giả.

- Sửa thông tin tác giả.

- Xóa tác giả khi không còn được đầu sách sử dụng.

- Tìm kiếm tác giả.

- Xem danh sách tác giả.

# 4. Quản lý thể loại

- Thêm thể loại sách.

- Sửa thông tin thể loại.

- Xóa thể loại khi không còn đầu sách sử dụng.

- Tìm kiếm thể loại.

- Xem danh sách thể loại.

# 5. Quản lý nhà xuất bản

- Thêm nhà xuất bản.

- Sửa thông tin nhà xuất bản.

- Xóa nhà xuất bản khi không còn đầu sách tham chiếu.

- Tìm kiếm nhà xuất bản.

- Xem danh sách nhà xuất bản.

# 6. Quản lý nhà cung cấp

- Thêm nhà cung cấp.

- Sửa thông tin nhà cung cấp.

- Xóa nhà cung cấp khi không còn phiếu nhập liên quan.

- Tìm kiếm nhà cung cấp.

- Xem danh sách nhà cung cấp.

# 7. Quản lý nhập sách

- Lập phiếu nhập sách.

- Chọn nhà cung cấp và nhân viên lập phiếu.

- Thêm đầu sách và số lượng nhập vào chi tiết phiếu.

- Sửa hoặc xóa chi tiết phiếu nhập trước khi xác nhận.

- Tìm kiếm và xem danh sách phiếu nhập.

- Tính thành tiền từng dòng và tổng tiền phiếu nhập.

- Cập nhật số lượng cuốn sách trong thư viện sau khi nhập.

# 8. Quản lý độc giả

- Thêm độc giả.

- Sửa thông tin độc giả.

- Xóa hoặc ngừng hoạt động thẻ độc giả theo điều kiện nghiệp vụ.

- Tìm kiếm độc giả.

- Quản lý loại độc giả.

- Xem lịch sử mượn và trả sách của độc giả.

# 9. Quản lý loại độc giả

- Thêm loại độc giả.

- Sửa thông tin loại độc giả.

- Xóa loại độc giả khi không còn độc giả sử dụng.

- Tìm kiếm loại độc giả.

- Xem danh sách loại độc giả.

# 10. Quản lý nhân viên

- Thêm nhân viên.

- Sửa thông tin nhân viên.

- Xóa hoặc ngừng hoạt động nhân viên theo điều kiện nghiệp vụ.

- Tìm kiếm nhân viên.

- Xem danh sách nhân viên.

# 11. Quản lý mượn sách

- Lập phiếu mượn sách.

- Chọn độc giả và nhân viên lập phiếu.

- Chọn từng cuốn sách cần mượn.

- Kiểm tra tình trạng và khả năng cho mượn của sách.

- Kiểm tra điều kiện mượn của độc giả.

- Xác nhận mượn và cập nhật trạng thái sách.

- Tìm kiếm phiếu mượn.

- Xem danh sách sách đang được mượn và các phiếu mượn.

# 12. Quản lý trả sách

- Tiếp nhận và xác nhận sách được trả.

- Ghi nhận ngày trả thực tế.

- Cập nhật tình trạng sách khi trả.

- Kiểm tra hạn trả và xác định sách trả trễ.

- Cập nhật trạng thái chi tiết mượn và phiếu mượn.

- Xem lịch sử trả sách.

# 13. Quản lý phiếu phạt

- Lập phiếu phạt cho độc giả.

- Thêm các khoản phạt vào chi tiết phiếu phạt.

- Áp dụng quy định phạt đối với trả trễ, mất hoặc hư hỏng sách.

- Tính tiền phạt theo quy định.

- Sửa quy định phạt và mức phạt.

- Ghi nhận trạng thái thanh toán.

- Tìm kiếm và xem danh sách phiếu phạt.

# 14. Thống kê -- Báo cáo

- Thống kê tổng số đầu sách.

- Thống kê tổng số cuốn sách.

- Thống kê sách theo thể loại và tác giả.

- Thống kê sách có sẵn, đang được mượn, hư hỏng hoặc bị mất.

- Thống kê số lượng độc giả.

- Thống kê lượt mượn theo ngày, tháng và năm.

- Thống kê sách được mượn nhiều nhất.

- Thống kê tiền nhập sách.

- Thống kê tiền phạt.

# 15. Gợi ý bố trí menu giao diện

1.  Trang chủ

2.  Quản lý đầu sách

3.  Quản lý sách

4.  Quản lý tác giả

5.  Quản lý thể loại

6.  Quản lý nhà xuất bản

7.  Quản lý nhà cung cấp

8.  Quản lý nhập sách

9.  Quản lý độc giả

10. Quản lý nhân viên

11. Quản lý mượn sách

12. Quản lý trả sách

13. Quản lý phiếu phạt

14. Thống kê -- Báo cáo

# 16. Một số lưu ý nghiệp vụ

- Đầu sách lưu thông tin chung của một ấn phẩm như tên, ISBN, năm xuất
  bản, tác giả, thể loại và nhà xuất bản. Sách lưu thông tin của từng
  cuốn vật lý như mã sách, tình trạng và vị trí.

- Khi lập phiếu mượn, cần chọn mã sách của từng cuốn cụ thể, không chỉ
  chọn mã đầu sách.

- Khi nhập sách theo số lượng, hệ thống cần tạo hoặc ghi nhận các cuốn
  sách thực tế tương ứng để có thể quản lý từng cuốn khi cho mượn.

- Không nên xóa dữ liệu đã phát sinh giao dịch mượn, trả, nhập hoặc
  phạt; nên cân nhắc chức năng ngừng hoạt động hoặc khóa dữ liệu.

- Các trạng thái và quy định mượn -- trả -- phạt cần được thống nhất
  trước khi lập trình để tránh cập nhật sai số lượng và tình trạng sách.

- Phiếu trả riêng là lựa chọn phù hợp nếu thư viện muốn lưu nhiều lần
  trả cho một phiếu mượn. Nếu mỗi chi tiết mượn chỉ ghi nhận một lần
  trả, có thể lưu ngày trả thực tế ngay trong chi tiết phiếu mượn.
