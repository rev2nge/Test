using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Application.Repository.Interface
{
    public interface IAnnouncementImageRepository
    {
        Task<IEnumerable<Guid>> GetImagesAsync(Guid announcementId, bool isThumbnail = false);
        Task<FileContentResult> GetImageAsync(Guid imageId, string webRootPath, bool isThumbnail = false);
        Task<string> SaveImageAsync(Guid announcementId, IFormFile image, string webRootPath);
    }
}