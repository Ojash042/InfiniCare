using Infinicare_Ojash_Devkota.Models;
using Microsoft.EntityFrameworkCore;

namespace Infinicare_Ojash_Devkota.Data;

public class ApplicationDBContext : DbContext {
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }
    public DbSet<TransactionDetails> TransactionDetails { get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserCredentials>()
            .HasOne(userCredentials => userCredentials.UserDetails)
            .WithOne(userDetails => userDetails.UserCredentials)
            .HasForeignKey<UserCredentials>(userCredentials => userCredentials.UserId);
    }
}