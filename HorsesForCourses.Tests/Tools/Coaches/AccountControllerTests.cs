using HorsesForCourses.Service.Actors;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class AccountControllerTests
{
    protected readonly Mock<IAccountsService> service;

    public AccountControllerTests()
    {
        service = new Mock<IAccountsService>();
    }
}
