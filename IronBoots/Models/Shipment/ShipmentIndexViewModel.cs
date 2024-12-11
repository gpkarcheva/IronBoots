using IronBoots.Common;
using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Shipments
{
    public class ShipmentIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }


        public Vehicle? Vehicle { get; set; }

        [Required]
        public Status ShipmentStatus { get; set; }
    }
}
