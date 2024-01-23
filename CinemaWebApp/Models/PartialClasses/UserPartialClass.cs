using CinemaWebApp.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
