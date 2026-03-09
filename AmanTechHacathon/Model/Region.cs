using AmanTechHackathonWebApi.MetaData;
using Microsoft.AspNetCore.Mvc;

namespace AmanTechHackathon.Model
{
    [ModelMetadataType(typeof(RegionMetaData))]
    public class Region : Auditable
    {
        public int ID { get; set; }

        public string Code { get; set; } = "";
        public string Name { get; set; } = "";

        public ICollection<Member> Members { get; set; } = new HashSet<Member>();
    }

}
