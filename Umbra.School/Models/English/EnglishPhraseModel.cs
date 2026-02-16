using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data;

namespace Umbra.School.Models.English
{
    public class EnglishPhraseModel : BaseModel
    {
        [Required]
        public string Phrase { get; set; } = string.Empty;
        [Required]
        public string Meaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
