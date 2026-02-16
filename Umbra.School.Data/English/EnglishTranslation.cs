using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Umbra.School.Data.English
{
    public class EnglishTranslation : BaseEntity
    {
        [Required]
        public string Sentence { get; set; } = string.Empty;
        [Required]
        public string Translation { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
