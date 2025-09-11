using HorsesForCourses.Service.Accounts;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class AccountControllerTests
{
    protected readonly Mock<IAccountsService> service;
    protected readonly Mock<IAuthenticationService> authenticationService;

    public AccountControllerTests()
    {
        service = new Mock<IAccountsService>();
        authenticationService = new Mock<IAuthenticationService>();
    }
}
