using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace coleta_residuos.Data.Contexts
{
    public class DatabaseContext : DbContext
    {

        public virtual DbSet<AlertaModel> Alertas { get; set; }
        public virtual DbSet<ColetaAgendadaModel> ColetasAgendadas { get; set; }
        public virtual DbSet<EventoColetaModel> EventosColeta { get; set; }
        public virtual DbSet<PontoColetaModel> PontosColeta { get; set; }
        public virtual DbSet<ResiduoModel> Residuos { get; set; }
        public virtual DbSet<PontoColetaResiduoModel> PontoColetaResiduos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResiduoModel>(entity =>
            {
                entity.ToTable("Residuos");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(255)
                    .IsRequired();
            });

            modelBuilder.Entity<PontoColetaModel>(entity =>
            {
                entity.ToTable("PontosColeta");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Endereco)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.CapacidadeMaximaKg)
                    .IsRequired();
            });

            modelBuilder.Entity<PontoColetaResiduoModel>(entity =>
            {
                entity.ToTable("PontoColetaResiduo");

                entity.HasKey(e => new { e.PontoColetaId, e.ResiduoId });

                entity.HasOne(e => e.PontoColeta)
                    .WithMany(pc => pc.PontosColetaResiduos)
                    .HasForeignKey(e => e.PontoColetaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Residuo)
                    .WithMany(r => r.PontosColetaResiduos)
                    .HasForeignKey(e => e.ResiduoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AlertaModel>(entity =>
            {
                entity.ToTable("Alertas");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Mensagem)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.DataAlerta)
                    .IsRequired();

                entity.Property(e => e.Resolvido)
                    .HasColumnType("NUMBER(1)");

                entity.HasOne(e => e.PontoColeta)
                    .WithMany()
                    .HasForeignKey(e => e.PontoColetaId);
            });

            modelBuilder.Entity<ColetaAgendadaModel>(entity =>
            {
                entity.ToTable("ColetasAgendadas");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.DataAgendada).IsRequired();
                entity.Property(e => e.Observacao).HasMaxLength(255);

                entity.HasOne(e => e.PontoColeta)
                    .WithMany()
                    .HasForeignKey(e => e.PontoColetaId);
            });

            modelBuilder.Entity<EventoColetaModel>(entity =>
            {
                entity.ToTable("EventosColeta");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.DataEvento).IsRequired();
                entity.Property(e => e.QuantidadeKg).IsRequired();
                entity.Property(e => e.TipoEvento).IsRequired();

                entity.HasOne(e => e.PontoColeta)
                    .WithMany(pc => pc.EventosColeta)
                    .HasForeignKey(e => e.PontoColetaId);
            });
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected DatabaseContext()
        {
        }
    }
}
