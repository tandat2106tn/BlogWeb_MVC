using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "氏名")]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [Display(Name = "生年月日")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "住所")]
        [StringLength(200)]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "電話番号")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "学歴")]
        [Display(Name = "Học vấn")]
        [StringLength(500)]
        public string Education { get; set; }
        
        [Display(Name = "職歴")]
        [StringLength(1000)]
        public string WorkExperience { get; set; }
        
        [Display(Name = "資格")]
        [StringLength(500)]
        public string Skills { get; set; }
    }
}