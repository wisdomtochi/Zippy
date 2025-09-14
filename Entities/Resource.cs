using System.ComponentModel.DataAnnotations.Schema;

namespace Zippy.Entities
{
    [Table(name: "resources")]
    public class Resource
    {  
        public Guid Id { get; set; }      
        public string Url { get; set; }       
        public string Key { get; set; }
        public string? Alias { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
