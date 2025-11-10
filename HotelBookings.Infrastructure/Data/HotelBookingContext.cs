using HotelBookings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Infrastructure.Data
{
    public class HotelBookingContext(DbContextOptions<HotelBookingContext> options) : DbContext(options)
    {
        public virtual DbSet<Hotel> Hotels => Set<Hotel>();

        public virtual DbSet<Room> Rooms => Set<Room>();

        public virtual DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(h => h.Hotel)
                .HasForeignKey(r => r.HotelId)
                .HasPrincipalKey(h => h.Id);

            modelBuilder.Entity<Hotel>()
                 .HasIndex(h => h.Name);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Bookings)
                .WithOne(r => r.Room)
                .HasForeignKey(b => b.RoomId)
                .HasPrincipalKey(r => r.Id);

            modelBuilder.Entity<Booking>()
               .HasKey(b => b.Id);

            modelBuilder
               .HasSequence<int>("BookingSequence")
               .StartsAt(1)
               .IncrementsBy(1)
               .IsCyclic(false);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Id)
                //.UseHiLo(name: "BookingSequence"); -- option for better performance when scaling
                .HasDefaultValueSql("NEXT VALUE FOR BookingSequence");

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.RoomId, b.CheckIn, b.CheckOut })
                .IsUnique();
        }
    }
}
