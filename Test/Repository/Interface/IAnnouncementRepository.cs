using Test.Dto;

namespace Test.Repository.Interface
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<AnnouncementPutDto>> GetEntities(string sortBy, string sortOrder);
        Task<AnnouncementPutDto> GetEntity(Guid? id);
        Task PostEntity(AnnouncementPostDto entity);
        Task PutEntity(AnnouncementPutDto entity);
        Task DeleteEntity(Guid? id);
        Task<IEnumerable<AnnouncementPutDto>> SearchEntities(AnnouncementSearchDto searchDto);
        Task<int> GetUserAnnouncementsCount(Guid? userId);
    }
}
