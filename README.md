# iStore — Website bán sản phẩm công nghệ của Apple



## Công nghệ

- ASP.NET Core MVC 8, C# .NET 8
- Entity Framework Core 8 + ASP.NET Core Identity
- SQL Server / LocalDB
- Bootstrap 5, Bootstrap Icons, custom CSS/JS (gradient, animation, glassmorphism)

## Yêu cầu môi trường

| Công cụ | Ghi chú |
| --- | --- |
| .NET SDK 8.0+ | `dotnet --version` |
| SQL Server (localhost) | Windows Authentication, không cần mật khẩu |
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

## Cấu trúc thư mục

```
AppleStore/
├── Areas/Admin/           # Khu vực quản trị: Dashboard, Categories, Products, Orders, Customers, Statistics
├── Controllers/           # Home, Account, Product, Cart, Checkout, Order, Profile
├── Data/                  # ApplicationDbContext, DbInitializer
├── Database/
│   └── AppleStoreDb.sql   # File SQL tạo DB + schema
├── Helpers/               # SlugHelper, FormatHelper, VietnameseIdentityErrorDescriber
├── Models/                # ApplicationUser, Category, Product, Cart, Order, ...
├── Services/              # CartService, OrderService
├── ViewComponents/        # CartCount, CategoryMenu
├── ViewModels/
├── Views/
├── wwwroot/               # CSS (site.css, admin.css) + JS + ảnh
└── Program.cs
```

## Tính năng nổi bật

**Khách hàng**
- Đăng ký / Đăng nhập (Identity, mật khẩu mã hoá)
- Trang chủ hero gradient, floating chip, marquee thanh khuyến mãi
- Mega-menu Sản phẩm ở navbar, dropdown user, giỏ hàng badge
- Tìm kiếm, lọc theo danh mục & khoảng giá, sắp xếp
- Chi tiết sản phẩm: gallery, chọn màu / dung lượng, số lượng
- Giỏ hàng (session cho khách + database khi login, tự merge khi login)
- Thanh toán COD, tạo đơn, lịch sử đơn, chi tiết đơn
- Hồ sơ cá nhân, đổi mật khẩu
- Trang **Về chúng tôi** và **Liên hệ** với form gửi tin nhắn

**Quản trị (`/Admin`)**
- Dashboard: doanh thu, đơn hàng, hàng sắp hết, biểu đồ 6 tháng
- CRUD danh mục (chặn xoá khi còn sản phẩm)
- CRUD sản phẩm (upload ảnh chính + gallery, tự sinh slug)
- Đơn hàng: lọc theo trạng thái, cập nhật trạng thái
- Khách hàng: xem thông tin & số đơn
- Thống kê: doanh thu 12 tháng, best-seller, biểu đồ Chart.js

## Giao diện

- Palette gradient tím-hồng-cam kết hợp trắng/xám nhẹ, phong cách trẻ trung hiện đại
- Font Inter + Sora, corner-radius mềm 22–28px
- Animation: hero float, floating chip, hover product card, reveal on scroll
- Responsive từ mobile 375px đến desktop 1440px

## Debug tips

- Xoá DB làm lại: chạy trong sqlcmd
  ```sql
  ALTER DATABASE [AppleStoreDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
  DROP DATABASE [AppleStoreDb];
  ```
  rồi `dotnet run` lại.
- Nếu port 5000 bận: đổi trong `Properties/launchSettings.json`.
- Kết nối SSL lỗi: đảm bảo `TrustServerCertificate=True` (đã có sẵn).
