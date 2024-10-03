using MyLWork7.Models.Domain;
using Microsoft.EntityFrameworkCore;
using MyLWork7.Models.Domain;
using System.Collections.Generic;
namespace labwork7.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions option) : base(option)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}