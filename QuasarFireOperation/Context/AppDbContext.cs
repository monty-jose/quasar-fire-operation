using Microsoft.EntityFrameworkCore;
using QuasarFireOperation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base (options)
        {

        }

        public DbSet<Satellites> Satellites { get; set; }
        public DbSet<MessagesSecret> Messages { get; set; }

    }
}
