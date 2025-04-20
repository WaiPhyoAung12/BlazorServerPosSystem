using System.ComponentModel.DataAnnotations;

namespace PosSystem.Models.Login
{
    public class LoginRequestModel
    {
        [Required (ErrorMessage ="Üser name is required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="User name must between 3 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
