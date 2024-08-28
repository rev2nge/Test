using Microsoft.AspNetCore.Mvc;
using Test.Application.Dto;
using Test.Application.Repository.Interface;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementController(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        [HttpGet("GetAnnouncements")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncements(string sortBy = "Date", string sortOrder = "asc") => Ok(await _announcementRepository.GetEntities(sortBy, sortOrder));

        [HttpGet("GetAnnouncement/{id:Guid}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(Guid id) => Ok(await _announcementRepository.GetEntity(id));

        [HttpPost("CreateAnnouncement")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementPostDto dto)
        {
            await _announcementRepository.PostEntity(dto);
            return CreatedAtAction(nameof(GetAnnouncement), new { id = dto.Id }, dto);
        }

        [HttpPut("UpdateAnnouncement")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] AnnouncementPostDto dto)
        {
            await _announcementRepository.PutEntity(dto);
            return NoContent();
        }

        [HttpDelete("DeleteAnnouncement/{id:Guid}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var announcement = await _announcementRepository.GetEntity(id);

            await _announcementRepository.DeleteEntity(id);
            return NoContent();
        }

        [HttpGet("SearchAnnouncements")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> SearchAnnouncements([FromQuery] AnnouncementSearchDto searchDto) => Ok(await _announcementRepository.SearchEntities(searchDto));
    }
}