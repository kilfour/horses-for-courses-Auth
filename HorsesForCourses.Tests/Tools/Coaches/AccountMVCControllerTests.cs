
using HorsesForCourses.MVC.Controllers;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class AccountMVCControllerTests : AccountControllerTests
{
    protected readonly AccountController controller;

    public AccountMVCControllerTests()
    {
        controller = new AccountController(service.Object, authenticationService.Object);
    }
}
