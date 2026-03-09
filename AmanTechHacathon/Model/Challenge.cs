using AmanTechHackathonWebApi.MetaData;
using Microsoft.AspNetCore.Mvc;

namespace AmanTechHackathon.Model
{
    [ModelMetadataType(typeof(ChallengeMetaData))]
    public class Challenge : Auditable
    {
        public int ID { get; set; }

        public string Code { get; set; } = "";
        public string Name { get; set; } = "";

        public ICollection<Member> Members { get; set; } = new HashSet<Member>();
    }
}
