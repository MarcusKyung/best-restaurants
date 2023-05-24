using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Linq;
using System.Collections.Generic;

namespace BestRestaurants.Controllers
{
    public class HomeController : Controller
    {
      private readonly BestRestaurantsContext _db;

      public HomeController(BestRestaurantsContext db)
      {
        _db = db;
      }

      [HttpGet("/")]
      public ActionResult Index()
      {
        Restaurant[] restaurants = _db.Restaurants.ToArray();
        Cuisine[] cuisines = _db.Cuisines.ToArray();
        Dictionary<string,object[]> model = new Dictionary<string, object[]>();
        model.Add("restaurants", restaurants);
        model.Add("cuisines", cuisines);
        return View(model);
      }

    }
}