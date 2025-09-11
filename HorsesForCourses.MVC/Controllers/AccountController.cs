using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HorsesForCourses.MVC.Models.Account;
using HorsesForCourses.Service.Actors;
using HorsesForCourses.MVC.Controllers.Abstract;

namespace HorsesForCourses.MVC.Controllers;

public class AccountController : MvcController
{
    private readonly IAccountsService accountService;

    public AccountController(IAccountsService accountService)
    {
        this.accountService = accountService;
    }
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

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return await Task.FromResult(View(new RegisterAccountViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterAccountViewModel viewModel)
        => await This(async () => await accountService.Register(
                viewModel.Name,
                viewModel.Email,
                viewModel.Pass,
                viewModel.PassConfirm,
                viewModel.AsCoach))
            .OnSuccess(() => RedirectToAction(nameof(Index), "Home"))
            .OnException(() => View(viewModel));

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