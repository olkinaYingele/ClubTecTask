using ClubTecTask.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubTecTask.Data
{
    public class CitiesListContext : DbContext
    {
        public CitiesListContext(DbContextOptions<CitiesListContext> options) : base(options)
        {
        }

        public DbSet<CitiesList> Locations { get; set; }
    }
}
