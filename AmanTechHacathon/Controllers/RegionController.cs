using AmanTechHackathon.Data;
using AmanTechHackathonWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmanTechHackathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly HacathonContext _context;

        public RegionController(HacathonContext context)
        {
            _context = context;
        }

        // GET: api/Region
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegions()
        {
            var regionDTOs = await _context.Regions
                .Include(m => m.Members)
                .Select(m => new RegionDTO
                {
                    ID = m.ID,
                    Code = m.Code,
                    Name = m.Name,
                    RowVersion = m.RowVersion
                })
                .ToListAsync();

            if (regionDTOs.Count() > 0)
            {
                return regionDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Region records found in the database." });
            }
        }

        // GET: api/Region/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegion(int id)
        {
            var region = await _context.Regions
                .Include(m => m.Members)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion

                })
                .FirstOrDefaultAsync(r => r.ID == id);

            if (region == null)
            {
                return NotFound(new { message = "Error: No Region records found in the database." });
            }

            return region;

        }

        // GET: api/Region/inc
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegionsInc()
        {
            var regionDTOs = await _context.Regions
                .Include(m => m.Members)
                .Select(m => new RegionDTO
                {
                    ID = m.ID,
                    Code = m.Code,
                    Name = m.Name,
                    RowVersion = m.RowVersion,
                    Members = m.Members.Any() ? m.Members.Select(mMember => new MemberDTO
                    {
                        ID = mMember.ID,
                        FirstName = mMember.FirstName,
                        MiddleName = mMember.MiddleName,
                        LastName = mMember.LastName,
                        MemberCode = mMember.MemberCode,
                        DOB = mMember.DOB,
                        SkillRating = mMember.SkillRating,
                        YearsExperience = mMember.YearsExperience,
                        Category = mMember.Category,
                        Organization = mMember.Organization,
                        RowVersion = mMember.RowVersion,
                        ChallengeID = mMember.ChallengeID,
                    }).ToList()
                    : null
                })
                .ToListAsync();

            if (regionDTOs.Count() > 0)
            {
                return regionDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Region records found in the database." });
            }
        }

        // GET: api/Region/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegionInc(int id)
        {
            var region = await _context.Regions
                .Include(m => m.Members)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion,
                    Members = r.Members.Any() ? r.Members.Select(rMember => new MemberDTO
                    {
                        ID = rMember.ID,
                        FirstName = rMember.FirstName,
                        MiddleName = rMember.MiddleName,
                        LastName = rMember.LastName,
                        MemberCode = rMember.MemberCode,
                        DOB = rMember.DOB,
                        SkillRating = rMember.SkillRating,
                        YearsExperience = rMember.YearsExperience,
                        Category = rMember.Category,
                        Organization = rMember.Organization,
                        RowVersion = rMember.RowVersion,
                        ChallengeID = rMember.ChallengeID,
                    }).ToList()
                    : null
                })
                .FirstOrDefaultAsync(r => r.ID == id);

            if (region == null)
            {
                return NotFound(new { message = "Error: No Region records found in the database." });
            }

            return region;

        }


        #region Commented out endpoints

        //// PUT: api/Region/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRegion(int id, Region region)
        //{
        //    if (id != region.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(region).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RegionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Region
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Region>> PostRegion(Region region)
        //{
        //    _context.Regions.Add(region);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRegion", new { id = region.ID }, region);
        //}

        //// DELETE: api/Region/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRegion(int id)
        //{
        //    var region = await _context.Regions.FindAsync(id);
        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Regions.Remove(region);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        #endregion


        private bool RegionExists(int id)
        {
            return _context.Regions.Any(e => e.ID == id);
        }
    }
}
