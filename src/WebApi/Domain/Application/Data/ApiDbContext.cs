using Api.Test.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Test.Domain.Application.Data;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "public");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(p => p.Name).HasColumnName("Name").IsUnicode(false);
            entity.Property(p => p.UserName).HasColumnName("UserName").IsUnicode(false);
            entity.Property(p => p.Email).HasColumnName("Email").IsUnicode(false);
            entity.Property(p => p.Address.Street).HasColumnName("Street").IsUnicode(false);
            entity.Property(p => p.Address.Suite).HasColumnName("Suite").IsUnicode(false);
            entity.Property(p => p.Address.City).HasColumnName("City").IsUnicode(false);
            entity.Property(p => p.Address.Zipcode).HasColumnName("Zipcode").IsUnicode(false);
            entity.Property(p => p.Address.Geo.Lat).HasColumnName("Lat").IsUnicode(false);
            entity.Property(p => p.Address.Geo.Lng).HasColumnName("Lng").IsUnicode(false);
            entity.Property(p => p.Phone).HasColumnName("Phone").IsUnicode(false);
            entity.Property(p => p.Website).HasColumnName("Website").IsUnicode(false);
            entity.Property(p => p.Phone).HasColumnName("Phone").IsUnicode(false);
            entity.Property(p => p.Company.Name).HasColumnName("CompanyName").IsUnicode(false);
            entity.Property(p => p.Company.CatchPhrase).HasColumnName("CatchPhrase").IsUnicode(false);
            entity.Property(p => p.Company.Bs).HasColumnName("Bs").IsUnicode(false);
        });
    }
}
