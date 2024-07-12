using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SeguimientoPagos.Models
{
    public partial class PagosUsuarioContext : DbContext
    {
        public PagosUsuarioContext()
        {
        }

        public PagosUsuarioContext(DbContextOptions<PagosUsuarioContext> options)
            : base(options)
        {
        }

        // representa la tabla Pagos en la base de datos
        public virtual DbSet<Pago> Pagos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configuración de la cadena de conexión
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost; database=PagosUsuario; integrated security=true; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad Pago
            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Pagos"); // Configuración de la clave primaria

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false); // Configuración de la propiedad Descripcion
            });
        }
    }
}
