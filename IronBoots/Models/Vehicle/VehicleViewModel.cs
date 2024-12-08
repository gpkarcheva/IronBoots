using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Vehicle
{
	public class VehicleViewModel
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;

		[Required]
		public double WeightCapacity { get; set; }

		[Required]
		public double SizeCapacity { get; set; }

		public Shipment? Shipment { get; set; }

		[Required]
		public bool IsAvailable { get; set; }

		[Required]
		public bool IsDeleted { get; set; }
	}
}
