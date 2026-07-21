using KinematicsSimulator.Application.Interfaces;
using KinematicsSimulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinematicsSimulator.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users {get; set;}
    public DbSet<KinematicSimulation> Simulations {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}