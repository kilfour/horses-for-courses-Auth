
using HorsesForCourses.MVC.Controllers;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class AccountMVCControllerTests : CoachesControllerTests
{
    protected readonly AccountController controller;

    public AccountMVCControllerTests()
    {
        controller = new AccountController();
    }
}
