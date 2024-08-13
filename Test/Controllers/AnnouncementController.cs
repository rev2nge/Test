using Microsoft.AspNetCore.Mvc;
using Test.Dto;
using Test.Repository.Interface;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IConfiguration _configuration;

        public AnnouncementController(IAnnouncementRepository announcementRepository, IConfiguration configuration)
        {
            _announcementRepository = announcementRepository;
            _configuration = configuration;

        }

        [HttpGet("GetAnnouncements")]
        public async Task<IActionResult> GetAnnouncements(string sortBy = "Date", string sortOrder = "asc")
        {
            var announcements = await _announcementRepository.GetEntities(sortBy, sortOrder);
            return Ok(announcements);
        }

        [HttpGet("GetAnnouncement/{id:Guid}")]
        public async Task<IActionResult> GetAnnouncement(Guid? id) => Ok(await _announcementRepository.GetEntity(id));

        [HttpPost("CreateAnnouncement")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementPostDto dto)
        {
            try
            {
                var maxAnnouncements = int.Parse(_configuration["MaxAnnouncementsPerUser"]); 
                var userAnnouncementsCount = await _announcementRepository.GetUserAnnouncementsCount(dto.UserId);

                if (userAnnouncementsCount >= maxAnnouncements)
                {
                    return BadRequest($"Пользователь уже достиг лимита в {maxAnnouncements} объявлений.");
                }

                await _announcementRepository.PostEntity(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при создании объявления: " + ex);
            }
        }

        [HttpPut("UpdateAnnouncement")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] AnnouncementPutDto dto)
        {
            try
            {
                await _announcementRepository.PutEntity(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при обновлении объявления: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAnnouncement/{id:Guid}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid? id)
        {
            try
            {
                await _announcementRepository.DeleteEntity(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при удалении объявления: {ex.Message}");
            }
        }

        [HttpGet("SearchAnnouncements")]
        public async Task<IActionResult> SearchAnnouncements([FromQuery] AnnouncementSearchDto searchDto)
        {
            var announcements = await _announcementRepository.SearchEntities(searchDto);
            return Ok(announcements);
        }
    }
}
