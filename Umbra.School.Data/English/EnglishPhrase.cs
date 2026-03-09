using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Umbra.School.Data.PersonalData;

namespace Umbra.School.Data.English
{
    public class EnglishPhrase : BaseEntity
    {
        [Required]
        public string Phrase { get; set; } = string.Empty;
        [Required]
        public string Meaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public int Sort { get; set; }

        public ICollection<UserEnglishPhraseRating> UserRatings { get; set; } = new List<UserEnglishPhraseRating>();
    }
}
