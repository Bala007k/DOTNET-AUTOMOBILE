using System.ComponentModel.DataAnnotations;

namespace TopSpeedAutomobile.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Established Year")]
        public int EstablishedYear { get; set; }
        [Display(Name ="Brand Logo")]
        public string Brandlogo {  get; set; } 

    }
}
