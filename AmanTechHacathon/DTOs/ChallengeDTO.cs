using AmanTechHackathonWebApi.MetaData;
using Microsoft.AspNetCore.Mvc;

namespace AmanTechHackathonWebApi.DTOs
{
    [ModelMetadataType(typeof(ChallengeMetaData))]
    public class ChallengeDTO
    {
        public int ID { get; set; }

        public string Code { get; set; } = "";
        public string Name { get; set; } = "";

        public byte[]? RowVersion { get; set; }
        public ICollection<MemberDTO>? Members { get; set; }

    }
}
