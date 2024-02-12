using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecipeBookContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager, RecipeBookContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("/")]
        public async Task<ActionResult> Index()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                Recipe[] recipes = _db.Recipes.Where(r => r.User.Id == currentUser.Id).ToArray();
            }
            return View();
        }
    }
}

// Item[] items = _db.Items
//                       .Where(entry => entry.User.Id == currentUser.Id)
//                       .ToArray();
//           model.Add("items", items);