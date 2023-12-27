using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModel
{
    public class EditProfileVM
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPass { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPass { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPass")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPass { get; set; }
    }
}
