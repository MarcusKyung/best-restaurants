using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BestRestaurants.Models
{
  public class Cuisine
  {
    public int CuisineId { get; set; }
    [Required(ErrorMessage = "The cuisine's type can't be empty!")]
    public string Type { get; set; }
    public List<CuisineRestaurant> JoinEntities {get;}
  }
}