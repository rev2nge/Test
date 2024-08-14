using Mapster;
using Microsoft.EntityFrameworkCore;
using Test.Context;
using Test.Dto;
using Test.Models;
using Test.Repository.Interface;

namespace Test.Repository
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationContext _context;

        public AnnouncementRepository(ApplicationContext context)
        {
            _context = context;
        }

        private DbSet<Announcement> DbSet => _context.Set<Announcement>();

        public async Task<IEnumerable<AnnouncementPutDto>> GetEntities(string sortBy, string sortOrder)
        {
            var query = DbSet.AsQueryable();
            var isDescending = string.Equals(sortOrder, "desc", StringComparison.CurrentCultureIgnoreCase);

            query = ApplySorting(query, sortBy, isDescending);

            var announcements = await query.ToListAsync();
            return announcements.Adapt<IEnumerable<AnnouncementPutDto>>();
        }

        private IQueryable<Announcement> ApplySorting(IQueryable<Announcement> query, string sortBy, bool isDescending)
        {
            return sortBy.ToLower() switch
            {
                "number" => isDescending ? query.OrderByDescending(a => a.Number) : query.OrderBy(a => a.Number),
                "userid" => isDescending ? query.OrderByDescending(a => a.UserId) : query.OrderBy(a => a.UserId),
                "text" => isDescending ? query.OrderByDescending(a => a.Text) : query.OrderBy(a => a.Text),
                "rate" => isDescending ? query.OrderByDescending(a => a.Rate) : query.OrderBy(a => a.Rate),
                "createdate" => isDescending ? query.OrderByDescending(a => a.CreateDate) : query.OrderBy(a => a.CreateDate),
                "expirydate" => isDescending ? query.OrderByDescending(a => a.ExpiryDate) : query.OrderBy(a => a.ExpiryDate),
                _ => query.OrderBy(a => a.CreateDate) 
            };
        }

        public async Task<AnnouncementPutDto> GetEntity(Guid? id)
        {
            var announcement = await DbSet.FindAsync(id);
            return announcement?.Adapt<AnnouncementPutDto>();
        }

        public async Task PostEntity(AnnouncementPostDto entity)
        {
            var announcement = entity.Adapt<Announcement>();
            await _context.AddAsync(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task PutEntity(AnnouncementPutDto entity)
        {
            var announcement = entity.Adapt<Announcement>();
            _context.Update(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Guid? id)
        {
            var announcement = await DbSet.FindAsync(id);
            if (announcement != null)
            {
                _context.Remove(announcement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AnnouncementPutDto>> SearchEntities(AnnouncementSearchDto searchDto)
        {
            var query = DbSet.AsQueryable();

            if (searchDto.Number.HasValue)
            {
                query = query.Where(a => a.Number == searchDto.Number.Value);
            }

            if (searchDto.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == searchDto.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Text))
            {
                query = query.Where(a => a.Text == searchDto.Text);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Rate))
            {
                query = query.Where(a => a.Rate.Contains(searchDto.Rate));
            }

            if (searchDto.CreateDate.HasValue)
            {
                query = query.Where(a => a.CreateDate == searchDto.CreateDate.Value.Date);
            }

            if (searchDto.ExpiryDate.HasValue)
            {
                query = query.Where(a => a.ExpiryDate == searchDto.ExpiryDate.Value.Date);
            }

            var announcements = await query.ToListAsync();
            return announcements.Adapt<IEnumerable<AnnouncementPutDto>>();
        }

        public async Task<int> GetUserAnnouncementsCount(Guid? userId)
        {
            return await DbSet.CountAsync(a => a.UserId == userId);
        }
    }
}
