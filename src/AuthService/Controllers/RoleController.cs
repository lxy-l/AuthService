using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers;

/// <summary>
/// 角色管理
/// </summary>
[Authorize(Policy = "AdminOnly")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var list= await _roleManager.Roles.ToListAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(IdentityRole Input)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleManager.CreateAsync(Input);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(Input);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var model = await _roleManager.FindByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(string id, IdentityRole model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            role.Name=model.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var model = await _roleManager.FindByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _roleManager.Roles == null)
        {
            return NotFound();
        }
        var movie = await _roleManager.Roles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_roleManager.Roles == null)
        {
            return Problem("Entity set 'Role'  is null.");
        }
        var user = await _roleManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _roleManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return NotFound();
    }

}