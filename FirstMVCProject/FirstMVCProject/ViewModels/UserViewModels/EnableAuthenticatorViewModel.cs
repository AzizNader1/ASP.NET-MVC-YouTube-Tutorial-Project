using System.ComponentModel.DataAnnotations;

namespace FirstMVCProject.ViewModels.UserViewModels
{
    public class EnableAuthenticatorViewModel
    {
        [Required(ErrorMessage = "Verification code is required.")]
        [StringLength(7, ErrorMessage = "The code must be at least {2} and at most {1} characters.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; } = string.Empty;

        public string? SharedKey { get; set; }
        public string? AuthenticatorUri { get; set; }
    }
}
