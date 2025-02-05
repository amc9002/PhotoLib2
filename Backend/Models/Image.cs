using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
        public string ExifData { get; set; }
    }
}