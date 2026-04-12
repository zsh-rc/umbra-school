using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data.PersonalData;

namespace Umbra.School.Data.Chinese
{
    public class ChineseClassical : BaseEntity
    {
        [Required]
        public string Sentence { get; set; } = string.Empty;
        [Required]
        public string Keyword { get; set; } = string.Empty;
        [Required]
        public string KeywordMeaning { get; set; } = string.Empty;
        public string? Remark { get; set; }

        public ICollection<UserChineseClassicalRating> UserRatings { get; set; } = new List<UserChineseClassicalRating>();
    }
}
