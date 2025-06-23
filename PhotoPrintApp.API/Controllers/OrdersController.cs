namespace PhotoPrintApp.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PhotoPrintApp.Api.Data;
    using PhotoPrintApp.Api.Enums;
    using PhotoPrintApp.Api.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        // Place order with list of photo IDs and customer name
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderCreateDto dto)
        {
            if (dto.PhotoIds == null || !dto.PhotoIds.Any())
                return BadRequest("At least one photo required");

            var photos = await _db.UploadedPhotos.Where(p => dto.PhotoIds.Contains(p.Id)).ToListAsync();
            if (photos.Count != dto.PhotoIds.Count)
                return BadRequest("One or more photos not found");

            var order = new Order
            {
                CustomerName = dto.CustomerName,
                Photos = photos,
                Status = OrderStatus.Pending
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return Ok(new { order.Id, order.Status });
        }

        // List all orders for operator
        [HttpGet("all")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _db.Orders.Include(o => o.Photos).ToListAsync();
            return Ok(orders);
        }

        // Update order status (operator)
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateStatusDto dto)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.Status = dto.Status;
            await _db.SaveChangesAsync();
            return Ok(order);
        }

        // Simulate payment completion
        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> CompletePayment(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.PaymentCompleted = true;
            await _db.SaveChangesAsync();
            return Ok(new { order.Id, order.PaymentCompleted });
        }
    }

    // DTOs

    public record OrderCreateDto(string CustomerName, List<int> PhotoIds);

    public record UpdateStatusDto(OrderStatus Status);

}
