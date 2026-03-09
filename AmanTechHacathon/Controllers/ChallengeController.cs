using AmanTechHackathon.Data;
using AmanTechHackathonWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmanTechHackathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly HacathonContext _context;

        public ChallengeController(HacathonContext context)
        {
            _context = context;
        }

        // GET: api/Challenge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallenges()
        {
            var challengesDTOs = await _context.Challenges
                .Include(c => c.Members)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion
                })
                .ToListAsync();


            if (challengesDTOs.Count() > 0)
            {
                return challengesDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Challenges records found in the database." });
            }
        }

        // GET: api/Challenge/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeDTO>> GetChallenge(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Members)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion
                })
               .FirstOrDefaultAsync(c => c.ID == id);

            if (challenge == null)
            {
                return NotFound(new { message = "Error: No Challenge records found in the database." });
            }

            return challenge;
        }


        // GET: api/Challenge/inc
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallengesInclude()
        {
            var challengesDTOs = await _context.Challenges
                .Include(c => c.Members)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Members = c.Members.Any() ? c.Members.Select(c => new MemberDTO
                    {
                        ID = c.ID,
                        FirstName = c.FirstName,
                        MiddleName = c.MiddleName,
                        LastName = c.LastName,
                        MemberCode = c.MemberCode,
                        DOB = c.DOB,
                        SkillRating = c.SkillRating,
                        YearsExperience = c.YearsExperience,
                        Category = c.Category,
                        Organization = c.Organization,
                        RegionID = c.RegionID,
                        ChallengeID = c.ChallengeID,
                        RowVersion = c.RowVersion
                    }).ToList()
                    : null
                })
                .ToListAsync();


            if (challengesDTOs.Count() > 0)
            {
                return challengesDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Challenges records found in the database." });
            }
        }

        // GET: api/Challenge/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ChallengeDTO>> GetChallengeInclude(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Members)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Members = c.Members.Any() ? c.Members.Select(c => new MemberDTO
                    {
                        ID = c.ID,
                        FirstName = c.FirstName,
                        MiddleName = c.MiddleName,
                        LastName = c.LastName,
                        MemberCode = c.MemberCode,
                        DOB = c.DOB,
                        SkillRating = c.SkillRating,
                        YearsExperience = c.YearsExperience,
                        Category = c.Category,
                        Organization = c.Organization,
                        RegionID = c.RegionID,
                        ChallengeID = c.ChallengeID,
                        RowVersion = c.RowVersion
                    }).ToList()
                    : null
                })
               .FirstOrDefaultAsync(c => c.ID == id);

            if (challenge == null)
            {
                return NotFound(new { message = "Error: No Challenge records found in the database." });
            }

            return challenge;
        }


        #region  Commentented out some methods for project simulation 
        //// PUT: api/Challenge/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChallenge(int id, Challenge challenge)
        //{
        //    if (id != challenge.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(challenge).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChallengeExists(id))
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

        //// POST: api/Challenge
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Challenge>> PostChallenge(Challenge challenge)
        //{
        //    _context.Challenges.Add(challenge);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetChallenge", new { id = challenge.ID }, challenge);
        //}

        //// DELETE: api/Challenge/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteChallenge(int id)
        //{
        //    var challenge = await _context.Challenges.FindAsync(id);
        //    if (challenge == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Challenges.Remove(challenge);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        #endregion


        private bool ChallengeExists(int id)
        {
            return _context.Challenges.Any(e => e.ID == id);
        }
    }
}
