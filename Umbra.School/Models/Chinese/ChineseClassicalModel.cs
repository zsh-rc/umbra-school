using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data;

namespace Umbra.School.Models.Chinese
{
    public class ChineseClassicalModel : BaseEntity
    {
        [Required]
        public string Sentence { get; set; } = string.Empty;
        [Required]
        public string Keyword { get; set; } = string.Empty;
        [Required]
        public string KeywordMeaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public int Rating { get; set; }
    }
}
