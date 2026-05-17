using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain;

namespace CoffeeShop.Api
{
    // Наследуемся от DbContext — это встроенный класс EF Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Это свойство представляет собой таблицу в базе данных. 
        // EF Core сам создаст таблицу "Shifts" на основе нашего класса Shift.
        public DbSet<Shift> Shifts { get; set; }
    }
}