using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HorsesForCourses.MVC.Models.Account;
using HorsesForCourses.MVC.Controllers.Abstract;

namespace HorsesForCourses.MVC.Controllers;

public class ActorController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(string name, string email, bool asCoach)
    {
        var claims = new List<Claim> {
            new(ClaimTypes.Name, email)
        };
        var id = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(id));
        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, bool asCoach)
    {
        // var user = AppUser.From(account.Name, account.Email, account.Password, account.PassConfirm, choice);
        // if (choice == "coach") { await _coachService.CreateCoach(new Coach(account.Name, account.Email)); }
        // await _service.CreateUser(user);

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Redirect("/");
    }

    [HttpGet]
    public IActionResult AccessDenied(string? returnUrl = null)
        => View(model: returnUrl);
}