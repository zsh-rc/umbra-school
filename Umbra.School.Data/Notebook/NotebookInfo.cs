using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Umbra.School.Data.Notebook
{
    public class NotebookInfo:BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
