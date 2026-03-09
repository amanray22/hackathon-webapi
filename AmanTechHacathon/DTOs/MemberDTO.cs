using AmanTechHackathonWebApi.MetaData;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AmanTechHackathonWebApi.DTOs
{
    [ModelMetadataType(typeof(MemberMetaData))]
    public class MemberDTO : IValidatableObject
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }
        public string LastName { get; set; } = "";

        public string MemberCode { get; set; } = "0000000";
        public DateTime DOB { get; set; }
        public int SkillRating { get; set; }

        public int YearsExperience { get; set; }

        public string Category { get; set; } = "";
        public string Organization { get; set; } = "";

        public int RegionID { get; set; }
        public RegionDTO? Regions { get; set; }

        public int ChallengeID { get; set; }
        public ChallengeDTO? Challenges { get; set; }

        public byte[]? RowVersion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int age = DateTime.Today.Year - DOB.Year;
            if (DOB.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 12 || age > 30)
            {
                yield return new ValidationResult(
                    "Member age must be between 12 and 30.",
                    new[] { "DOB" });
            }
        }
    }

}
