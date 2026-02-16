using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data.PersonalData;

namespace Umbra.School.Data.English
{
    public class EnglishWord : BaseEntity
    {
        [Required]
        public string Word { get; set; } = string.Empty;
        [Required]
        public string Meaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
        [Required]
        public string Book { get; set; } = "SHSV"; // Shanghai High School Vocabulary

        public ICollection<UserEnglishWordRating> UserRatings { get; set; } = new List<UserEnglishWordRating>();
    }
}
