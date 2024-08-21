using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Drawing;
using Test.Application.Configuration;
using Test.Application.Dto;
using Test.Application.Repository.Interface;
using Test.Domain.Models;
using Test.Infrastructure.Context;

namespace Test.Infrastructure.Repository
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly AnnouncementSettings _announcementSettings;

        public AnnouncementRepository(ApplicationContext context, IConfiguration configuration, IOptions<AnnouncementSettings> announcementSettings)
        {
            _context = context;
            _configuration = configuration;
            _announcementSettings = announcementSettings.Value;
        }

        private DbSet<Announcement> DbSet => _context.Set<Announcement>();

        public async Task<IEnumerable<AnnouncementDto>> GetEntities(string sortBy, string sortOrder)
        {
            var query = DbSet.AsQueryable();
            var isDescending = string.Equals(sortOrder, "desc", StringComparison.CurrentCultureIgnoreCase);

            query = ApplySorting(query, "Date", isDescending);

            var announcements = await query.ToListAsync();
            return announcements.Adapt<IEnumerable<AnnouncementDto>>();
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

        public async Task<AnnouncementDto> GetEntity(Guid? id)
        {
            var announcement = await DbSet.FindAsync(id);
            return announcement.Adapt<AnnouncementDto>();
        }

        public async Task PostEntity(AnnouncementDto entity)
        {
            var maxAnnouncements = _announcementSettings.MaxAnnouncementsPerUser;
            var userAnnouncementsCount = await GetUserAnnouncementsCount(entity.UserId);

            if (userAnnouncementsCount >= maxAnnouncements)
            {
                throw new InvalidOperationException($"Пользователь уже достиг лимита в {maxAnnouncements} объявлений.");
            }

            var announcement = entity.Adapt<Announcement>();
            await _context.AddAsync(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task PutEntity(AnnouncementDto entity)
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

        public async Task<IEnumerable<AnnouncementDto>> SearchEntities(AnnouncementSearchDto searchDto)
        {
            var query = DbSet.AsQueryable();

            if (searchDto.Number != 0)
            {
                query = query.Where(a => a.Number == searchDto.Number);
            }

            if (searchDto.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == searchDto.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Text))
            {
                query = query.Where(a => a.Text.Contains(searchDto.Text));
            }

            if (searchDto.Rate.HasValue)
            {
                query = query.Where(a => a.Rate == searchDto.Rate.Value);
            }

            if (searchDto.StartCreateDate.HasValue)
            {
                query = query.Where(a => a.CreateDate >= searchDto.StartCreateDate.Value);
            }

            if (searchDto.EndCreateDate.HasValue)
            {
                query = query.Where(a => a.CreateDate <= searchDto.EndCreateDate.Value);
            }

            if (searchDto.StartExpiryDate.HasValue)
            {
                query = query.Where(a => a.ExpiryDate >= searchDto.StartExpiryDate.Value);
            }

            if (searchDto.EndExpiryDate.HasValue)
            {
                query = query.Where(a => a.ExpiryDate <= searchDto.EndExpiryDate.Value);
            }

            var announcements = await query.ToListAsync();
            return announcements.Adapt<IEnumerable<AnnouncementDto>>();
        }

        public async Task<int> GetUserAnnouncementsCount(Guid? userId)
        {
            return await DbSet.CountAsync(a => a.UserId == userId);
        }

        public async Task SaveImageAsync(Guid announcementId, IFormFile image)
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var originalImage = memoryStream.ToArray();

            var thumbnailImage = ResizeImage(originalImage, 100, 100); // Изменение размера миниатюры

            var announcementImage = new AnnouncementImage
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcementId,
                OriginalImage = originalImage,
                ThumbnailImage = thumbnailImage,
                ImageFormat = image.ContentType
            };

            await _context.AnnouncementImages.AddAsync(announcementImage);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> GetImageAsync(Guid imageId)
        {
            var image = await _context.AnnouncementImages.FindAsync(imageId);
            return image?.OriginalImage ?? Array.Empty<byte>();
        }

        public async Task<byte[]> GetThumbnailImageAsync(Guid imageId)
        {
            var image = await _context.AnnouncementImages.FindAsync(imageId);
            return image?.ThumbnailImage ?? Array.Empty<byte>();
        }

        private byte[] ResizeImage(byte[] image, int width, int height)
        {
            using var inputStream = new MemoryStream(image);
            var original = new Bitmap(inputStream);
            var thumbnail = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.DrawImage(original, 0, 0, width, height);
            }

            using var outputStream = new MemoryStream();
            thumbnail.Save(outputStream, original.RawFormat);
            return outputStream.ToArray();
        }
    }
}
