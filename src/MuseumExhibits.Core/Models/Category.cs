using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();  
        [Required]
        public string Name { get; set; }
        //public string Description { get; set; }
        public List<Exhibit> Exhibits { get; set; } = new List<Exhibit>(); 
    }
}
