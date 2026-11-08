Quy tắt 1:GUI gọi BUS, BUS gọi DAO ( ko dược nhảy nhảy bước, cái này gọi cái kia ngoài quy tắt)
Quy tắt 2: Tuyệt đối không ghi câu truy vấn ở lớp GUI, chỉ ghi ở DAO
Quy tắt 3: Đồ án làm theo mô hình 3 lớp
( thiết kế thêm 1 class mysqlconnect hoặc myconnection thực hiện việc kết nối cơ sở dữ liệu chỉ viết 1 lần ở lớp này rồi gọi truy vẫn ở các lớp DAO khác )

#ASP.NET.CORE
làm tối thiểu 1 trang cho đồ án ( làm nhiều được cộng điểm)
Lưu ý dấy @ trang trong Razor
Famrwork Blazor hỗ trợ tạo 1 dự án ASP.NET.CORE

Yêu cầu:
- Mội đồ án yêu cầu 1 trang web, quản lý đối tượng nào đó. ( nếu bổ sung thêm trang đặt hàng, trang xác nhận admin..)