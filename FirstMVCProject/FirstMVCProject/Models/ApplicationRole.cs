using Microsoft.AspNetCore.Identity;

namespace FirstMVCProject.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }
}
