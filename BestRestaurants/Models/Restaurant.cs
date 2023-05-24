using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BestRestaurants.Models
{
  public class Restaurant
  {
    public int RestaurantId { get; set; }
    [Required(ErrorMessage = "The restaurant's name can't be empty!")]

    public string Name { get; set; }  
    public List<CuisineRestaurant> JoinEntities {get;}
  }
}