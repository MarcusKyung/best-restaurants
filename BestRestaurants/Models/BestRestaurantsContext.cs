using Microsoft.EntityFrameworkCore;

namespace BestRestaurants.Models
{
  public class BestRestaurantsContext : DbContext
  {
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<CuisineRestaurant> CuisineRestaurants { get; set; }
    public BestRestaurantsContext(DbContextOptions options) : base(options) { }
  }
}