using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPrinciples.Data;

namespace MVCPrinciples.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryContext _context;

        public CategoriesController(CategoryContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'CategoryContext.Categories'  is null.");
        }

        // GET: Categories/Image/5
        public async Task<IActionResult> Image(int? id)
        {
            ViewBag.Id = id;
            return _context.Categories != null ?
                View(await _context.Categories.ToListAsync()) :
                Problem("Entity set 'CategoryContext.Categories'  is null.");
        }
    }
}
