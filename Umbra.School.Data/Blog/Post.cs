using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Umbra.School.Data.Blog
{
    public class Post : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Excerpt { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public int ReadTimeMinutes { get; set; } = 5;
        public string? FileName { get; set; }
        [Required]
        public string Status { get; set; } =string.Empty;
        public string? Tags {  get; set; }
        [Required]
        public string Category {  get; set; } = string.Empty;
    }
}
