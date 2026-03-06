using EventInvitationApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EventInvitationApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<RSVP> RSVPs { get; set; }
    }
}
