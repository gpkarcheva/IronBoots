using static IronBoots.Common.EntityValidationConstants.MaterialValidation;
using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace IronBoots.Models.Materials
{
    public class MaterialIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;


        [Required]
        public string Price { get; set; } = null!;


        public string? PictureUrl { get; set; }


        [Required]
        public bool IsDeleted { get; set; }
    }
}
