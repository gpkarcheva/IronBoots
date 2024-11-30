﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Shipment
    {
        public Shipment()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [Comment("Id of the vehicle that will handle the shipment")]
        public Guid VehicleId { get; set; }


        [Required]
        [ForeignKey(nameof(VehicleId))]
        [Comment("Vehicle object")]
        public Vehicle Vehicle { get; set; } = null!;


        [Required]
        [Comment("The orders part of the shipment")]
        public IList<Order> Orders { get; set; } = new List<Order>();


        [Required]
        [Comment("The date the shipment started")]
        public DateTime ShipmentDate { get; set; }


        [Required]
        [Comment("The date the shipment was completed")]
        public DateTime DeliveryDate { get; set; }


        public enum Status
        {
            PendingShipment,
            Shipped,
            InTransit,
            Delivered
        }
    }
}
