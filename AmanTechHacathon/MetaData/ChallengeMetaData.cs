using System.ComponentModel.DataAnnotations;

namespace AmanTechHackathonWebApi.MetaData
{
    public class ChallengeMetaData
    {

        [Display(Name = "Challenge Code")]
        [Required(ErrorMessage = "Challenge code is required.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Challenge code must be exactly 3 capital letters.")]
        [StringLength(3)]
        public string Code { get; set; } = "";

        [Display(Name = "Challenge Name")]
        [Required(ErrorMessage = "Challenge name is required.")]
        [StringLength(50, ErrorMessage = "Challenge name cannot exceed 50 characters.")]
        public string Name { get; set; } = "";


    }
}
