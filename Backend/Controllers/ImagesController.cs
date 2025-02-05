using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using ExifLib;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ImagesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetPhotos()
        {
            return await _context.Photos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Image>> PostPhoto([FromForm] IFormFile file, [FromForm] string title)
        {
            var filePath = Path.Combine("Uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var exifData = ExtractExif(filePath);
            var newPhoto = new Image
            {
                Title = title,
                Url = filePath,
                ExifData = exifData
            };
            _context.Photos.Add(newPhoto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPhotos), new { id = newPhoto.Id }, newPhoto);
        }

        private string ExtractExif(string filePath)
        {
            using (var reader = new ExifReader(filePath))
            {
                reader.GetTagValue(ExifTags.DateTime, out string dateTime);
                reader.GetTagValue(ExifTags.GPSLatitude, out double[] latitude);
                reader.GetTagValue(ExifTags.GPSLongitude, out double[] longitude);
                return $"Date: {dateTime}, Location: {latitude?[0]} {longitude?[0]}";
            }
        }
    }
}