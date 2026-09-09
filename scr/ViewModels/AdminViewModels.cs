using System.ComponentModel.DataAnnotations;
using AppleStore.Models;

namespace AppleStore.ViewModels;

public class ProfileViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
    [StringLength(120)]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [RegularExpression(@"^(0|\+84)[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }
}

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu hiện tại")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    [Display(Name = "Xác nhận mật khẩu mới")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class AdminProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá.")]
    [Range(0, 999999999)]
    public decimal Price { get; set; }

    [Range(0, 999999999)]
    public decimal? SalePrice { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [StringLength(400)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }
    public string? Specifications { get; set; }
    public string? ColorOptions { get; set; }
    public string? StorageOptions { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public string? CurrentImage { get; set; }
    public IFormFile? ImageFile { get; set; }
    public List<IFormFile>? GalleryFiles { get; set; }
}

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int LowStockProducts { get; set; }
    public List<string> ChartLabels { get; set; } = new();
    public List<decimal> ChartValues { get; set; } = new();
    public List<Order> RecentOrders { get; set; } = new();
    public List<Product> LowStockList { get; set; } = new();
}

public class StatisticsViewModel
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int ProductsSold { get; set; }
    public List<BestSellerItem> BestSellers { get; set; } = new();
    public List<string> ChartLabels { get; set; } = new();
    public List<decimal> ChartValues { get; set; } = new();
}

public class BestSellerItem
{
    public string ProductName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}
