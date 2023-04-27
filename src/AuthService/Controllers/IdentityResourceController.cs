using IdentityServer7.EntityFramework.Storage.DbContexts;
using IdentityServer7.EntityFramework.Storage.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{

    /// <summary>
    /// 认证资源管理
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    public class IdentityResourceController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public IdentityResourceController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list= await _context.IdentityResources.ToListAsync();
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

            var model = await _context.IdentityResources.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, IdentityResource model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var client = await _context.IdentityResources.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                var result = _context.Update(client);
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
            var model = await _context.IdentityResources.FindAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default || _context.IdentityResources == null)
            {
                return NotFound();
            }
            var client = await _context.IdentityResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.IdentityResources == null)
            {
                return Problem("Entity set 'IdentityResources'  is null.");
            }
            var client = await _context.IdentityResources.FindAsync(id);
            if (client != null)
            {
                var result = _context.IdentityResources.Remove(client);
                if (_context.SaveChanges() == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }
    }
}
