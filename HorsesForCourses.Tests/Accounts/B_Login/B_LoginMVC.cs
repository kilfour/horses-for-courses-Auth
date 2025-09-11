using System.Linq.Expressions;
using System.Security.Claims;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;
using HorsesForCourses.MVC.Models.Account;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Accounts.B_Login;

public class B_LoginMVC : AccountMVCControllerTests
{

    private async Task<IActionResult> DoValidLogin() =>
        await controller.Login(TheCanonical.CoachEmail, TheCanonical.Password);

    [Fact]
    public async Task Login_GET_Passes_The_Model_To_The_View()
    {
        var result = await controller.Login();
        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<LoginViewModel>(view.Model);
        Assert.Equal(string.Empty, viewModel.Email);
    }

    [Fact]
    public async Task Login_POST_calls_the_service()
    {
        await DoValidLogin();
        service.Verify(a => a.Login(TheCanonical.CoachEmail, TheCanonical.Password));
    }

    [Fact]
    public async Task Login_POST_calls_the_auth_service_with_claims()
    {
        service.Setup(a => a.Login(TheCanonical.CoachEmail, TheCanonical.Password))
            .ReturnsAsync([new Claim("claimtype", "claimvalue")]);
        await DoValidLogin();
        Expression<Func<ClaimsPrincipal, bool>> predicate =
            a => a.Claims.SingleOrDefault(a => a.Type == "claimtype" && a.Value == "claimvalue") != null;
        authenticationService.Verify(a =>
            a.SignInAsync(null!, "Cookies", It.Is(predicate), It.IsAny<AuthenticationProperties>()));
    }

    [Fact]
    public async Task Login_POST_Redirects_To_Index_On_Success()
    {
        var result = await DoValidLogin();
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Home", redirect.ControllerName);
        Assert.Equal("Index", redirect.ActionName);
    }

    private void MakeServiceMethodThrow()
        => service
            .Setup(a => a.Login(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ThrowsAsync(new EmailOrPasswordAreInvalid());

    [Fact]
    public async Task Login_POST_Returns_View_On_Exception()
    {
        MakeServiceMethodThrow();
        var result = await DoValidLogin();
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LoginViewModel>(view.Model);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
        Assert.Equal(string.Empty, model.Password);
    }

    [Fact]
    public async Task Login_POST_does_not_calls_the_auth_service_on_exception()
    {
        service.Setup(a => a.Login(TheCanonical.CoachEmail, TheCanonical.Password))
            .ReturnsAsync([new Claim("claimtype", "claimvalue")]);
        MakeServiceMethodThrow();
        await DoValidLogin();
        Expression<Func<ClaimsPrincipal, bool>> predicate =
            a => a.Claims.SingleOrDefault(a => a.Type == "claimtype" && a.Value == "claimvalue") != null;
        authenticationService.Verify(a =>
            a.SignInAsync(null!, "Cookies", It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Never);
    }

    [Fact]
    public async Task Login_POST_Returns_View_With_ModelError_On_Exception()
    {
        MakeServiceMethodThrow();
        var result = await DoValidLogin();
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(
            e => e.ErrorMessage == "Email or password are invalid."));
    }
}