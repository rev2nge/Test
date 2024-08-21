using Microsoft.AspNetCore.Http;
using Test.Application.Dto;

namespace Test.Application.Repository.Interface
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<AnnouncementDto>> GetEntities(string sortBy, string sortOrder);
        Task<AnnouncementDto> GetEntity(Guid? id);
        Task PostEntity(AnnouncementDto entity);
        Task PutEntity(AnnouncementDto entity);
        Task DeleteEntity(Guid? id);
        Task<IEnumerable<AnnouncementDto>> SearchEntities(AnnouncementSearchDto searchDto);
        Task<int> GetUserAnnouncementsCount(Guid? userId);
        Task SaveImageAsync(Guid announcementId, IFormFile image);
        Task<byte[]> GetImageAsync(Guid imageId);
        Task<byte[]> GetThumbnailImageAsync(Guid imageId);
    }
}
