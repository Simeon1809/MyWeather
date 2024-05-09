using Microsoft.EntityFrameworkCore;
using MyWeather.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyWeather.Infrastructure
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<WeatherHistory> WeatherHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
        }
    }
}
