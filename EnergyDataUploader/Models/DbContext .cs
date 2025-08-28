using EnergyDataUploader.Models;
using Microsoft.EntityFrameworkCore;
public class EnergyDbContext : DbContext
{
    public EnergyDbContext(DbContextOptions<EnergyDbContext> options) : base(options) { }
    public DbSet<DimTramo> DimTramo { get; set; }
    public DbSet<DimFecha> DimFecha { get; set; }
    public DbSet<FactoConsumoCostosPerdidas> FactoConsumoCostosPerdidas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FactoConsumoCostosPerdidas>()
            .HasKey(f => new { f.FechaId, f.TramoId });

        modelBuilder.Entity<FactoConsumoCostosPerdidas>()
            .HasOne(f => f.Tramo)
            .WithMany(t => t.Factos)
            .HasForeignKey(f => f.TramoId);

        modelBuilder.Entity<FactoConsumoCostosPerdidas>()
            .HasOne(f => f.Fecha)
            .WithMany()
            .HasForeignKey(f => f.FechaId);
    }
}