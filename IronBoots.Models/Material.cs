﻿using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlAttribute = System.ComponentModel.DataAnnotations.UrlAttribute;

namespace IronBoots.Data.Models
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [Required]
        [Min(1)]
        public decimal Price { get; set; }
        [Required]
        [Url]
        public string DistrubutorContact { get; set; } = null!;
    }
}
