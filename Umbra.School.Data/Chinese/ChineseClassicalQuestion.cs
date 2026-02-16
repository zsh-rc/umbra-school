using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Umbra.School.Data.Chinese
{
    public class ChineseClassicalQuestion:BaseEntity
    {
        [Required]
        public string Sentence { get; set; } = string.Empty;
        [Required]
        public string Keyword { get; set; } = string.Empty;
        [Required]
        public string Options { get; set; } = string.Empty;
        [Required]
        public string Answer { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
