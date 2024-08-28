using Microsoft.AspNetCore.Mvc;
using Test.Application.Repository.Interface;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementImageController : ControllerBase
    {
        private readonly IAnnouncementImageRepository _imageRepository;
        private readonly IHostingEnvironment _webHostEnvironment;

        public AnnouncementImageController(IAnnouncementImageRepository imageRepository, IHostingEnvironment webHostEnvironment)
        {
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("UploadImage/{announcementId:Guid}")]
        public async Task<IActionResult> UploadImage(Guid announcementId, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Не удалось загрузить изображение.");
            }

            var savedImagePath = await _imageRepository.SaveImageAsync(announcementId, image, _webHostEnvironment.WebRootPath);

            return Ok(new { Message = "Изображение успешно загружено.", ImagePath = savedImagePath });
        }

        [HttpGet("GetImages/{announcementId:Guid}")]
        public async Task<IActionResult> GetImages(Guid announcementId, bool isThumbnail = false)
        {
            var imageResults = await _imageRepository.GetImagesAsync(announcementId, isThumbnail);

            if (imageResults == null || !imageResults.Any())
            {
                return NotFound("Изображения не найдены.");
            }

            return Ok(imageResults);
        }

        [HttpGet("GetImage/{imageId:Guid}")]
        public async Task<IActionResult> GetImage(Guid imageId, bool isThumbnail = false)
        {
            var imageResult = await _imageRepository.GetImageAsync(imageId, _webHostEnvironment.WebRootPath, isThumbnail);

            if (imageResult == null)
            {
                return NotFound(isThumbnail ? "Миниатюра не найдена." : "Изображение не найдено.");
            }

            return imageResult;
        }
    }
}
