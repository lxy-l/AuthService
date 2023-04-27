using IdentityServer7.EntityFramework.Storage.DbContexts;
using IdentityServer7.EntityFramework.Storage.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{

    /// <summary>
    /// Api资源管理
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    public class ApiResourceController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public ApiResourceController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _context.ApiResources.ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == default)
            {
                return NotFound();
            }

            var model = await _context.ApiResources.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, ApiResource model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var apiresource = await _context.ApiResources.FindAsync(id);
                if (apiresource == null)
                {
                    return NotFound();
                }
                var result = _context.Update(apiresource);
                if (_context.SaveChanges() == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _context.ApiResources.FindAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default || _context.ApiResources == null)
            {
                return NotFound();
            }
            var apiresource = await _context.ApiResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiresource == null)
            {
                return NotFound();
            }

            return View(apiresource);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ApiResources == null)
            {
                return Problem("Entity set 'ApiResources'  is null.");
            }
            var apiresource = await _context.ApiResources.FindAsync(id);
            if (apiresource != null)
            {
                var result = _context.ApiResources.Remove(apiresource);
                if (_context.SaveChanges() == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }
    }
}
