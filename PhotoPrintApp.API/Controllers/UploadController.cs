using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using PhotoPrintApp.Api.Data;
using PhotoPrintApp.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public UploadController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000_000)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var connectionString = _config.GetConnectionString("BlobStorage");
        var container = new BlobContainerClient(connectionString, "uploads");
        await container.CreateIfNotExistsAsync(PublicAccessType.None);

        var blob = container.GetBlobClient($"{Guid.NewGuid()}_{file.FileName}");

        using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, overwrite: true);

        var photo = new UploadedPhoto
        {
            FileName = file.FileName,
            FilePath = blob.Uri.ToString()
        };

        _db.UploadedPhotos.Add(photo);
        await _db.SaveChangesAsync();

        return Ok(new { photo.Id, photo.FileName });
    }
}
