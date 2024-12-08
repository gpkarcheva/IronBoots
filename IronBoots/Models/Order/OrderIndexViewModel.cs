using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Orders
{
    public class OrderIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Client Client { get; set; } = null!;

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime PlannedAssignedDate { get; set; }


        public DateTime? ActualAssignedDate { get; set; }
    }
}
