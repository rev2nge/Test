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

        [HttpGet("GetImages/{announcementId:Guid}")]
        public async Task<IActionResult> GetImages(Guid announcementId, bool isThumbnail = false) => Ok(await _imageRepository.GetImagesAsync(announcementId, isThumbnail));

        [HttpGet("GetImage/{imageId:Guid}")]
        public async Task<IActionResult> GetImage(Guid imageId, bool isThumbnail = false) => await _imageRepository.GetImageAsync(imageId, _webHostEnvironment.WebRootPath, isThumbnail);

        [HttpPost("UploadImage/{announcementId:Guid}")]
        public async Task<IActionResult> UploadImage(Guid announcementId, IFormFile image) => Ok(await _imageRepository.SaveImageAsync(announcementId, image, _webHostEnvironment.WebRootPath));
    }
}