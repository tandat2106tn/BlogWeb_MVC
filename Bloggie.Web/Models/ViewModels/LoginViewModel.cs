using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "mật khẩu phải dài hơn 6 chữ số")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
