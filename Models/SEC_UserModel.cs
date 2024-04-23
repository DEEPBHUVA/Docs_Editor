using System.ComponentModel.DataAnnotations;

namespace Docs_Editor.Models
{
    public class SEC_UserModel
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [MaxLength(8)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
