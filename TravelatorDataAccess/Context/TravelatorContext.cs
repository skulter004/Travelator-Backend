using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;

namespace TravelatorDataAccess.Context
{
    public class TravelatorContext: IdentityDbContext<IdentityUser>
    {
        public TravelatorContext(DbContextOptions<TravelatorContext> options): base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TravelRequest> TravelRequests { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<TripBooking> TripBookings { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<CabRequest> CabRequests { get; set; }
        public DbSet<CabBooking> CabBookings { get; set; }
        public DbSet<Cab> Cabs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-many relationship between Employee and TravelRequest
            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.Employee)
                .WithMany(e => e.TravelRequests)
                .HasForeignKey(tr => tr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // One-to-many relationship between Employee and Approval (for approvers)
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.Approver)
                .WithMany(e => e.Approvals)
                .HasForeignKey(a => a.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // One-to-one relationship between TravelRequest and Booking
            modelBuilder.Entity<TripBooking>()
                .HasOne(b => b.TravelRequest)
                .WithOne(tr => tr.Booking)
                .HasForeignKey<TripBooking>(b => b.RequestId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // One-to-many relationship between Employee and Expense
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Employee)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // One-to-many relationship between TravelRequest and Approvals
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.TravelRequest)
                .WithMany(tr => tr.Approvals)
                .HasForeignKey(a => a.RequestId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                      .HasColumnType("char(36)"); // Explicitly map Guid to CHAR(36)

                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasMaxLength(255)
                      .IsRequired();


                entity.Property(e => e.Department)
                      .HasMaxLength(255)
                      .IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
