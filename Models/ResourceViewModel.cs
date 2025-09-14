using System.ComponentModel.DataAnnotations;

namespace Zippy.Models
{
    public class ResourceViewModel
    {
        [Required]
        public string Url { get; set; }
        public string? Alias { get; set; }
    }
}
