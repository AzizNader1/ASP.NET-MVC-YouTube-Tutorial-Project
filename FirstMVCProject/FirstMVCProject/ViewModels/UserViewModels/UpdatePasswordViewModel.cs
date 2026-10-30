using System.ComponentModel.DataAnnotations;

namespace FirstMVCProject.ViewModels.UserViewModels
{
    public class UpdatePasswordViewModel
    {
        [Required(ErrorMessage = "The Current Password Is required")]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password should has at least 8 characters")]
        [MaxLength(24)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
