using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Umbra.School.Data.English;

namespace Umbra.School.Data.PersonalData
{
    public class UserEnglishWordRating : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        public Guid WordId {  get; set; }
        [ForeignKey("WordId")]
        public EnglishWord? EnglishWord { get; set; }
        public int Rating { get; set; }
        public DateTime LastReviewed { get; set; } = DateTime.UtcNow;

    }
}
