using Test.Application.Dto;

namespace Test.Application.Repository.Interface
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<AnnouncementListDto>> GetEntities(string sortBy, string sortOrder);
        Task<AnnouncementDto> GetEntity(Guid? id);
        Task PostEntity(AnnouncementPostDto entity);
        Task PutEntity(AnnouncementPostDto entity);
        Task DeleteEntity(Guid? id);
        Task<IEnumerable<AnnouncementDto>> SearchEntities(AnnouncementSearchDto searchDto);
    }
}
