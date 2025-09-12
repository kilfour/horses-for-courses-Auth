using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Accounts;
using Moq;

namespace HorsesForCourses.Tests.Accounts.A_RegisterAccount;

public class C_RegisterAccountService : AccountsServiceTests
{
    [Fact]
    public async Task Register_delivers_the_ApplicationUser_to_the_supervisor()
    {
        await service.Register(TheCanonical.CoachName, TheCanonical.CoachEmail, TheCanonical.Password, TheCanonical.Password, false, false);
        supervisor.Verify(a => a.Enlist(
            It.Is<ApplicationUser>(a =>
                a.Name.Value == TheCanonical.CoachName &&
                a.Email.Value == TheCanonical.CoachEmail)));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task Register_Does_Not_Ship_On_Exception()
    {
        await Assert.ThrowsAnyAsync<DomainException>(async () =>
            await service.Register(TheCanonical.CoachName, TheCanonical.CoachEmail, TheCanonical.Password, "PLOEF", false, false));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }
}