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


        public Vehicle? Vehicle { get; set; }


        [Required]
        public IList<Order> AllOrders { get; set; } = new List<Order>();

        public IList<Guid> SelectedOrdersIds { get; set; } = new List<Guid>();

        public IList<Vehicle> VehicleList { get; set; } = new List<Vehicle>();

        public string? ShipmentDate { get; set; }


        [Required]
        public Status ShipmentStatus { get; set; }
    }
}
