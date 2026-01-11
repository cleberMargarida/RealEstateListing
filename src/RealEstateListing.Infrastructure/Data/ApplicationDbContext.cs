using Microsoft.EntityFrameworkCore;
using RealEstateListing.Domain.Entities;

namespace RealEstateListing.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext for the Real Estate Listing system.
/// Provides database access and entity configuration.
/// </summary>
/// <param name="options">The DbContext options.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    /// <summary>
    /// Gets or sets the Listings DbSet.
    /// </summary>
    public DbSet<Listing> Listings => Set<Listing>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);

            entity.ComplexProperty(e => e.Price, price =>
            {
                price.Property(m => m.Amount)
                     .HasColumnName("Price")
                     .HasPrecision(18, 2)
                     .IsRequired();

                price.Property(m => m.Currency)
                     .HasColumnName("Currency")
                     .IsRequired();
            });

            entity.ComplexProperty(e => e.Address, address =>
            {
                address.Property(a => a.Street)
                       .IsRequired(false)
                       .HasColumnName("Street")
                       .HasMaxLength(200);

                address.Property(a => a.City)
                       .IsRequired(false)
                       .HasColumnName("City")
                       .HasMaxLength(100);

                address.Property(a => a.State)
                       .HasColumnName("State")
                       .HasMaxLength(100);

                address.Property(a => a.ZipCode)
                       .HasColumnName("ZipCode")
                       .HasMaxLength(20);
            });
        });
    }
}
