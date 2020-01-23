using System.ComponentModel.DataAnnotations;

namespace KGERP.Models
{
    public class UserLogin
    {
        [Display(Name = "Member ID")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Member ID is required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}