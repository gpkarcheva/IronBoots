using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Vehicles
{
	public class VehicleViewModel
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		[MinLength(1)]
		[MaxLength(30)]
		public string Name { get; set; } = null!;

		[Required]
		public double WeightCapacity { get; set; }

		[Required]
		public double SizeCapacity { get; set; }

        public Guid? ShipmentId { get; set; }

        public Shipment? Shipment { get; set; }

		[Required]
		public bool IsAvailable { get; set; }

		[Required]
		public bool IsDeleted { get; set; }
	}
}
