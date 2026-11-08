![](./assets/c-3-layer/media/image41.png)

# Hướng dẫn xây dựng ứng dụng theo

> ![](./assets/c-3-layer/media/image43.png)![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
> height="9.541668853893263in"}mô hình 3 lớp (3-layer)
>
> GUI-BUS-DAO

# Mô hình 3 lớp

> ![](./assets/c-3-layer/media/image43.png)![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
> height="9.541668853893263in"}GUI-View-Presentation-form
>
> BUS-Business-nghiệp vụ
>
> ![](./assets/c-3-layer/media/image44.png)DAO-Database Access Object

![](./assets/c-3-layer/media/image43.png)

> //DTO: Data Transfer Object
>
> ![](./assets/c-3-layer/media/image90.png)class SinhvienDTO {String
> ma,ho,ten; ...} SinhvienDTO sv;
>
> GUI-View-Presentation-form -\> class SinhvienGUI

# sv sv

> BUS-Business-nghiệp vụ -\> class SinhVienBUS
>
> DAO-database Access Object -\> class SinhVienDAO
>
> class MySqlConnect {...}

class SinhvienDTO {String ma,ho,ten;

> ...}
>
> SinhvienDTO sv
>
> class SinhvienGUI { ..., bntDocDSSV_click(\...)}
>
> SinhVienBUS bus =new SinhvienBUS();
>
> ![](./assets/c-3-layer/media/image90.png)Bus.DocDSSV(); //⟶ArrayList
> dssv class SinhVienBUS {hàm DocDSSV()}
>
> SinhVienDAO data =new SinhvienDAO(); dssv=data.DocDSSV();
>
> class SinhVienDAO {hàm DocDSSV()}
>
> class MySqlConnect {...}

class SinhvienDTO {String ma,ho,ten;

> ...}
>
> SinhvienDTO sv
>
> class SinhvienGUI { ..., btnThem_click(\...)}

# sv sv

> class SinhVienBUS {hàm Them(SinhVienDTO sv)}
>
> ![](./assets/c-3-layer/media/image90.png)class SinhVienDAO {hàm
> Them(SinhVienDTO sv)} class MySqlConnect {...}

## Chương trình QLSV-3 lớp

1.  Database mySQL: database name: SVDB, table sinhvien(ma,ho,ten,.)

2.  Class SinhVienGUI: LVSinhVien, các textbox, button, ...

3.  Class SinhVienDTO (ma, ho, ten,..., get, set)

4.  Class SinhVienBUS:

> Thuộc tính ArrayList dssv_arr
>
> ![](./assets/c-3-layer/media/image90.png)Hàm thêm, sửa, xóa, tìm kiếm,
> kiểm tra mã sv duy nhất, kiểm tra dữ liệu hợp lệ, đọc danh sách...
>
> Đọc ds sinh viên ghi vào ArrayList dssv_arr: gọi hàm đọc danh sách sv
> trong sinhvienDAO,
>
> Hàm thêm: Kiểm tra dữ liệu hợp lệ, kiểm tra mã sinh viên duy nhất,
> thêm sv vào
>
> ArrayList dssv_arr, gọi hàm thêm sv vào database của SinhVienDAO

5.  Class SinhVienDAO

> Hàm thêm, sửa, xóa sinh viên, đọc danh sách sv, thực hiện câu truy vấn
> SQL

6.  Class MyConnection: thực hiện việc kết nối CSDL

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

![](./assets/c-3-layer/media/image90.png)

## ![](./assets/c-3-layer/media/image43.png)![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in" height="9.541668853893263in"}UTF-8

+\"?useUnicode=true&characterEncoding=UTF-8\"

![](./assets/c-3-layer/media/image41.png)

![](./assets/c-3-layer/media/image43.png)

> ![](./assets/c-3-layer/media/image120.png)

![](./assets/c-3-layer/media/image43.png)

![](./assets/c-3-layer/media/image43.png)

