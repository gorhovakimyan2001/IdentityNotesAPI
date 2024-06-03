using IdentityDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDb.Data
{
    public class ApplicationContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDoNoteModel> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ToDoNoteModel>()
            .HasKey(t => t.Id);

                builder.Entity<ToDoNoteModel>()
            .HasOne(n => n.User)
            .WithMany() 
            .HasForeignKey(n => n.UId)
            .IsRequired();
        }
    }
}
