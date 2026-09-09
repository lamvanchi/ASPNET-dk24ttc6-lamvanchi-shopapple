
# Hướng dẫn kỹ thuật
## Yêu cầu môi trường

| Công cụ | Ghi chú |
| --- | --- |
| .NET SDK 8.0+ | `dotnet --version` |
| SQL Server 2022 (localhost) | Windows Authentication, không cần mật khẩu |
| sqlcmd hoặc SSMS | Tùy chọn, dùng để chạy file `.sql` |

## Kết nối database

Đã cấu hình sẵn trong `appsettings.json`:

```json
"DefaultConnection": "Server=localhost;Database=AppleStoreDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Trùng đúng với ảnh cấu hình:
- **Server Name**: `localhost`
- **Authentication**: Windows Authentication
- **Password**: không cần

## Khởi tạo dữ liệu — 2 cách

### Cách 1 — Chỉ cần chạy app (khuyên dùng)

```bash
dotnet run
```

`DbInitializer` sẽ tự động:
- Tạo database `AppleStoreDb`
- Chạy toàn bộ EF Core migrations
- Nạp 6 danh mục, 24 sản phẩm, 59 ảnh (ảnh thật từ Unsplash)
- Tạo tài khoản admin/user mẫu + 1 đơn hàng mẫu

### Cách 2 — Chạy file `.sql` trước (nếu giáo viên yêu cầu)

```bash
sqlcmd -S localhost -E -C -i Database\AppleStoreDb.sql
dotnet run
```

Hoặc mở `Database\AppleStoreDb.sql` bằng SSMS / Azure Data Studio và bấm **Execute**.

File này sẽ tạo database + toàn bộ schema (15 bảng). Sau đó `dotnet run` sẽ nạp dữ liệu mẫu.

## Tài khoản demo

| Vai trò | Email | Mật khẩu |
| --- | --- | --- |
| Quản trị | admin@istore.vn | Admin@123 |
| Khách hàng | user@istore.vn | User@123 |

## Đường dẫn chính

- Trang chủ: <http://localhost:5000>
- Sản phẩm: `/Product`
- Chi tiết sản phẩm: `/san-pham/{slug}` (VD `/san-pham/iphone-17-pro`)
- Về chúng tôi: `/Home/About`
- Liên hệ: `/Home/Contact`
- Giỏ hàng: `/Cart`
- Đơn hàng của tôi: `/Order`
- **Quản trị**: `/Admin/Dashboard` (chỉ role Admin)

---

Tài liệu kỹ thuật  
System: iStore
