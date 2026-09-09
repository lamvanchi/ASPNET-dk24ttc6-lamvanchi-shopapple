iStore — Website bán sản phẩm công nghệ của Apple
📋 Mục lục
Giới thiệu dự án
Cấu trúc dự án
Tài khoản đăng nhập
Chức năng nổi bật
Giao diện
Hướng dẫn cài đặt
Troubleshooting
Hỗ trợ
📋 Giới thiệu dự án

iStore là website thương mại điện tử chuyên bán các sản phẩm Apple, được phát triển bằng ASP.NET Core MVC 8 và C# .NET 8.

🛠️ Công nghệ sử dụng
ASP.NET Core MVC 8, C# .NET 8
Entity Framework Core 8
ASP.NET Core Identity
SQL Server / LocalDB
Bootstrap 5
Bootstrap Icons
Custom CSS/JS
Gradient, animation và glassmorphism
📁 Cấu trúc dự án
iStore/
├── Areas/
│   └── Admin/              # Khu vực quản trị
│       ├── Dashboard/
│       ├── Categories/
│       ├── Products/
│       ├── Orders/
│       ├── Customers/
│       └── Statistics/
├── Controllers/            # Home, Account, Product, Cart, Checkout, Order, Profile
├── Data/                   # ApplicationDbContext, DbInitializer
├── Database/
│   └── AppleStoreDb.sql    # File SQL tạo DB + schema
├── Helpers/                # SlugHelper, FormatHelper, VietnameseIdentityErrorDescriber
├── Models/                 # ApplicationUser, Category, Product, Cart, Order, ...
├── Services/               # CartService, OrderService
├── ViewComponents/         # CartCount, CategoryMenu
├── ViewModels/
├── Views/
├── wwwroot/                # CSS + JS + hình ảnh
│   ├── css/
│   ├── js/
│   └── images/
└── Program.cs

🔐 Tài khoản đăng nhập
Tài khoản Admin
Email: admin@istore.vn
Mật khẩu: Admin@123
Quyền: Quản trị toàn bộ hệ thống
Tài khoản khách hàng mẫu
Email: user@istore.vn
Mật khẩu: User@123
Quyền: Mua sắm và xem đơn hàng

⚠️ Lưu ý: Các tài khoản trên chỉ nên sử dụng cho môi trường phát triển/demo. Khi triển khai thực tế, hãy thay đổi mật khẩu mặc định.

⭐ Chức năng nổi bật
👤 Khách hàng
Đăng ký / Đăng nhập bằng ASP.NET Core Identity
Mật khẩu được mã hóa và bảo vệ theo cơ chế của Identity
Trang chủ với hero gradient
Floating chip và marquee thanh khuyến mãi
Mega-menu Sản phẩm trên navbar
Dropdown tài khoản người dùng
Hiển thị số lượng sản phẩm trong giỏ hàng bằng badge
Tìm kiếm sản phẩm
Lọc sản phẩm theo:
Danh mục
Khoảng giá
Sắp xếp sản phẩm
Trang chi tiết sản phẩm:
Gallery hình ảnh
Chọn màu
Chọn dung lượng
Chọn số lượng
Giỏ hàng:
Session cho khách chưa đăng nhập
Database cho người dùng đã đăng nhập
Tự động merge giỏ hàng khi đăng nhập
Thanh toán COD
Tạo và quản lý đơn hàng
Xem lịch sử đơn hàng
Xem chi tiết đơn hàng
Quản lý hồ sơ cá nhân
Đổi mật khẩu
Trang Về chúng tôi
Trang Liên hệ
Form gửi tin nhắn liên hệ
🛡️ Quản trị (/Admin)

Dashboard

Doanh thu
Số lượng đơn hàng
Sản phẩm sắp hết hàng
Biểu đồ doanh thu 6 tháng

Quản lý danh mục

Thêm danh mục
Sửa danh mục
Xóa danh mục
Chặn xóa khi danh mục vẫn còn sản phẩm

Quản lý sản phẩm

Thêm sản phẩm
Sửa sản phẩm
Xóa sản phẩm
Upload ảnh chính
Upload gallery
Tự động sinh slug

Quản lý đơn hàng

Xem danh sách đơn hàng
Lọc theo trạng thái
Cập nhật trạng thái đơn hàng

Quản lý khách hàng

Xem thông tin khách hàng
Xem số lượng đơn hàng

Thống kê

