﻿using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.ProductValidation;

namespace IronBoots.Models.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.01", "100000000.00")] //TODO FIX MAGIC NUMBERS
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [Range(WeightMin, WeightMax)]
        public double Weight { get; set; }

        [Required]
        [Range(SizeMin, SizeMax)]
        [Comment("Net size of the product in cm2")]
        public double Size { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "100000000.00")] //TODO FIX MAGIC NUMBERS
        [Precision(18, 2)]
        public decimal ProductionCost { get; set; }

        [Required]
        public TimeSpan ProductionTime { get; set; }

        [Required]
        public IList<ProductMaterial> ProductMaterials = new List<ProductMaterial>();


        [Required]
        public IList<OrderProduct> ProductOrders = new List<OrderProduct>();


        [Required]
        public bool IsDeleted { get; set; }

        [Required]
		public IList<Guid> SelectedMaterialsIds { get; set; } = new List<Guid>();

        [Required]
        public IList<Material> Materials { get; set; } = new List<Material>();
	}
}
