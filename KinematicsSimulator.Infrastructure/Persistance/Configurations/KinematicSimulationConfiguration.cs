using KinematicsSimulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KinematicsSimulator.Infrastructure.Persistance.Configurations;

public class KinematicSimulationConfiguration : IEntityTypeConfiguration<KinematicSimulation>
{
    public void Configure(EntityTypeBuilder<KinematicSimulation> builder)
    {
        builder.ToTable("KinematicSimulations");

        builder.HasKey(ks => ks.Id);

        builder.Property(ks => ks.UserId)
        .IsRequired();

        builder.ComplexProperty(ks => ks.SimulationType, simTypeBuilder =>
        {
            simTypeBuilder.Property(stb => stb.Value)
                .HasColumnName("SimulationType")
                .IsRequired()
                .HasMaxLength(3);
        });

        builder.ComplexProperty(ks => ks.ResultValue, rValueBuilder =>
        {
            rValueBuilder.Property(rv => rv.Value)
                .HasColumnName("Result")
                .IsRequired();
        });

        builder.Property(ks => ks.CreatedAt)
            .IsRequired();

    }

}
