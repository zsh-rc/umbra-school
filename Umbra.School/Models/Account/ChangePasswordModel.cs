using System.ComponentModel.DataAnnotations;

namespace Umbra.School.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码必须保持一致。")]
        public string ConfirmPassword { get; set; } = "";
    }
}
