using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;

namespace HorsesForCourses.Tests.Accounts.A_RegisterAccount;

public class D_RegisterAccountData : DatabaseTests
{
    private readonly DataSupervisor supervisor;
    private readonly ApplicationUser applicationUser;

    public D_RegisterAccountData()
    {
        supervisor = new DataSupervisor(GetDbContext());
        applicationUser = TheCanonical.ApplicationUser();
    }

    private async Task Act()
    {
        await supervisor.Enlist(applicationUser);
        await supervisor.Ship();
    }

    private ApplicationUser Reload()
        => GetDbContext().ApplicationUsers.Find(Id<ApplicationUser>.From(applicationUser.Id.Value))!;

    [Fact]
    public async Task Supervisor_Assigns_id()
    {
        await Act();
        Assert.NotEqual(default, applicationUser.Id.Value);
    }

    [Fact]
    public async Task Supervisor_Stores()
    {
        await Act();
        var reloaded = Reload();
        Assert.NotNull(reloaded);
        Assert.NotEqual(default, reloaded.Id.Value);
    }

    [Fact]
    public async Task Supervisor_name_and_email()
    {
        await Act();
        var reloaded = Reload();
        Assert.Equal(TheCanonical.CoachName, reloaded!.Name.Value);
        Assert.Equal(TheCanonical.CoachEmail, reloaded!.Email.Value);
    }
}
