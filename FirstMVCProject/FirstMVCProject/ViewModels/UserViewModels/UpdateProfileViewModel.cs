namespace FirstMVCProject.ViewModels.UserViewModels
{
    public class UpdateProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ExistingImageUrl { get; set; } // To display the existing image if available
        public IFormFile? ProfileImage { get; set; }
    }
}
