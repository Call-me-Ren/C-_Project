Yêu cầu đồ án bổ sung

1.  Mô hình 3-Layer ver2 (tạo class MyConnection)

2.  Tìm kiếm trên ArrayList → viết trong BUS

3.  Tạo Form main-sub: các bảng quan hệ 1-n

Liên kết 2 Listview: click 1 row hóa đơn → n row chi tiết hóa đơn

> ![](./assets/C-w7-YeuCauDoAn/media/image0.jpg)

![](./assets/C-w7-YeuCauDoAn/media/image20.jpg)

![](./assets/C-w7-YeuCauDoAn/media/image5.jpg){width="4.900831146106737in"
height="2.6597222222222223in"}

> ![](./assets/C-w7-YeuCauDoAn/media/image50.jpg)
>
> ![](./assets/C-w7-YeuCauDoAn/media/image7.jpg){width="5.180930664916885in"
> height="3.5729166666666665in"}
>
> 5\. Lưu ý tính duy nhất của dữ liệu: vd mã sinh viên 6. Lưu ý các
> field bị thay đổi dữ liệu:

a.  Bán hàng → giảm số lượng có của sản phẩm

b.  Nhập hàng → tăng số lượng có của sản phẩm

<!-- -->

7.  Lưu ý có field dữ liệu không được nhập

8.  Tìm kiếm nâng cao: dùng các phép toán and, or , not, \>,≥,\<,≤, \<\>
    để lọc dữ liệu trên 1 field or nhiều field

9.  Table tham số

10. Thống kê: 3 bảng (tiền)

> a\. Tổng thu (hóa đơn), tổng chi (phiếu nhập hàng), lợi nhuận
>
> i\. Từ ngày→ đến ngày ii. Theo tháng/quý/năm

Tổng tiền hóa đơn theo nhân viên và quý, năm

+----------+----------+----------+----------+----------+----------+
|          | > Q1     | > Q2     | > Q3     | > Q4     | > TC     |
+==========+==========+==========+==========+==========+==========+
| > NV 1   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > NV 2   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > NV 3   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > TC     |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+

Tổng tiền hóa đơn theo khách hàng và quý, năm

+----------+----------+----------+----------+----------+----------+
|          | > Q1     | > Q2     | > Q3     | > Q4     | > TC     |
+==========+==========+==========+==========+==========+==========+
| > KH 1   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > KH 2   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > KH 3   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > TC     |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+

Tổng tiền hóa đơn theo sản phẩm và quý (hóa đơn & CTHD)

+----------+----------+----------+----------+----------+----------+
|          | > Q1     | > Q2     | > Q3     | > Q4     | > TC     |
+==========+==========+==========+==========+==========+==========+
| > Sp 1   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > Sp 2   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > Sp 3   |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+
| > TC     |          |          |          |          |          |
+----------+----------+----------+----------+----------+----------+

11. (1đ) Export-import file xls

12. (1đ) Report → pdf: hóa đơn, bảng điểm

![](./assets/C-w7-YeuCauDoAn/media/image8.jpg){width="4.7030293088363955in"
height="2.6444444444444444in"}

13. Mẫu màn hình chính (desktop) form desktop

![](./assets/C-w7-YeuCauDoAn/media/image9.jpg){width="6.252084426946632in"
height="3.325in"} menu left

form con

![](./assets/C-w7-YeuCauDoAn/media/image10.jpg){width="6.260416666666667in"
height="2.967361111111111in"}

14. Viết báo cáo cuối kỳ

    (1) Trang bìa

        a.  Đồ án học phần Lập trình C#

        b.  Tên đề tài

        c.  Thông tin nhóm sinh viên, lớp

        d.  Gvhd: Phùng Thái Thiên Trang

        e.  Dòng cuối: Tháng 12/2026

    (2) Nội dung chính

+---------------------------------------------------------------------+
| I.  Giới thiệu đề tài                                               |
|                                                                     |
| II. Thiết kế mô hình CSDL: chụp màn hình CSDL, lưu ý ảnh phải rõ    |
|     nét III. Các chức năng chính                                    |
|                                                                     |
| Liệt kê các chức năng chính 1,2,3,..., các chức năng con: a,b,c..., |
| mỗi chức năng thực hiện:                                            |
|                                                                     |
| a.  Giới thiệu chức năng                                            |
|                                                                     |
| b.  Chụp màn hình form, chú thích các thành phần trên form          |
|                                                                     |
| c.  Thuyết minh công thức tính toán: thành tiền, tổng tiền, tổng    |
|     lương, giảm giá, ...                                            |
|                                                                     |
| d.  Mỗi hàm (chức năng): xác định Đầu vào, đầu ra dữ liệu, xử lý    |
|     tính toán...                                                    |
|                                                                     |
| IV\. Hướng dẫn cài đặt đồ án                                        |
|                                                                     |
| a\. Chụp màn hình cây thư mục chứa source, thấy rõ phân lớp         |
| GUI-BUS-DAO b. Cách kết nối CSDL                                    |
|                                                                     |
| c\. Cách cài đặt và link download các tool hoặc thư viện sử dụng    |
| trong đồ án V. Sản phẩm đồ án: link chứa source code và file báo    |
| cáo đồ án DOC/PDF                                                   |
+=====================================================================+

\- Lưu ý báo cáo không quá 20 trang, font chữ Times New Roman size 13,
không có trang mục lục.

> ![](./assets/C-w7-YeuCauDoAn/media/image11.jpg){width="5.181208442694663in"
> height="4.090972222222222in"}

15. Lưu ý nếu CSDL có lưu trữ tiếng Việt: nối thêm text này vào cuối
    chuỗi kết nối với CSDL: "?useUnicode=yes&characterEncoding=UTF-8"

16. Time line

    a.  Tuần 9+10: kiểm tra giấy 10', viết 1-2 hàm

    b.  Tuần 11+12: chấm đồ án, bóc thăm thứ tự chấm
