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
        public string? PlannedAssignedDate { get; set; }


        public string? ActualAssignedDate { get; set; }
    }
}
