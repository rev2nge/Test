using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Test.Application.Repository.Interface;
using Test.Domain.Models;
using Test.Infrastructure.Context;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Test.Infrastructure.Repository
{
    public class AnnouncementImageRepository : IAnnouncementImageRepository
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _webHostEnvironment;

        public AnnouncementImageRepository(ApplicationContext context, IHostingEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Guid>> GetImagesAsync(Guid announcementId, bool isThumbnail = false)
        {
            var images = await _context.AnnouncementImages
                .Where(img => img.AnnouncementId == announcementId)
                .Select(img => img.Id)
                .ToListAsync();

            if (images == null || !images.Any())
            {
                return Enumerable.Empty<Guid>();
            }

            return images;
        }

        public async Task<FileContentResult> GetImageAsync(Guid imageId, string webRootPath, bool isThumbnail = false)
        {
            var image = await _context.AnnouncementImages.FindAsync(imageId);
            if (image == null)
            {
                return null;
            }

            var imagePath = Path.Combine(webRootPath, isThumbnail ? image.ThumbnailPath : image.ImagePath);
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            var contentType = image.ImageFormat ?? "image/jpeg";

            return new FileContentResult(imageBytes, contentType);
        }

        public async Task<string> SaveImageAsync(Guid announcementId, IFormFile image, string webRootPath)
        {
            var imagePath = Path.Combine(webRootPath, "images");
            Directory.CreateDirectory(imagePath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(imagePath, fileName);

            await using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var originalImageBytes = memoryStream.ToArray();

            var resizedImageBytes = ResizeImage(originalImageBytes, 100, 100);
            var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_thumb{Path.GetExtension(fileName)}";
            var thumbnailPath = Path.Combine(imagePath, thumbnailFileName);

            await File.WriteAllBytesAsync(thumbnailPath, resizedImageBytes);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await fileStream.WriteAsync(originalImageBytes, 0, originalImageBytes.Length);

            var announcementImage = new AnnouncementImage
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcementId,
                ImagePath = Path.Combine("images", fileName),
                ThumbnailPath = Path.Combine("images", thumbnailFileName),
                ImageFormat = image.ContentType
            };

            await _context.AnnouncementImages.AddAsync(announcementImage);
            await _context.SaveChangesAsync();

            return announcementImage.ImagePath;
        }

        private byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            using var inputStream = new MemoryStream(imageBytes);
            using var originalImage = new Bitmap(inputStream);
            using var resizedImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(originalImage, 0, 0, width, height);
            }

            using var outputStream = new MemoryStream();
            resizedImage.Save(outputStream, originalImage.RawFormat);
            return outputStream.ToArray();
        }
    }
}