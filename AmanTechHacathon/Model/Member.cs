using AmanTechHackathonWebApi.MetaData;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AmanTechHackathon.Model
{
    [ModelMetadataType(typeof(MemberMetaData))]
    public class Member : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        public string Summary => FormalName + " - " + MCode;

        public string FullName =>
            FirstName +
            (string.IsNullOrEmpty(MiddleName) ? " " :
                (" " + MiddleName[0] + ". ").ToUpper()) +
            LastName;

        public string FormalName =>
            LastName + ", " + FirstName +
            (string.IsNullOrEmpty(MiddleName) ? "" :
                (" " + MiddleName[0] + ".").ToUpper());

        public string MCode => "M:" + MemberCode.PadLeft(7, '0');

        public string Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month ||
                        (today.Month == DOB.Month && today.Day < DOB.Day)) ? 1 : 0);
                return a.ToString();
            }
        }
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
        public Region? Regions { get; set; }


        public int ChallengeID { get; set; }
        public Challenge? Challenges { get; set; }

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