> ![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
> height="9.541668853893263in"}Các phương thức thường dùng
>
> ![](./assets/c-3-layer/media/image43.png)![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Mở kết nối với CSDL
>
> ![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Đóng kết nối với CSDL
> ![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Thực hiện câu truy vấn
>
> ![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Nhận kết quả phản hồi

![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
height="9.541668853893263in"}Cài đặt thư viện kết nối CSDL

> Cài đặt 1 trong 2 thư viện sau để kết nối với hệ quản trị
>
> CSDL MySQL:
>
> //# Standard official driver

1.  dotnet add package MySql.Data

> //Or the modern async alternative

2.  dotnet add package MySqlConnector

> \###
>
> ![](./assets/c-3-layer/media/image43.png)Khai báo sử dụng thư viện
> using System; using MySql.Data.MySqlClient; using MySqlConnector;

## Mở kết nối

1.  Khai báo chuỗi kết nối với CSDL

string connString =

![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
height="9.541668853893263in"}\"Server=localhost;Port=3306;Database=SINHVIENDB;Uid=root;Pwd=\'\';

\";

2.  Khai báo đối tượng kết nối CSDL

MySqlConnection connection = new MySqlConnection(connString))

3.  ![](./assets/c-3-layer/media/image43.png)Mở kết nối với CSDL
    connection.Open();

4.  Đóng kết nối với CSDL connection.Close();

> Đối tượng thực hiện câu truy vấn

![](./assets/c-3-layer/media/image43.png)Khai báo câu truy vấn string
query = \"SELECT MA,HO,TEN FROM SINHVIEN\";

> Đối tượng thực hiện câu truy vấn: MySqlCommand

![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
height="9.541668853893263in"}MySqlCommand command = new
MySqlCommand(query, connection)

## ![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in" height="9.541668853893263in"}Thực hiện câu truy vấn

> ![](./assets/c-3-layer/media/image43.png)![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} 2 loại câu truy vấn:
>
> ![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Lấy dữ liệu: SELECT:
> command.ExecuteReader()
>
> ![](./assets/c-3-layer/media/image33.png){width="0.1388888888888889in"
> height="0.18055555555555555in"} Thay đổi dữ liệu: INSERT, UPDATE,
>
> DELETE, ..: command.ExecuteNonQuery();

## ![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in" height="9.541668853893263in"}Thực hiện câu truy vấn SELECT

> •Đối tượng nhận kết quả câu truy vấn SELECT
>
> MySqlDataReader reader = command.ExecuteReader()
>
> Duyệt kết quả while (reader.Read())
>
> ![](./assets/c-3-layer/media/image43.png){
>
> // Access columns by name or index string ma =
> reader.GetString(0);//\"MA\"
>
> string ho = reader.GetString(\"HO\"); string ten =
> reader.GetString(\"TEN\");
>
> //MessageBox.Show(ma + \" \" + ho + \" \" + ten);
>
> Console.WriteLine(\$\"MA: {ma} \| Ho: {ho} \| Ten: {ten}\"); }
>
> ![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
> height="9.541668853893263in"}Thực hiện câu truy vấn Select

string connString =

![](./assets/c-3-layer/media/image43.png)\"Server=localhost;Port=3306;Database=SINHVIENDB;Uid=root;Pwd=\'\';\";
MySqlConnection connection = new MySqlConnection(connString);
connection.Open();

string query = \"SELECT MA,HO,TEN FROM SINHVIEN\";

MySqlCommand command = new MySqlCommand(query, connection);

MySqlDataReader reader = command.ExecuteReader(); while (reader.Read())

{

> // Access columns by name or index //int id = reader.GetInt32(\"id\");
> string ma = reader.GetString(0); string ho = reader.GetString(\"HO\");
> string ten = reader.GetString(\"TEN\"); SinhVien sv = new SinhVien(ma,
> ho, ten)
>
> //...

} connection.Close();

## Thực hiện câu truy vấn Insert

> string connString =
>
> ![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
> height="9.541668853893263in"}\"Server=localhost;Port=3306;Database=SINHVIENDB;Uid=r
> oot;Pwd=\'\';\";
>
> ![](./assets/c-3-layer/media/image43.png)MySqlConnection connection =
> new MySqlConnection(connString); connection.Open();
>
> string query = \"INSERT INTO SINHVIEN VALUES (' "
>
> +ma+" ' , ' "+ho+" ' , ' \"+ten+" ' ) \"; MySqlCommand command = new
> MySqlCommand(query, connection);
>
> int r = command.ExecuteNonQuery();
>
> //command.Cancel(); connection.Close();

## Thực hiện câu truy vấn Update

string connString =

![](./assets/c-3-layer/media/image11.png){width="0.3472222222222222in"
height="9.541668853893263in"}\"Server=localhost;Port=3306;Database=SINHVIENDB;Uid=root;Pwd

=\'\';\";

![](./assets/c-3-layer/media/image43.png)MySqlConnection connection =
new MySqlConnection(connString); connection.Open(); string
query=\"UPDATE SINHVIEN SET HO=' " +sv.Ho

> \+ " ',TEN=' " + sv.Ten + " ' "
>
> +\" WHERE MA=\'\"+sv.Ma+" ' \";

MySqlCommand command = new MySqlCommand(query, connection); int r =
command.ExecuteNonQuery();

//command.Cancel(); connection.Close();

## Thực hiện câu truy vấn Delete

string connString =

\"Server=localhost;Port=3306;Database=SINHVIENDB;Uid=ro ot;Pwd='';";

MySqlConnection connection = new MySqlConnection(connString);
connection.Open();

![](./assets/c-3-layer/media/image90.png)Console.WriteLine(\"Connection
successful!\"); string query=\"DELETE FROM SINHVIEN WHERE MA=\'\"+sv.Ma+
\"\'\"; MySqlCommand command = new MySqlCommand(query, connection);

int r = command.ExecuteNonQuery();

//command.Cancel(); connection.Close();
