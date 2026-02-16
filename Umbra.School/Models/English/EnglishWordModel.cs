using System.ComponentModel.DataAnnotations;

namespace Umbra.School.Models.English
{
    public class EnglishWordModel:BaseModel
    {
        [Required]
        public string Word { get; set; } = string.Empty;
        [Required]
        public string Meaning { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
