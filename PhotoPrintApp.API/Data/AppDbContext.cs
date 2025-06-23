using Microsoft.EntityFrameworkCore;
using PhotoPrintApp.Api.Models;
using System.Collections.Generic;

namespace PhotoPrintApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UploadedPhoto> UploadedPhotos => Set<UploadedPhoto>();
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}