Doanh thu 12 tháng
Sản phẩm bán chạy
Biểu đồ Chart.js
🎨 Giao diện
Palette gradient tím – hồng – cam
Kết hợp màu trắng và xám nhẹ
Phong cách trẻ trung, hiện đại
Font chữ:
Inter
Sora
Corner radius mềm mại từ 22–28px
Animation:
Hero float
Floating chip
Hover product card
Reveal on scroll
Responsive từ:
Mobile 375px
Tablet
Desktop 1440px
⚙️ Hướng dẫn cài đặt
1. Yêu cầu hệ thống
.NET 8.0 SDK
Visual Studio 2022 hoặc VS Code
SQL Server 2022
SQL Server Management Studio (SSMS)
2. Clone hoặc tải source code

Mở terminal và di chuyển đến thư mục dự án:

cd iStore

3. Khôi phục package
dotnet restore

4. Cấu hình cơ sở dữ liệu

Kiểm tra connection string trong:

appsettings.json


Đảm bảo thông tin kết nối SQL Server phù hợp với môi trường đang sử dụng.

5. Tạo database

Nếu sử dụng Entity Framework Core Migration:

dotnet ef database update


Hoặc sử dụng file SQL có sẵn:

Database/AppleStoreDb.sql


Sau khi database được tạo, chạy ứng dụng để khởi tạo dữ liệu mặc định nếu DbInitializer được cấu hình.

6. Chạy ứng dụng
dotnet run

7. Truy cập website
HTTP: http://localhost:5000
HTTPS: https://localhost:7211

Port thực tế có thể khác tùy theo cấu hình trong Properties/launchSettings.json.

🛠️ Troubleshooting
1. Lỗi 403 — Access Denied

Kiểm tra:

Đã đăng nhập bằng tài khoản Admin chưa?
Tài khoản có role Admin chưa?
Database đã được khởi tạo đầy đủ chưa?

Nếu cần tạo lại database bằng Entity Framework:

dotnet ef database update

2. Lỗi database

Thử chạy:

dotnet ef database update


Nếu vẫn xảy ra lỗi, có thể xóa database hiện tại rồi tạo lại.

Nếu database là SQL Server, có thể sử dụng:

ALTER DATABASE [AppleStoreDb]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE [AppleStoreDb];


Sau đó chạy lại:

dotnet run

3. Lỗi build

Thử clean và build lại project:

dotnet clean
dotnet build


Nếu vẫn lỗi, kiểm tra:

.NET SDK đã được cài đặt chưa
Version SDK có phù hợp với project không
Các NuGet package đã restore thành công chưa
4. Lỗi port đã được sử dụng

Nếu port 5000 hoặc port HTTPS đang bị sử dụng, có thể dừng process:

taskkill /f /im dotnet.exe


Hoặc thay đổi port trong:

Properties/launchSettings.json

5. Kết nối SSL / SQL Server bị lỗi

Kiểm tra connection string và đảm bảo có:

TrustServerCertificate=True


Ví dụ:

Server=localhost;Database=AppleStoreDb;Trusted_Connection=True;TrustServerCertificate=True;

🐞 Debug Commands
Xóa database SQL Server

Chạy trong SQL Server Management Studio hoặc sqlcmd:

ALTER DATABASE [AppleStoreDb]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE [AppleStoreDb];


Sau đó chạy lại:

dotnet run

Kiểm tra / thay đổi port

Mở:

Properties/launchSettings.json


Sau đó kiểm tra cấu hình applicationUrl.

Kiểm tra kết nối database

Đảm bảo:

SQL Server đang chạy
Database server có thể truy cập
Database name chính xác
Username/password hoặc Windows Authentication chính xác
TrustServerCertificate=True nếu môi trường yêu cầu
📞 Hỗ trợ

Nếu gặp vấn đề trong quá trình chạy dự án, hãy kiểm tra lần lượt:

Log hiển thị trong console.
Database AppleStoreDb có tồn tại hay chưa.
Connection string trong appsettings.json.
Tài khoản Admin đã được tạo hay chưa.
Role Admin đã được gán đúng hay chưa.
Port trong launchSettings.json.
👨‍💻 Thông tin dự án
Sinh viên thực hiện: Lâm Văn Chỉ
Số điện thoại: 0942 473 373
Email: lamchi93@gmail.com
Giảng viên hướng dẫn: TS. Đoàn Phước Miền
Thời gian thực hiện: Tháng 6 – 8/2026
<p align="center"> <strong>iStore — Apple Technology E-commerce Website</strong> </p> <p align="center"> Built with ASP.NET Core MVC 8 & C# .NET 8 </p>
