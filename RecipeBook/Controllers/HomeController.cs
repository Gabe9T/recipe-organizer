using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using RecipeBook.Models;
using System.Linq;

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
        [Authorize]
        public async Task<ActionResult> Index()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                Recipe[] recipes = _db.Recipes.Where(r => r.User.Id == currentUser.Id).ToArray();

                return View(recipes);
            }
            return View();
        }
    }
}
