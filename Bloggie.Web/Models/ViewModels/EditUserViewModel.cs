using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public string Id { get; set; }

    [Required]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Mật khẩu hiện tại")]
    public string? CurrentPassword { get; set; }

    [Display(Name = "Mật khẩu mới")]
    [StringLength(100, MinimumLength = 6)]
    public string? NewPassword { get; set; }

    [Display(Name = "Xác nhận mật khẩu mới")]
    [Compare("NewPassword")]
    public string? ConfirmNewPassword { get; set; }

    // Thông tin profile
    public UserProfile Profile { get; set; }
}