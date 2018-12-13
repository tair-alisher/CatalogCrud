using System.ComponentModel.DataAnnotations;

namespace CatalogCrud.Web.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Подтвердите новый пароль")]
        public string ConfirmPassword { get; set; }
    }
}