using System.ComponentModel.DataAnnotations;

namespace CatalogCrud.Web.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
    }
}