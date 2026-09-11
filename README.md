# iStore — Website bán sản phẩm công nghệ của Apple

## 📋 Mục lục

1. [Giới thiệu dự án](#giới-thiệu-dự-án)
2. [Cấu trúc dự án](#cấu-trúc-dự-án)
3. [Tài khoản đăng nhập](#tài-khoản-đăng-nhập)
4. [Chức năng nổi bật](#chức-năng-nổi-bật)
5. [Cơ sở dữ liệu](#cơ-sở-dữ-liệu)
6. [Thông Tin Dự Án](#thong-tin-dự-án)

---

##  Giới thiệu dự án

**iStore** là website thương mại điện tử chuyên bán các sản phẩm Apple được phát triển bằng ASP.NET Core MVC 8, C# .NET 8.

### Công nghệ sử dụng:

- ASP.NET Core MVC 8, C# .NET 8
- Entity Framework Core 8 + ASP.NET Core Identity
- SQL Server / LocalDB
- Bootstrap 5, Bootstrap Icons, custom CSS/JS (gradient, animation, glassmorphism)

##  Cấu trúc dự án

```text
scr/
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

##  Tài khoản đăng nhập

### Tài khoản Admin:

- **Email:** `admin@istore.vn`
- **Mật khẩu:** `Admin@123`
- **Quyền:** Quản trị toàn bộ hệ thống

### Tài khoản khách hàng mẫu:

- **Email:** `user@istore.vn`
- **Mật khẩu:** `User@123`
- **Quyền:** Mua sắm, xem đơn hàng

##  Chức năng nổi bật

### Khách hàng

- Đăng ký / Đăng nhập (Identity, mật khẩu mã hoá)
- Trang chủ hero gradient, floating chip, marquee thanh khuyến mãi
- Mega-menu Sản phẩm ở navbar, dropdown user, giỏ hàng badge
- Tìm kiếm, lọc theo danh mục & khoảng giá, sắp xếp
- Chi tiết sản phẩm: gallery, chọn màu / dung lượng, số lượng
- Giỏ hàng (session cho khách + database khi login, tự merge khi login)
- Thanh toán COD, tạo đơn, lịch sử đơn, chi tiết đơn
- Hồ sơ cá nhân, đổi mật khẩu
- Trang **Về chúng tôi** và **Liên hệ** với form gửi tin nhắn

### Quản trị (`/Admin`)

- Dashboard: doanh thu, đơn hàng, hàng sắp hết, biểu đồ 6 tháng
- CRUD danh mục (chặn xoá khi còn sản phẩm)
- CRUD sản phẩm (upload ảnh chính + gallery, tự sinh slug)
- Đơn hàng: lọc theo trạng thái, cập nhật trạng thái
- Khách hàng: xem thông tin & số đơn
- Thống kê: doanh thu 12 tháng, best-seller, biểu đồ Chart.js

##  Giao diện

- Palette gradient tím-hồng-cam kết hợp trắng/xám nhẹ, phong cách trẻ trung hiện đại
- Font Inter + Sora, corner-radius mềm 22–28px
- Animation: hero float, floating chip, hover product card, reveal on scroll
- Responsive từ mobile 375px đến desktop 1440px

##  Cơ sở dữ liệu

- **Database:** `AppleStoreDb`
- **Database Engine:** SQL Server / LocalDB
- **ORM:** Entity Framework Core 8
- **Authentication:** ASP.NET Core Identity
- **SQL Script:** `Database/AppleStoreDb.sql`

##  Thông tin dự án

- **Sinh viên thực hiện:** Lâm Văn Chỉ
- **Số điện thoại:** 0942 473 373
- **Email:** `chilv200693@tvu-onschool.edu.vn`
- **Giảng viên hướng dẫn:** TS. Đoàn Phước Miền
- **Thời gian thực hiện:** Tháng 6 – 8/2026

---
