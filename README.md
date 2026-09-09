# iStore — Website bán sản phẩm công nghệ của Apple

## 📋 Mục lục

1. [Giới thiệu dự án](#giới-thiệu-dự-án)
2. [Cấu trúc dự án](#cấu-trúc-dự-án)
3. [Tài khoản đăng nhập](#tài-khoản-đăng-nhập)
4. [Chức năng từng trang](#chức-năng-từng-trang)
5. [Cơ sở dữ liệu](#cơ-sở-dữ-liệu)
6. [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt)
7. [API Endpoints](#api-endpoints)
8. [Troubleshooting](#troubleshooting)

---

## 📋 Giới thiệu dự án

**iStore** là website thương mại điện tử chuyên bán các sản phẩm Apple được phát triển bằng ASP.NET Core MVC 8, C# .NET 8.

### Công nghệ sử dụng:

- ASP.NET Core MVC 8, C# .NET 8
- Entity Framework Core 8 + ASP.NET Core Identity
- SQL Server / LocalDB
- Bootstrap 5, Bootstrap Icons, custom CSS/JS (gradient, animation, glassmorphism)

## 📁 Cấu trúc dự án

```text
iStore/
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

## 🔐 Tài khoản đăng nhập

### Tài khoản Admin:

- **Email:** `admin@istore.vn`
- **Mật khẩu:** `Admin@123`
- **Quyền:** Quản trị toàn bộ hệ thống

### Tài khoản khách hàng mẫu:

- **Email:** `user@istore.vn`
- **Mật khẩu:** `User@123`
- **Quyền:** Mua sắm, xem đơn hàng

## ⭐ Chức năng nổi bật

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

## 🎨 Giao diện

- Palette gradient tím-hồng-cam kết hợp trắng/xám nhẹ, phong cách trẻ trung hiện đại
- Font Inter + Sora, corner-radius mềm 22–28px
- Animation: hero float, floating chip, hover product card, reveal on scroll
- Responsive từ mobile 375px đến desktop 1440px

## 🗄️ Cơ sở dữ liệu

- **Database:** `AppleStoreDb`
- **Database Engine:** SQL Server / LocalDB
- **ORM:** Entity Framework Core 8
- **Authentication:** ASP.NET Core Identity
- **SQL Script:** `Database/AppleStoreDb.sql`

## ⚙️ Hướng dẫn cài đặt

### Yêu cầu hệ thống:

- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code
- SQL Server 2022
- SQL Server Management Studio

### 1. Clone hoặc tải source code

Mở terminal và di chuyển đến thư mục dự án:

```bash
cd iStore
```

### 2. Khôi phục package

```bash
dotnet restore
```

### 3. Cấu hình cơ sở dữ liệu

Kiểm tra connection string trong:

```text
appsettings.json
```

Đảm bảo thông tin kết nối SQL Server phù hợp với môi trường đang sử dụng.

### 4. Tạo database

Nếu sử dụng Entity Framework Core Migration:

```bash
dotnet ef database update
```

Hoặc sử dụng file SQL có sẵn:

```text
Database/AppleStoreDb.sql
```

### 5. Chạy ứng dụng

```bash
dotnet run
```

### 6. Truy cập

- **Website:** `http://localhost:5000`
- **HTTPS:** `https://localhost:7211`

> Port thực tế có thể khác tùy theo cấu hình trong `Properties/launchSettings.json`.

## 🔌 API Endpoints

### Account

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Account/Login` | Trang đăng nhập |
| POST | `/Account/Login` | Xử lý đăng nhập |
| GET | `/Account/Register` | Trang đăng ký |
| POST | `/Account/Register` | Xử lý đăng ký |
| POST | `/Account/Logout` | Đăng xuất |

### Product

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Product` | Danh sách sản phẩm |
| GET | `/Product/Details/{id}` | Chi tiết sản phẩm |

### Cart

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Cart` | Xem giỏ hàng |
| POST | `/Cart/Add` | Thêm sản phẩm vào giỏ |
| POST | `/Cart/Update` | Cập nhật số lượng |
| POST | `/Cart/Remove` | Xóa sản phẩm khỏi giỏ |

### Checkout

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Checkout` | Trang thanh toán |
| POST | `/Checkout` | Tạo đơn hàng |

### Order

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Order` | Lịch sử đơn hàng |
| GET | `/Order/Details/{id}` | Chi tiết đơn hàng |

### Admin

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/Admin` | Dashboard quản trị |
| GET | `/Admin/Products` | Quản lý sản phẩm |
| GET | `/Admin/Categories` | Quản lý danh mục |
| GET | `/Admin/Orders` | Quản lý đơn hàng |
| GET | `/Admin/Customers` | Quản lý khách hàng |
| GET | `/Admin/Statistics` | Thống kê |

## 🛠️ Troubleshooting

### Lỗi thường gặp:

#### 1. Lỗi 403 - Access Denied

- Kiểm tra đã đăng nhập admin chưa
- Kiểm tra tài khoản có role `Admin` chưa
- Kiểm tra database đã được khởi tạo chưa

Nếu sử dụng Entity Framework:

```bash
dotnet ef database update
```

#### 2. Lỗi database

Chạy:

```bash
dotnet ef database update
```

Nếu vẫn lỗi, kiểm tra:

- SQL Server đang chạy
- Database `AppleStoreDb` đã tồn tại
- Connection string trong `appsettings.json` chính xác
- `TrustServerCertificate=True` đã được cấu hình nếu cần

#### 3. Lỗi build

Chạy:

```bash
dotnet clean
dotnet build
```

Nếu vẫn lỗi, kiểm tra:

- .NET SDK đã được cài đặt chưa
- Version SDK có phù hợp với project không
- Các NuGet package đã restore thành công chưa

#### 4. Lỗi port đã sử dụng

Dừng process:

```bash
taskkill /f /im dotnet.exe
```

Hoặc thay đổi port trong:

```text
Properties/launchSettings.json
```

### 🐞 Debug Commands

#### Xóa database SQL Server

Chạy trong SQL Server Management Studio hoặc `sqlcmd`:

```sql
ALTER DATABASE [AppleStoreDb]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE [AppleStoreDb];
```

Sau đó chạy lại:

```bash
dotnet run
```

#### Kiểm tra / thay đổi port

Mở:

```text
Properties/launchSettings.json
```

Sau đó kiểm tra cấu hình `applicationUrl`.

#### Kiểm tra kết nối database

Đảm bảo:

- SQL Server đang chạy
- Database server có thể truy cập
- Database name chính xác
- Username/password hoặc Windows Authentication chính xác
- `TrustServerCertificate=True` nếu môi trường yêu cầu

## 📞 Hỗ trợ

Nếu gặp vấn đề, hãy kiểm tra:

1. Log trong console
2. File `AppleStoreWeb.db` có tồn tại không
3. Database `AppleStoreDb` có tồn tại không
4. Connection string trong `appsettings.json`
5. Tài khoản admin đã được tạo chưa
6. Role `Admin` đã được gán đúng chưa
7. Port trong `launchSettings.json`

---

## 👨‍💻 Thông tin dự án

- **Sinh viên thực hiện:** Lâm Văn Chỉ
- **Số điện thoại:** 0942 473 373
- **Email:** `lamchi93@gmail.com`
- **Giảng viên hướng dẫn:** TS. Đoàn Phước Miền
- **Thời gian thực hiện:** Tháng 6 – 8/2026

---

<p align="center">
  <strong>iStore — Apple Technology E-commerce Website</strong>
</p>

<p align="center">
  Built with ASP.NET Core MVC 8 & C# .NET 8
</p>
