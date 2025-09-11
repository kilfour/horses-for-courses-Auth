using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Service.Accounts.GetApplicationUserByEmail;
using HorsesForCourses.Tests.Tools;

namespace HorsesForCourses.Tests.Accounts.A_Login;

public class D_LoginData : DatabaseTests
{
    private IdPrimitive IdAssignedByDb;

    private async Task<ApplicationUser?> Act()
        => await new GetApplicationUserByEmail(GetDbContext()).One(TheCanonical.CoachEmail);

    [Fact]
    public async Task LoadIt()
    {
        IdAssignedByDb = AddToDb(TheCanonical.ApplicationUser());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(IdAssignedByDb, result.Id.Value);
        Assert.Equal(TheCanonical.CoachName, result.Name.Value);
        Assert.Equal(TheCanonical.CoachEmail, result.Email.Value);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}
