using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Umbra.School.Data.English
{
    public class EnglishPhrase : BaseEntity
    {
        [Required]
        public string Phrase { get; set; } = string.Empty;
        [Required]
        public string Meaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
