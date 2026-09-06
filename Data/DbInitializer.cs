using AppleStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Data;

public static class DbInitializer
{
    private const string Unsplash = "https://images.unsplash.com/";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        var admin = await userManager.FindByEmailAsync("admin@istore.vn");
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin@istore.vn",
                Email = "admin@istore.vn",
                FullName = "Quản trị viên",
                PhoneNumber = "0901234567",
                Address = "1 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now.AddMonths(-6)
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var demoUser = await userManager.FindByEmailAsync("user@istore.vn");
        if (demoUser is null)
        {
            demoUser = new ApplicationUser
            {
                UserName = "user@istore.vn",
                Email = "user@istore.vn",
                FullName = "Nguyễn Minh Anh",
                PhoneNumber = "0912345678",
                Address = "25 Lê Lợi, Quận 1, TP. Hồ Chí Minh",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now.AddMonths(-3)
            };
            await userManager.CreateAsync(demoUser, "User@123");
            await userManager.AddToRoleAsync(demoUser, "User");
        }

        if (await context.Categories.AnyAsync())
            return;

        var categories = new List<Category>
        {
            new() { Name = "iPhone", Slug = "iphone", Description = "Điện thoại iPhone chính hãng", Icon = "phone", DisplayOrder = 1 },
            new() { Name = "iPad", Slug = "ipad", Description = "Máy tính bảng iPad", Icon = "tablet", DisplayOrder = 2 },
            new() { Name = "MacBook", Slug = "macbook", Description = "Máy tính xách tay Mac", Icon = "laptop", DisplayOrder = 3 },
            new() { Name = "Apple Watch", Slug = "apple-watch", Description = "Đồng hồ thông minh Apple Watch", Icon = "watch", DisplayOrder = 4 },
            new() { Name = "AirPods", Slug = "airpods", Description = "Tai nghe không dây AirPods", Icon = "headphones", DisplayOrder = 5 },
            new() { Name = "Phụ kiện", Slug = "phu-kien", Description = "Phụ kiện chính hãng Apple", Icon = "grid", DisplayOrder = 6 }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var iphone = categories.First(c => c.Slug == "iphone");
        var ipad = categories.First(c => c.Slug == "ipad");
        var mac = categories.First(c => c.Slug == "macbook");
        var watch = categories.First(c => c.Slug == "apple-watch");
        var airpods = categories.First(c => c.Slug == "airpods");
        var accessory = categories.First(c => c.Slug == "phu-kien");

        var products = new List<Product>
        {
            Create("iPhone 17", "iphone-17", iphone.Id, 22990000, 21990000, 48, true,
                "iPhone 17. Thiết kế mới, camera mạnh mẽ và thời lượng pin vượt trội.",
                "iPhone 17 mang đến trải nghiệm iPhone hoàn thiện nhất từ trước đến nay với màn hình Super Retina XDR sống động, chip A19 và hệ thống camera 48MP.",
                "Màn hình: Super Retina XDR 6.3\"\nChip: A19\nCamera: Hệ thống 48MP\nPin: Lên đến 27 giờ xem video\nKết nối: USB-C, 5G",
                "Đen,Trắng,Xanh,Hồng", "128GB,256GB,512GB",
                Img("photo-1695048133142-1a20484d2569"), 18,
                Img("photo-1592286927505-1def25115558"), Img("photo-1511707171634-5f897ff02aa9")),
            Create("iPhone 17 Pro", "iphone-17-pro", iphone.Id, 28990000, null, 32, true,
                "iPhone 17 Pro. Titanium. Camera Pro. Hiệu năng đỉnh cao.",
                "iPhone 17 Pro với khung titanium, camera tele và chip A19 Pro dành cho những người dùng cần sức mạnh tối đa.",
                "Màn hình: Super Retina XDR 6.3\" ProMotion 120Hz\nChip: A19 Pro\nCamera: 48MP chính + Ultra Wide + Tele\nPin: Lên đến 29 giờ xem video",
                "Đen,Bạc,Cam Desert", "256GB,512GB,1TB",
                Img("photo-1678652197831-2d180705cd2c"), 20,
                Img("photo-1605236453806-6ff36851218e"), Img("photo-1592899677977-9c10ca588bbd")),
            Create("iPhone 17 Pro Max", "iphone-17-pro-max", iphone.Id, 34990000, 33490000, 25, true,
                "iPhone 17 Pro Max. Màn hình lớn nhất. Pin khỏe nhất.",
                "Phiên bản Pro Max với màn hình 6.9 inch, zoom quang học mạnh và thời lượng pin cả ngày cho công việc lẫn giải trí.",
                "Màn hình: Super Retina XDR 6.9\" ProMotion\nChip: A19 Pro\nCamera: 48MP Fusion + Tele 5x\nPin: Lên đến 33 giờ xem video",
                "Đen,Bạc,Cam Desert", "256GB,512GB,1TB",
                Img("photo-1663499482523-1c0c1bae4ce1"), 25,
                Img("photo-1580910051074-3eb694886505"), Img("photo-1616348436168-de43ad0db179")),
            Create("iPhone Air", "iphone-air", iphone.Id, 24990000, null, 40, true,
                "iPhone Air. Mỏng hơn. Nhẹ hơn. Đủ mạnh mẽ.",
                "iPhone Air kết hợp thiết kế siêu mỏng với hiệu năng A19, dành cho người muốn sự tinh gọn tuyệt đối.",
                "Màn hình: Super Retina XDR 6.1\"\nChip: A19\nCamera: 48MP\nTrọng lượng: Siêu nhẹ",
                "Bạc,Xanh nhạt,Vàng", "128GB,256GB,512GB",
                Img("photo-1567581935884-3349723552ca"), 15,
                Img("photo-1574756153922-52c1eb35eacf"), Img("photo-1621330396173-e41b1cafd17f")),
            Create("iPhone 16", "iphone-16", iphone.Id, 19990000, 18490000, 55, false,
                "iPhone 16. Camera Control. Chip A18. Màu sắc tươi mới.",
                "iPhone 16 vẫn là lựa chọn cân bằng giữa hiệu năng, camera và mức giá phải chăng.",
                "Màn hình: Super Retina XDR 6.1\"\nChip: A18\nCamera: 48MP Fusion",
                "Đen,Hồng,Xanh lục,Trắng", "128GB,256GB",
                Img("photo-1592750475338-74b7b21085ab"), 40,
                Img("photo-1546027658-7aa750153465"), Img("photo-1523206489230-c012c64b2b48")),
            Create("iPhone 16e", "iphone-16e", iphone.Id, 16990000, null, 60, false,
                "iPhone 16e. Trải nghiệm iPhone với mức giá dễ tiếp cận hơn.",
                "iPhone 16e giữ những điều cốt lõi: chip mạnh, camera đẹp và iOS mượt mà.",
                "Màn hình: Super Retina XDR 6.1\"\nChip: A18\nCamera: 48MP",
                "Đen,Trắng", "128GB,256GB",
                Img("photo-1510557880182-3d4d3cba35a5"), 50,
                Img("photo-1512054502232-10a0a035d672"), null),

            Create("MacBook Air 13\"", "macbook-air-13", mac.Id, 27990000, 26490000, 22, true,
                "MacBook Air 13\". Mỏng nhẹ, yên tĩnh, mạnh mẽ với chip M4.",
                "MacBook Air 13 inch là người bạn đồng hành lý tưởng cho học tập và công việc với thiết kế không quạt và pin cả ngày.",
                "Màn hình: Liquid Retina 13.6\"\nChip: Apple M4\nRAM: 16GB\nỔ cứng: 256GB/512GB\nPin: Lên đến 18 giờ",
                "Bạc,Xám không gian,Xanh midnight,Vàng starlight", "256GB,512GB,1TB",
                Img("photo-1611186871348-b1ce696e52c9"), 30,
                Img("photo-1541807084-5c52b6b3adef"), Img("photo-1496181133206-80ce9b88a853")),
            Create("MacBook Air 15\"", "macbook-air-15", mac.Id, 32990000, null, 18, true,
                "MacBook Air 15\". Không gian lớn hơn, vẫn mỏng nhẹ.",
                "Màn hình 15.3 inch sống động cùng chip M4 giúp bạn làm việc, xem phim và sáng tạo thoải mái.",
                "Màn hình: Liquid Retina 15.3\"\nChip: Apple M4\nRAM: 16GB\nPin: Lên đến 18 giờ",
                "Bạc,Xám không gian,Xanh midnight", "256GB,512GB,1TB",
                Img("photo-1517336714731-489689fd1ca8"), 22,
                Img("photo-1531297484001-80022131f5a1"), Img("photo-1629429407756-446d68172d05")),
            Create("MacBook Pro 14\"", "macbook-pro-14", mac.Id, 45990000, 43990000, 14, true,
                "MacBook Pro 14\". Cho chuyên gia cần hiệu năng thực sự.",
                "Liquid Retina XDR, chip M4 Pro và hệ thống loa sáu loa giúp MacBook Pro 14 inch trở thành công cụ sáng tạo đỉnh cao.",
                "Màn hình: Liquid Retina XDR 14.2\"\nChip: Apple M4 Pro\nRAM: 24GB\nCổng: HDMI, MagSafe, SDXC",
                "Bạc,Đen space black", "512GB,1TB,2TB",
                Img("photo-1517059224940-d4af9eec41b7"), 12,
                Img("photo-1544731612-de7f96afe55f"), Img("photo-1629131726692-1accd0c53ce0")),
            Create("MacBook Pro 16\"", "macbook-pro-16", mac.Id, 59990000, null, 10, true,
                "MacBook Pro 16\". Màn hình lớn nhất. Sức mạnh studio.",
                "Dành cho dựng video, lập trình nặng và thiết kế 3D với chip M4 Max và pin bền bỉ.",
                "Màn hình: Liquid Retina XDR 16.2\"\nChip: Apple M4 Max\nRAM: 36GB\nPin: Lên đến 22 giờ",
                "Bạc,Đen space black", "512GB,1TB,2TB",
                Img("photo-1587614382346-4ec70e388b28"), 8,
                Img("photo-1517336714731-489689fd1ca8"), Img("photo-1541807084-5c52b6b3adef")),

            Create("iPad", "ipad", ipad.Id, 11990000, 10990000, 35, true,
                "iPad. Toàn diện cho học tập, giải trí và sáng tạo.",
                "iPad với chip A16, màn hình Liquid Retina và hỗ trợ Apple Pencil, hoàn hảo cho học sinh và gia đình.",
                "Màn hình: Liquid Retina 11\"\nChip: A16\nCamera: 12MP\nKết nối: USB-C",
                "Bạc,Xanh,Hồng,Vàng", "128GB,256GB",
                Img("photo-1585790050230-5dd28404ccb9"), 28,
                Img("photo-1587033411391-5d9e51cce126"), Img("photo-1561154464-82e9adf32764")),
            Create("iPad Air", "ipad-air", ipad.Id, 16990000, null, 24, true,
                "iPad Air. Mỏng, mạnh, linh hoạt với chip M3.",
                "iPad Air cân bằng hoàn hảo giữa hiệu năng máy tính bảng và sự cơ động.",
                "Màn hình: Liquid Retina 11\" / 13\"\nChip: Apple M3\nHỗ trợ: Apple Pencil Pro, Magic Keyboard",
                "Xám không gian,Xanh,Tím,Vàng starlight", "128GB,256GB,512GB",
                Img("photo-1622533950014-6cca6ffe3f95"), 16,
                Img("photo-1544244015-0df4b3ffc6b0"), Img("photo-1557825835-70d97c4aa567")),
            Create("iPad Pro", "ipad-pro", ipad.Id, 28990000, 27490000, 16, true,
                "iPad Pro. Màn hình Ultra Retina XDR. Chip M4.",
                "iPad Pro thay thế laptop cho nhiều công việc chuyên nghiệp với màn hình Tandem OLED và Apple Pencil Pro.",
                "Màn hình: Ultra Retina XDR 11\" / 13\"\nChip: Apple M4\nCamera: 12MP ngang\nFace ID",
                "Bạc,Đen space black", "256GB,512GB,1TB",
                Img("photo-1544244015-0df4b3ffc6b0"), 11,
                Img("photo-1557825835-70d97c4aa567"), Img("photo-1585790050230-5dd28404ccb9")),
            Create("iPad mini", "ipad-mini", ipad.Id, 13990000, null, 20, false,
                "iPad mini. Sức mạnh đầy đủ trong lòng bàn tay.",
                "Nhỏ gọn nhưng mạnh mẽ với chip A17 Pro, lý tưởng để đọc, ghi chú và giải trí khi di chuyển.",
                "Màn hình: Liquid Retina 8.3\"\nChip: A17 Pro\nHỗ trợ Apple Pencil Pro",
                "Xám không gian,Tím,Vàng starlight,Xanh", "128GB,256GB",
                Img("photo-1587033411391-5d9e51cce126"), 19,
                Img("photo-1561154464-82e9adf32764"), null),

            Create("Apple Watch Series 11", "apple-watch-series-11", watch.Id, 10990000, 10490000, 30, true,
                "Apple Watch Series 11. Sức khỏe trên cổ tay bạn.",
                "Theo dõi tim mạch, giấc ngủ, vận động và kết nối iPhone liền mạch trên màn hình Always-On sáng hơn.",
                "Màn hình: Always-On Retina\nChip: S10\nCảm biến: Tim, SpO2, nhiệt độ\nChống nước: 50m",
                "Đen,Bạc,Vàng hồng", "41mm,45mm",
                Img("photo-1551816230-ef5deaed4a26"), 21,
                Img("photo-1546868871-7041f2a55e12"), Img("photo-1523275335684-37898b6baf30")),
            Create("Apple Watch Ultra 2", "apple-watch-ultra-2", watch.Id, 21990000, null, 12, true,
                "Apple Watch Ultra 2. Cho những cuộc phiêu lưu khắc nghiệt.",
                "Vỏ titanium, pin 36 giờ và GPS chính xác dành cho vận động viên và người thích khám phá.",
                "Màn hình: 3000 nit\nVỏ: Titanium\nPin: Lên đến 36 giờ\nĐộ sâu: 100m",
                "Natural,Black", "49mm",
                Img("photo-1579586337278-3befd40fd17a"), 9,
                Img("photo-1617625802921-b3f9b57a8b8e"), null),
            Create("Apple Watch SE", "apple-watch-se", watch.Id, 6990000, 6490000, 40, false,
                "Apple Watch SE. Những tính năng cốt lõi với mức giá tốt.",
                "Nhận cuộc gọi, theo dõi vận động và dùng Apple Pay trên thiết kế quen thuộc.",
                "Màn hình: Retina\nChip: S8\nPhát hiện té ngã và tai nạn",
                "Đen,Bạc,Vàng starlight", "40mm,44mm",
                Img("photo-1546868871-7041f2a55e12"), 33,
                Img("photo-1551816230-ef5deaed4a26"), null),

            Create("AirPods 4", "airpods-4", airpods.Id, 3490000, 3290000, 50, true,
                "AirPods 4. Âm thanh sống động. Đeo vừa vặn cả ngày.",
                "Thiết kế mới, chip H2 và chống ồn chủ động trên phiên bản tùy chọn.",
                "Chip: H2\nThời lượng: 5 giờ nghe / 30 giờ với hộp\nCổng: USB-C",
                "Trắng", "",
                Img("photo-1600294037681-c80b4cb5b434"), 45,
                Img("photo-1588423771073-b8903fbb85b5"), null),
            Create("AirPods Pro 2", "airpods-pro-2", airpods.Id, 5990000, 5490000, 38, true,
                "AirPods Pro 2. Chống ồn chủ động hàng đầu.",
                "Adaptive Audio, Transparency mode và Personalized Spatial Audio cho trải nghiệm đắm chìm.",
                "Chip: H2\nChống ồn: Active Noise Cancellation\nHộp sạc MagSafe USB-C",
                "Trắng", "",
                Img("photo-1606220838315-056192d5e927"), 36,
                Img("photo-1600294037681-c80b4cb5b434"), Img("photo-1590658268037-6bf12165a8df")),
            Create("AirPods Max", "airpods-max", airpods.Id, 12990000, null, 15, true,
                "AirPods Max. Over-ear. Hi-fi. Thiết kế cao cấp.",
                "Driver dynamic 40mm, Digital Crown và khung nhôm CNC cho âm thanh studio.",
                "Driver: 40mm\nANC: Có\nThời lượng: 20 giờ",
                "Bạc,Xám không gian,Xanh,Tím,Cam", "",
                Img("photo-1610438235354-a6ae5528385c"), 10,
                Img("photo-1546435770-a3e426bf472b"), Img("photo-1583394838336-acd977736f90")),

            Create("Apple Pencil Pro", "apple-pencil-pro", accessory.Id, 3490000, 3290000, 28, false,
                "Apple Pencil Pro. Viết, vẽ, điều khiển bằng cử chỉ.",
                "Barrel roll, haptic và Find My giúp Apple Pencil Pro trở thành công cụ sáng tạo chính xác.",
                "Tương thích: iPad Pro, iPad Air, iPad mini mới\nSạc: Nam châm",
                "Trắng", "",
                Img("photo-1585771362604-ce149b348a70"), 24,
                Img("photo-1585771362604-ce149b348a70"), null),
            Create("Magic Keyboard", "magic-keyboard", accessory.Id, 7990000, null, 18, false,
                "Magic Keyboard. Trackpad, phím backlight, cổng USB-C.",
                "Biến iPad thành máy tính xách tay với trải nghiệm gõ phím desktop.",
                "Tương thích: iPad Pro / iPad Air\nTrackpad: Multi-Touch\nGóc nghiêng: Có",
                "Đen,Trắng", "",
                Img("photo-1587829741301-dc798b83add3"), 14,
                Img("photo-1587829741301-dc798b83add3"), null),
            Create("Sạc MagSafe", "magsafe-charger", accessory.Id, 1190000, 990000, 70, false,
                "Sạc MagSafe. Gắn nam châm. Sạc nhanh không dây.",
                "Lên đến 15W cho iPhone, thiết kế gọn và cáp USB-C.",
                "Công suất: 15W\nCổng: USB-C\nTương thích MagSafe",
                "Trắng", "",
                Img("photo-1615526675250-f7bc4c34754d"), 60,
                Img("photo-1615526675250-f7bc4c34754d"), null),
            Create("AirTag", "airtag", accessory.Id, 790000, null, 80, false,
                "AirTag. Tìm đồ thất lạc dễ dàng với Mạng lưới Tìm.",
                "Precision Finding trên iPhone giúp bạn tìm chìa khóa, balo hay vali nhanh chóng.",
                "Pin: CR2032 ~ 1 năm\nKháng nước: IP67\nPrecision Finding: UWB",
                "Trắng", "1 pack,4 pack",
                Img("photo-1620750573680-3ab21eb56ff9"), 70,
                Img("photo-1620750573680-3ab21eb56ff9"), null)
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // ProductImages đã được lưu qua Product.Images navigation ở SaveChangesAsync trước
        // Bổ sung ảnh chính làm ảnh đầu tiên trong gallery nếu chưa có
        foreach (var product in products)
        {
            var hasMain = await context.ProductImages
                .AnyAsync(x => x.ProductId == product.Id && x.ImageUrl == product.MainImage);
            if (!hasMain && !string.IsNullOrEmpty(product.MainImage))
            {
                context.ProductImages.Add(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = product.MainImage!,
                    DisplayOrder = 1
                });
            }
        }
        await context.SaveChangesAsync();

        if (demoUser is not null && !await context.Orders.AnyAsync())
        {
            var sampleProduct = products.First(p => p.Slug == "iphone-17");
            var sampleAirpods = products.First(p => p.Slug == "airpods-pro-2");
            var order = new Order
            {
                OrderCode = "ORD20260801001",
                UserId = demoUser.Id,
                ReceiverName = demoUser.FullName,
                Phone = demoUser.PhoneNumber ?? "0912345678",
                Address = demoUser.Address ?? "TP. Hồ Chí Minh",
                Note = "Giao giờ hành chính giúp mình.",
                TotalAmount = sampleProduct.DisplayPrice + sampleAirpods.DisplayPrice,
                Status = OrderStatus.Completed,
                PaymentMethod = "COD",
                CreatedAt = DateTime.Now.AddDays(-18),
                Details = new List<OrderDetail>
                {
                    new()
                    {
                        ProductId = sampleProduct.Id,
                        ProductName = sampleProduct.Name,
                        ProductImage = sampleProduct.MainImage,
                        Color = "Đen",
                        Storage = "256GB",
                        Quantity = 1,
                        UnitPrice = sampleProduct.DisplayPrice
                    },
                    new()
                    {
                        ProductId = sampleAirpods.Id,
                        ProductName = sampleAirpods.Name,
                        ProductImage = sampleAirpods.MainImage,
                        Color = "Trắng",
                        Quantity = 1,
                        UnitPrice = sampleAirpods.DisplayPrice
                    }
                }
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
    }

    private static string Img(string photoId) =>
        $"{Unsplash}{photoId}?w=1200&q=80&auto=format&fit=crop";

    private static Product Create(
        string name, string slug, int categoryId, decimal price, decimal? sale,
        int stock, bool featured, string shortDesc, string desc, string specs,
        string colors, string storage, string image, int daysAgo,
        string? extra1 = null, string? extra2 = null)
    {
        var product = new Product
        {
            Name = name,
            Slug = slug,
            CategoryId = categoryId,
            Price = price,
            SalePrice = sale,
            Stock = stock,
            IsFeatured = featured,
            IsActive = true,
            ShortDescription = shortDesc,
            Description = desc,
            Specifications = specs,
            ColorOptions = colors,
            StorageOptions = storage,
            MainImage = image,
            CreatedAt = DateTime.Now.AddDays(-daysAgo)
        };

        if (!string.IsNullOrEmpty(extra1))
            product.Images.Add(new ProductImage { ImageUrl = extra1, DisplayOrder = 2 });
        if (!string.IsNullOrEmpty(extra2))
            product.Images.Add(new ProductImage { ImageUrl = extra2, DisplayOrder = 3 });

        return product;
    }
}
