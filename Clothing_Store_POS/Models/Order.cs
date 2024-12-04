using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_Store_POS.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("discount_percentage")]
        public float DiscountPercentage { get; set; }

        [Column("tax_percentage")]
        public float TaxPercentage { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Customer")]
        [Column("customer_id")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
