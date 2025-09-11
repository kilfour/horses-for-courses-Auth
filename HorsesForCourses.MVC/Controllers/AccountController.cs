using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HorsesForCourses.MVC.Models.Account;
using HorsesForCourses.MVC.Controllers.Abstract;
using HorsesForCourses.Service.Accounts;

namespace HorsesForCourses.MVC.Controllers;

public class AccountController : MvcController
{
    private readonly IAccountsService accountService;
    private readonly IAuthenticationService authenticationService;

    public AccountController(IAccountsService accountService, IAuthenticationService authenticationService)
    {
        this.accountService = accountService;
        this.authenticationService = authenticationService;
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

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return await Task.FromResult(View(new LoginViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        return await This(async () =>
            {
                var claims = await accountService.Login(email, password);
                var id = new ClaimsIdentity(claims, "Cookies");
                await authenticationService.SignInAsync(HttpContext, "Cookies", new ClaimsPrincipal(id), new AuthenticationProperties());
            })
            .OnSuccess(() => RedirectToAction(nameof(HomeController.Index), "Home"))
            .OnException(() => View(new LoginViewModel { Email = email }));

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