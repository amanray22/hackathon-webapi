using AmanTechHackathon.Data;
using AmanTechHackathon.Model;
using AmanTechHackathonWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmanTechHackathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly HacathonContext _context;

        public MemberController(HacathonContext context)
        {
            _context = context;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            var memberDTOs = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RegionID = m.RegionID,
                    ChallengeID = m.ChallengeID,
                    RowVersion = m.RowVersion
                })
                .ToListAsync();

            if (memberDTOs.Count() > 0)
            {
                return memberDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No members found in the database" });
            }
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(int id)
        {
            var memberDTOs = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RegionID = m.RegionID,
                    ChallengeID = m.ChallengeID,
                    RowVersion = m.RowVersion
                })
                .FirstOrDefaultAsync(m => m.ID == id);


            if (memberDTOs == null)
            {
                return NotFound(new { message = "Error: No member found in the database" });
            }

            return memberDTOs;
        }

        // PUT: api/Member/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberDTO memberDTO)
        {
            if (id != memberDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Member" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var memberToUpdate = await _context.Members.FindAsync(id);

            //Check that you got it
            if (memberToUpdate == null)
            {
                return NotFound(new { message = "Error: Member record not found." });
            }

            if (memberDTO.RowVersion != null)
            {
                if (!memberToUpdate.RowVersion.SequenceEqual(memberDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Member has been changed by another user.  Try editing the record again." });
                }
            }

            memberToUpdate.ID = memberDTO.ID;
            memberToUpdate.FirstName = memberDTO.FirstName;
            memberToUpdate.MiddleName = memberDTO.MiddleName;
            memberToUpdate.LastName = memberDTO.LastName;
            memberToUpdate.MemberCode = memberDTO.MemberCode;
            memberToUpdate.DOB = memberDTO.DOB;
            memberToUpdate.SkillRating = memberDTO.SkillRating;
            memberToUpdate.YearsExperience = memberDTO.YearsExperience;
            memberToUpdate.Category = memberDTO.Category;
            memberToUpdate.Organization = memberDTO.Organization;
            memberToUpdate.RegionID = memberDTO.RegionID;
            memberToUpdate.ChallengeID = memberDTO.ChallengeID;
            memberToUpdate.RowVersion = memberDTO.RowVersion;



            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(memberToUpdate).Property("RowVersion").OriginalValue = memberDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Member has been removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Member has been updated by another user.  Back out and try editing the record again." });
                }
            }

            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Member code." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Member
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO memberDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = new Member
            {
                ID = memberDTO.ID,
                FirstName = memberDTO.FirstName,
                MiddleName = memberDTO.MiddleName,
                LastName = memberDTO.LastName,
                MemberCode = memberDTO.MemberCode,
                DOB = memberDTO.DOB,
                SkillRating = memberDTO.SkillRating,
                YearsExperience = memberDTO.YearsExperience,
                Category = memberDTO.Category,
                Organization = memberDTO.Organization,
                RegionID = memberDTO.RegionID,
                ChallengeID = memberDTO.ChallengeID,
                RowVersion = memberDTO.RowVersion
            };

            try
            {
                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                memberDTO.ID = member.ID;
                memberDTO.RowVersion = member.RowVersion;

                return CreatedAtAction(nameof(GetMember), new { id = member.ID }, memberDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Member Code." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Delete Error: Member has already been removed." });
            }

            try
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Member." });
            }

        }


        // GET: api/Member/ByRegion/5
        [HttpGet("ByRegion/{id}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberByRegion(int id)
        {
            var memberDTOs = await _context.Members
                .Where(m => m.RegionID == id)
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RowVersion = m.RowVersion,
                    RegionID = m.RegionID
                }).ToListAsync();


            if (memberDTOs == null)
            {
                return NotFound(new { message = "Error: No member found in the database" });
            }

            return memberDTOs;
        }


        // GET: api/Member/ByChallenge/5
        [HttpGet("ByChallenge/{id}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberByChallenge(int id)
        {
            var memberDTOs = await _context.Members
               .Where(m => m.ChallengeID == id)
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RowVersion = m.RowVersion,
                    ChallengeID = m.ChallengeID
                })
                .ToListAsync();


            if (memberDTOs == null)
            {
                return NotFound(new { message = "Error: No member found in the database" });
            }

            return memberDTOs;
        }


        #region Additional endpoints for member including region and challenges


        // GET: api/Member/inc/Region
        [HttpGet("inc/Region")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersIncludeRegion()
        {
            var memberDTOs = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RowVersion = m.RowVersion,
                    RegionID = m.RegionID,
                    Regions = m.Regions != null ? new RegionDTO
                    {
                        ID = m.Regions.ID,
                        Name = m.Regions.Name,
                        Code = m.Regions.Code,
                        RowVersion = m.RowVersion
                    } : null
                })
                .ToListAsync();

            if (memberDTOs.Count() > 0)
            {
                return memberDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No members found in the database" });
            }
        }

        // GET: api/Member/Inc/Challenge
        [HttpGet("inc/Challenge")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberIncludeChallenge()
        {
            var memberDTOs = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RowVersion = m.RowVersion,
                    ChallengeID = m.ChallengeID,
                    Challenges = m.Challenges != null ? new ChallengeDTO
                    {
                        ID = m.Challenges.ID,
                        Code = m.Challenges.Code,
                        Name = m.Challenges.Name,
                        RowVersion = m.RowVersion
                    } : null
                })
                .ToListAsync();


            if (memberDTOs.Count() > 0)
            {
                return memberDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No members found in the database" });
            }
        }

        // GET: api/Member/Inc/Full
        [HttpGet("inc/Full")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberIncludeBoth()
        {
            var memberDTOs = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    MemberCode = m.MemberCode,
                    DOB = m.DOB,
                    SkillRating = m.SkillRating,
                    YearsExperience = m.YearsExperience,
                    Category = m.Category,
                    Organization = m.Organization,
                    RowVersion = m.RowVersion,
                    RegionID = m.RegionID,
                    Regions = m.Regions != null ? new RegionDTO
                    {
                        ID = m.Regions.ID,
                        Name = m.Regions.Name,
                        Code = m.Regions.Code,
                        RowVersion = m.RowVersion
                    } : null,
                    ChallengeID = m.ChallengeID,
                    Challenges = m.Challenges != null ? new ChallengeDTO
                    {
                        ID = m.Challenges.ID,
                        Code = m.Challenges.Code,
                        Name = m.Challenges.Name,
                        RowVersion = m.RowVersion
                    } : null
                })
                .ToListAsync();


            if (memberDTOs.Count() > 0)
            {
                return memberDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No members found in the database" });
            }
        }


        #endregion 


        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
