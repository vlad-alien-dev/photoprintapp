using PhotoPrintApp.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhotoPrintApp.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; } = null!;
        public List<UploadedPhoto> Photos { get; set; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public bool PaymentCompleted { get; set; } = false;
    }
}