using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;
using HorsesForCourses.MVC.Models.Account;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Accounts.A_RegisterAccount;

public class B_RegisterAccountMVC : AccountMVCControllerTests
{
    private readonly RegisterAccountViewModel viewModel = new()
    {
        Name = TheCanonical.CoachName,
        Email = TheCanonical.CoachEmail,
        Pass = "pass123",
        PassConfirm = "pass123",
        AsCoach = false
    };

    [Fact]
    public async Task RegisterCoach_GET_Passes_The_Model_To_The_View()
    {
        var result = await controller.Register();
        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<RegisterAccountViewModel>(view.Model);
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Equal(string.Empty, viewModel.Email);
    }

    [Fact]
    public async Task RegisterCoach_POST_calls_the_service()
    {
        await controller.Register(viewModel);
        service.Verify(a => a.Register(TheCanonical.CoachName, TheCanonical.CoachEmail, "pass123", "pass123", false));
    }

    [Fact]
    public async Task Register_POST_Redirects_To_Index_On_Success()
    {
        var result = await controller.Register(viewModel);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Home", redirect.ControllerName);
        Assert.Equal("Index", redirect.ActionName);
    }

    private void MakeServiceMethodThrow()
        => service
            .Setup(a => a.Register(
                It.IsAny<string>(), // name
                It.IsAny<string>(), // email
                It.IsAny<string>(), // pass
                It.IsAny<string>(), // passconfirm
                false))
            .ThrowsAsync(new PasswordAndPasswordConfirmDoNotMatch());

    [Fact]
    public async Task Register_POST_Returns_View_On_Exception()
    {
        MakeServiceMethodThrow();
        var result = await controller.Register(viewModel);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RegisterAccountViewModel>(view.Model);
        Assert.Equal(TheCanonical.CoachName, model.Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
    }

    [Fact]
    public async Task RegisterCoach_POST_Returns_View_With_ModelError_On_Exception()
    {
        MakeServiceMethodThrow();
        await controller.Register(viewModel);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(
            e => e.ErrorMessage == "Password and password confirm do not match."));
    }
}