using API.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public OrderType OrderType { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string CreatedByUserName { get; set; }
    }
}