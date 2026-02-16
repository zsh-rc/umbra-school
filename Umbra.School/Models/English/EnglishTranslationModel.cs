using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data;

namespace Umbra.School.Models.English
{
    public class EnglishTranslationModel : BaseModel
    {
        [Required]
        public string Sentence { get; set; } = string.Empty;
        [Required]
        public string Translation { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
