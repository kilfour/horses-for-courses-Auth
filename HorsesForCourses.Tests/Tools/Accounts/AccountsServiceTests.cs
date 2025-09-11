using HorsesForCourses.Service.Warehouse;
using Moq;
using HorsesForCourses.Service.Actors;
using HorsesForCourses.Service.Accounts.GetApplicationUserByEmail;

namespace HorsesForCourses.Tests.Tools.Accounts;

public abstract class AccountsServiceTests
{
    protected readonly IAccountsService service;

    protected readonly Mock<IGetApplicationUserByEmail> getApplicationUserByEmail;
    protected readonly Mock<IAmASuperVisor> supervisor;

    public AccountsServiceTests()
    {
        supervisor = new Mock<IAmASuperVisor>();
        getApplicationUserByEmail = new Mock<IGetApplicationUserByEmail>();
        service = new AccountsService(supervisor.Object, getApplicationUserByEmail.Object);
    }
}
