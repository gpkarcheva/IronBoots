using IronBoots.Common;
using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Shipments
{
    public class ShipmentViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid VehicleId { get; set; }


        [Required]
        public Vehicle Vehicle { get; set; } = null!;


        [Required]
        public IList<Order> AllOrders { get; set; } = new List<Order>();

        public IList<Guid> SelectedOrdersIds { get; set; } = new List<Guid>();


        public string? ShipmentDate { get; set; }


        public string? DeliveryDate { get; set; }


        [Required]
        public Status ShipmentStatus { get; set; }
    }
}
