using System.ComponentModel.DataAnnotations;

namespace Fanex.NAP.CMS.ViewModels.UserInfo
{
    public class ProfileViewModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public int GroupId { get; set; }
    }
}
