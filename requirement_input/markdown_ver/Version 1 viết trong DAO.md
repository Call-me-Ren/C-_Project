Version 1 viết trong DAO

Database mà lủng là 0 điểm

1.Đồ án phải xây theo 3 layer version 2, 3 layer là phải ( tạo class
Myconnection)

Thiết kê thêm lớp myconection thực hiện khai báo kết nối ( thường là mở
kết nối và đóng kết nối, thực thi câu truy vấn )

Mỗi lớp DAO gọi Myconnection thực hiện kết nối

- Viết 1 lần tái sử dung ( Quy tắt tái sử dụng )

2\. Tìm kiếm trên arraylist -\> viết trong BUS

Tối ưu truy xuất cơ sở dữ liệu( hạn chế việc truy xuất cơ sở dữ liệu 1
các bừa bãi )

KO được run câu truy vấn select ( tìm kiếm ) thường xuyên ( lâu lâu thì
được )

Khi đọc danh sách nạp dữ liệu lên GUI, nằm trong array-list, rồi run câu
truy vấn của database ở trên server

Bắt buột run câu truy vấn thêm, sửa, xóa

3.Tạo form main-sub: các bảng quan hệ 1-n

Tạo main-subform trên văn bản 1 chiều, thuận lợi cho người dùng

( thường dung list-view ( ko biết ghi đung không ), data read )

Liên kết 2 listview: click 1 row hóa đơn -\> n row chi tiết hóa đơn

Những chi tiết thường xuyên sử dụng thì hiện luông trên form ( còn ko
thuận tiên, ít dùng thì thiết kế nút ) ít dùng là click 3 tái mới tới

Hóa đơn có chi tiết hóa đơn: mã hóa đơn, mã sản phẩm, đơn giá ( gõ mã
sản phẩm nó hiển thị đơn giá ( để sẵn mã sản phẩm ) rồi nhập số lượng vô
thôi là xong. ( người ta nhớ thì thôi nhưng mà thiết kế form tạo điều
kiện thuần lợi cho người dùng nhưng đừng làm rối là được )

Đừng làm quá lòe loạt ( chỉ 1-2 chỗ màu đỏ ) còn lại để màu hài hòa

4\. Tạo nút ... hỗ trọ tìm kiếm -\> lấy thông tin -\> điề form: sử dụng
biến static

Ko nhớ thì hỗ trọ tìm kiếm bằng cách bấm báo nút ... rồi gõ tên, rồi
click chuột lên cái row tìm thấy, đóng fthì nó quay trở lại form rồi tự
điền mã

5 Lưu y tính duy nhất của dữ liệu: vd mã sinh viên

Nghiệp vụ viết ở lớp bus để gọi hàm đó bất cứ lúc nào ( ko được viết ở
lớp GUI và DAO , tính toán sử lý dử liệu tính ở lớp bus hết,

6\. Lưu ý các field bị thay đổi cơ sử dữ liệu

a\. Bán hàng -\> giảm số lượng sản phẩm

b\. Nhập hàng\> Tăng số lượng sản phẩm

7.Lưu ý có field dữ liệu không đươc nhập ( user ko sửa mất cáy như tên
sản phẩm, đơn giá, chỉ view lên thôi)

8.TÌm kiếm nâng cao: dùng các phép toán and, or, not, \>, \<, \<\>,
=\<,=\> để lọc dữ liêu trên 1 field or nhiều field

Ví dụ:

TÌm kiếm các hóa đơn từ 1 triệu đến 10 triệu

Xuất danh sách cách các khách hàng ở hồ chí minh và hà nội ( dùng OR)

9\. Table tham số

Tổ chực 1 table tham số chứa tất cả các giá trị ( gốm 2 cột, cột thứ
nhất là mã số để chứa các hằng số rời rạc nhau trong hệ thông, nói chung
là những value nào ko liên quan gì nhau và 1 tham số thì cho hết vào
đây)

10\. Thống kế: 3 bảng( tiền)

a\. Tổng thu (hóa đơn), tổng chi (phiếu nhập hàng ), lới nhuận

i\. Từ ngày -\> đến ngày

ii\. Theo tháng/quý/năm

Tổng tiền hóa đơn thheo nhân viên và quý

  -----------------------------------------------------------------------
              Q1          Q2          Q3          Q4          Q5
  ----------- ----------- ----------- ----------- ----------- -----------
  NV 1                                                        

  NV 2                                                        

  NV 3                                                        

  NV4                                                         
  -----------------------------------------------------------------------

11\. (1đ) Export-import file xls ( google read-write c#)

12.(1đ) Report -\> pdf: hóa đơn, bảng điểm ( sử dụng ai-tech ?, không
biết khi đúng không)

13\. Mẫu màn hình chính (desktop) form desktop

14\. Viết báo cáo cuối kỳ

\(1\) Trang bìa

a\. Đò án học phần lập trình C#

b\. Tên đề tài

c\. Thông ti nhóm sinh viên, lớp

d\. Gvhd: Phùng Thái Thiên Trang

e\. Dòng cuối: Tháng 12/2026

15 Lưu ý nếu CSDL có lưu trữ tiếng việt nối thêm text này vào cuối chuối
kết nối vói CSDL:

"?useUnicode=yes&characterEncoding=UTF-8"

16\. Time line

a\. Tuần 9+10: Kiểm tra giấy 10 phút, viết 102 hàm

b\. Tuần 11+12: chấm đồ án, bóc thăm thứ tự chấm
