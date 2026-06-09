using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Data
{
    public class TicketingSystemDbContext : DbContext
    {
        public TicketingSystemDbContext(DbContextOptions<TicketingSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<TicketsComment> TicketsComments{ get; set; }
        public DbSet<TicketAttachment> TicketAttachments{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Product)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedEmployee)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TicketsComment>()
                .HasOne(tc => tc.Ticket)
                .WithMany(t => t.TicketsComments)
                .HasForeignKey(tc => tc.TicketId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TicketsComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TicketsComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TicketAttachment>()
                .HasOne(ta => ta.Ticket)
                .WithMany(t => t.TicketAttachments)
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasMaxLength(255);

            base.OnModelCreating(modelBuilder);
        }
    }
}