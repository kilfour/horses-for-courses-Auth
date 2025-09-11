using HorsesForCourses.Service.Warehouse;
using Moq;
using HorsesForCourses.Service.Actors;

namespace HorsesForCourses.Tests.Tools.Accounts;

public abstract class AccountsServiceTests
{
    protected readonly IAccountsService service;
    protected readonly Mock<IAmASuperVisor> supervisor;

    public AccountsServiceTests()
    {
        supervisor = new Mock<IAmASuperVisor>();
        service = new AccountsService(supervisor.Object);
    }
}
