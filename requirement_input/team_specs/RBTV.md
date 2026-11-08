Dưới đây là kịch bản SQL Server để khởi tạo Cơ sở dữ liệu Quản lý Thư
viện hoàn chỉnh dựa theo đúng sơ đồ thực thể mối quan hệ trong hình ảnh
của bạn, cùng phân tích đầy đủ các ràng buộc toàn vẹn.

**Tổng hợp Ràng buộc Toàn vẹn (Integrity Constraints)** Các ràng buộc
toàn vẹn được chia làm 4 nhóm chính:

A.  **Ràng buộc Miền giá trị (Domain Constraints)**

    - **Khóa chính & Không rỗng (**NOT NULL**):** Tất cả mã định danh
      (Ma\...) và các thông tin bắt buộc như TenSach, TenDG, NgayMuon,
      HanTra phải khác NULL.

    - **Giá trị số không âm (**CHECK \>= 0**):**

      - SoLuong, SLChoMuon, SLConLai trong DauSach \$\\ge 0\$.

      - DonGia, MucPhat, TongTien, TienPhat, SoNgayTre \$\\ge 0\$.

    - **Quy định thời gian (**CHECK**):**

      - NamXB trong DauSach không vượt quá năm hiện tại (\<=
        YEAR(GETDATE())).

      - HanTra trong PhieuMuon phải lớn hơn hoặc bằng NgayMuon.

      - NgayThucTra trong ChiTietPhieuMuon (nếu có) phải lớn hơn hoặc
        bằng NgayMuon.

B.  **Ràng buộc Thực thể (Entity Integrity)**

    - **Khóa chính (**PRIMARY KEY**):** Đảm bảo tính duy nhất và không
      rỗng cho từng bản ghi trong bảng:

      - Khóa đơn: MaDauSach, MaSach, MaDG, MaNV, MaPM, MaPN,
        MaPhieuPhat, MaQD, MaNXB, MaTG, MaTL, MaNCC, MaLDG.

      - Khóa phức hợp (bảng chi tiết): (MaPN, MaDauSach), (MaPhieuMuon,
        MaSach), (MaPhieuPhat, MaQD).

C.  **Ràng buộc Tham chiếu (Referential Integrity)**

    - **Khóa ngoại (**FOREIGN KEY**):** Đảm bảo dữ liệu liên kết phải
      tồn tại ở bảng danh mục tương ứng:

      - DocGia.MaLDG \$\\rightarrow\$ LoaiDocGia.MaLDG

      - DauSach.(MaNXB, MaTG, MaTL) \$\\rightarrow\$ NhaXuatBan, TacGia,
        TheLoai

      - Sach.MaDauSach \$\\rightarrow\$ DauSach.MaDauSach

      - PhieuNhap.(MaNCC, MaNV) \$\\rightarrow\$ NhaCungCap, NhanVien

      - ChiTietPhieuNhap.(MaPN, MaDauSach) \$\\rightarrow\$ PhieuNhap,
        DauSach

      - PhieuMuon.(MaDocGia, MaNV) \$\\rightarrow\$ DocGia, NhanVien

      - ChiTietPhieuMuon.(MaPhieuMuon, MaSach) \$\\rightarrow\$
        PhieuMuon, Sach

      - PhieuPhat.(MaDocGia, MaPhieuMuon, MaNV) \$\\rightarrow\$ DocGia,
        PhieuMuon, NhanVien

      - ChiTietPhieuPhat.(MaPhieuPhat, MaQD, MaSach) \$\\rightarrow\$
        PhieuPhat, QuyDinhPhat, Sach

D.  **Ràng buộc Liên kết & Quy tắc Nghiệp vụ (Business Rules -
    Trigger/Logic)**

Các quy tắc nghiệp vụ phức tạp dưới đây nên xử lý qua **Trigger** hoặc
**Stored Procedure**:

> 1\. **Quản lý tồn kho sách:**

- SoLuong trong DauSach phải luôn bằng: SLConLai + SLChoMuon.

- Khi mượn 1 cuốn sách: SLChoMuon tăng 1, SLConLai giảm 1 (Điều kiện
  mượn: SLConLai \> 0).

- Khi trả 1 cuốn sách: SLChoMuon giảm 1, SLConLai tăng 1.

> 2\. **Tính tổng tiền giao dịch:**

- PhieuNhap.TongTien = \$\\sum(\\text{ChiTietPhieuNhap.ThanhTien})\$.

- PhieuPhat.TongTienPhat = \$\\sum(\\text{ChiTietPhieuPhat.TienPhat})\$.

> 3\. **Quy định phạt quá hạn:**

- Nếu NgayThucTra \> HanTra, hệ thống tự tính SoNgayTre = DATEDIFF(day,
  HanTra, NgayThucTra) và sinh bản ghi phạt tương ứng.
