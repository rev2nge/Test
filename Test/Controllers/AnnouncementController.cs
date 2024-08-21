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
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncements(string sortBy = "Date", string sortOrder = "asc")
        {
            var announcements = await _announcementRepository.GetEntities(sortBy, sortOrder);
            return Ok(announcements);
        }

        [HttpGet("GetAnnouncement/{id:Guid}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(Guid id)
        {
            var announcement = await _announcementRepository.GetEntity(id);
            if (announcement == null)
            {
                return NotFound($"Объявление с ID {id} не найдено.");
            }
            return Ok(announcement);
        }

        [HttpPost("CreateAnnouncement")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Данные объявления не могут быть пустыми.");
            }

            await _announcementRepository.PostEntity(dto);
            return CreatedAtAction(nameof(GetAnnouncement), new { id = dto.Id }, dto);
        }

        [HttpPut("UpdateAnnouncement")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] AnnouncementDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Данные объявления не могут быть пустыми.");
            }

            await _announcementRepository.PutEntity(dto);
            return NoContent();
        }

        [HttpDelete("DeleteAnnouncement/{id:Guid}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var announcement = await _announcementRepository.GetEntity(id);
            if (announcement == null)
            {
                return NotFound($"Объявление с ID {id} не найдено.");
            }

            await _announcementRepository.DeleteEntity(id);
            return NoContent();
        }

        [HttpGet("SearchAnnouncements")]
        public async Task<ActionResult<IEnumerable<AnnouncementSearchDto>>> SearchAnnouncements([FromQuery] AnnouncementSearchDto searchDto)
        {
            if (searchDto == null)
            {
                return BadRequest("Параметры поиска не могут быть пустыми.");
            }

            var announcements = await _announcementRepository.SearchEntities(searchDto);
            return Ok(announcements);
        }

        [HttpPost("UploadImage/{announcementId:Guid}")]
        public async Task<IActionResult> UploadImage(Guid announcementId, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Не удалось загрузить изображение.");
            }

            await _announcementRepository.SaveImageAsync(announcementId, image);
            return Ok("Изображение успешно загружено.");
        }

        [HttpGet("GetImage/{imageId:Guid}")]
        public async Task<IActionResult> GetImage(Guid imageId)
        {
            var imageBytes = await _announcementRepository.GetImageAsync(imageId);
            if (imageBytes.Length == 0)
            {
                return NotFound("Изображение не найдено.");
            }

            return File(imageBytes, "image/png"); 
        }

        [HttpGet("GetThumbnail/{imageId:Guid}")]
        public async Task<IActionResult> GetThumbnail(Guid imageId)
        {
            var thumbnailBytes = await _announcementRepository.GetThumbnailImageAsync(imageId);
            if (thumbnailBytes.Length == 0)
            {
                return NotFound("Миниатюра не найдена.");
            }

            return File(thumbnailBytes, "image/png"); 
        }
    }
}