﻿using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.MaterialValidation;
namespace IronBoots.Models.Material
{
    public class MaterialViewModel
    {
        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.01", "1000000000.00")] //TODO FIX MAGIC NUMBERS
        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string? PictureUrl { get; set; }

        [Required]
        [Url]
        public string DistributorContact { get; set; } = null!;

        [Required]
        public ICollection<ProductMaterial> MaterialProducts { get; set; } = new HashSet<ProductMaterial>();

        public List<Guid> SelectedProductIds { get; set; } = new();

        public List<Product> Products { get; set; } = new();
    }
}
