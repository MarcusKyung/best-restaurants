using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace BestRestaurants.Controllers
{
  public class CuisinesController : Controller
  {
    private readonly BestRestaurantsContext _db;

    public CuisinesController(BestRestaurantsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Cuisine> model = _db.Cuisines.OrderBy(cuisine => cuisine.Type).ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Cuisine cuisine)
    {
      if (!ModelState.IsValid)
      {
        return View(cuisine);
      }
      else
      {
      _db.Cuisines.Add(cuisine);
      _db.SaveChanges();
      return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Cuisine thisCuisine = _db.Cuisines
                                .Include(cuisine => cuisine.JoinEntities)
                                .ThenInclude(join => join.Restaurant)
                                .FirstOrDefault(cuisine => cuisine.CuisineId == id);
      return View(thisCuisine);
    }

    public ActionResult Edit(int id)
    {
      Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
      return View(thisCuisine);
    }

    [HttpPost]
    public ActionResult Edit(Cuisine cuisine)
    {
      _db.Cuisines.Update(cuisine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
      return View(thisCuisine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
      _db.Cuisines.Remove(thisCuisine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Search()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Results(string type)
    {
      Cuisine thisCuisine = _db.Cuisines
                              .Include(cuisine => cuisine.JoinEntities)
                              .ThenInclude(join => join.Restaurant)
                              .FirstOrDefault(cuisine => cuisine.Type == type);

      if (thisCuisine != null)
      {
          return View(thisCuisine);
      }
      else
      {
          return RedirectToAction("NoResults");
      }
    }

    public ActionResult NoResults()
    {
      return View();
    }

    public ActionResult AddRestaurant(int id)
    {
      Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisines => cuisines.CuisineId == id);
      ViewBag.RestaurantId = new SelectList(_db.Restaurants, "RestaurantId", "Name");
      return View(thisCuisine);
    }

    [HttpPost]
    public ActionResult AddRestaurant(Cuisine cuisine, int restaurantId)
    {
      #nullable enable
      CuisineRestaurant? joinEntity = _db.CuisineRestaurants.FirstOrDefault(join => (join.RestaurantId == restaurantId && join.CuisineId == cuisine.CuisineId));
      #nullable disable
      if (joinEntity == null && restaurantId != 0)
      {
        _db.CuisineRestaurants.Add(new CuisineRestaurant() {RestaurantId = restaurantId, CuisineId = cuisine.CuisineId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = cuisine.CuisineId});
    }

    [HttpPost]
    public ActionResult DeleteJoin(int cuisineId, int joinId)
    {
      CuisineRestaurant joinEntry = _db.CuisineRestaurants.FirstOrDefault(entry => entry.CuisineRestaurantId == joinId);
      _db.CuisineRestaurants.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = cuisineId});
    }
  }
}