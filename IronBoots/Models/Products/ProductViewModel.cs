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
        public string Price { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        [Range(WeightMin, WeightMax)]
        public double Weight { get; set; }

        [Required]
        [Range(SizeMin, SizeMax)]
        [Comment("Net size of the product in cm2")]
        public double Size { get; set; }

        [Required]
        public string ProductionCost { get; set; } = null!;

        [Required]
        public string ProductionTime { get; set; } = null!;

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
