using System.ComponentModel.DataAnnotations;

namespace FirstMVCProject.ViewModels.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage = "Please enter the 6-digit authenticator code.")]
        [StringLength(7, ErrorMessage = "Code must be at least 6 characters.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator Code")]
        public string TwoFactorCode { get; set; } = string.Empty;

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
