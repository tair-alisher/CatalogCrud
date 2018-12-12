using System.ComponentModel.DataAnnotations;

namespace CatalogCrud.Web.Models.Account
{
    public class ChangeEmailModel
    {
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}