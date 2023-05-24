using System.Text;
using System.Text.Encodings.Web;

using AuthService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

using X.PagedList;

namespace AuthService.Controllers
{

    /// <summary>
    /// 用户管理
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<IdentityUser> userManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            int pageNumber = page;

            var data = _userManager.Users;     

            var paging = new PagedList<IdentityUser>(data,pageNumber,pageSize);
        
            return View(paging);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(RegisterModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                    pageHandler: null,
                        values: new { area = "Identity", userId, code, },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");
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

            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(string id,IdentityUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                var result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
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

            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }
            var movie = await _userManager.Users
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
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'User'  is null.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result=await _userManager.DeleteAsync(user);
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
}
