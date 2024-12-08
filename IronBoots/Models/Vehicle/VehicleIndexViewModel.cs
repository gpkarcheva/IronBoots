using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Vehicles
{
    public class VehicleIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public bool IsAvailable { get; set; }
    }
}
