using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace BestRestaurants.Controllers
{
  public class RestaurantsController : Controller
  {
    private readonly BestRestaurantsContext _db;

    public RestaurantsController(BestRestaurantsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Restaurant> model = _db.Restaurants.OrderBy(restaurant => restaurant.Name).ToList();
      ViewBag.PageTitle = "View All Restaurants";                     
      return View(model);
    }

    public ActionResult Create()
    {  
      return View();
    }


    [HttpPost]
    public ActionResult Create(Restaurant restaurant)
    {
      if (!ModelState.IsValid)
      {
        return View(restaurant);
      }
      else
      {
      _db.Restaurants.Add(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants
                                    .Include(restaurant => restaurant.JoinEntities)
                                    .ThenInclude(join => join.Cuisine)
                                    .FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    public ActionResult Edit(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    [HttpPost]
    public ActionResult Edit(Restaurant restaurant)
    {
      _db.Restaurants.Update(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      _db.Restaurants.Remove(thisRestaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCuisine(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurants => restaurants.RestaurantId == id);
      ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
      return View(thisRestaurant);
    }

    [HttpPost]
    public ActionResult AddCuisine(Restaurant restaurant, int cuisineId)
    {
      #nullable enable
      CuisineRestaurant? joinEntity = _db.CuisineRestaurants.FirstOrDefault(join => (join.CuisineId == cuisineId && join.RestaurantId == restaurant.RestaurantId));
      #nullable disable
      if (joinEntity == null && cuisineId != 0)
      {
        _db.CuisineRestaurants.Add(new CuisineRestaurant () { CuisineId = cuisineId, RestaurantId = restaurant.RestaurantId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = restaurant.RestaurantId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int restaurantId, int joinId)
    {
      CuisineRestaurant joinEntry = _db.CuisineRestaurants.FirstOrDefault(entry => entry.CuisineRestaurantId == joinId);
      _db.CuisineRestaurants.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = restaurantId});
    }
  }
}