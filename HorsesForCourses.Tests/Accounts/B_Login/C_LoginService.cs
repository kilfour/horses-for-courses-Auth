using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Accounts;
using Moq;

namespace HorsesForCourses.Tests.Accounts.B_Login;

public class C_LoginService : AccountsServiceTests
{
    // [Fact]
    // public async Task Login_uses_the_query_object()
    // {
    //     await service.Login(TheCanonical.CoachName, TheCanonical.Password);
    //     supervisor.Verify(a => a.Enlist(
    //         It.Is<ApplicationUser>(a =>
    //             a.Name.Value == TheCanonical.CoachName &&
    //             a.Email.Value == TheCanonical.CoachEmail)));
    //     supervisor.Verify(a => a.Ship());
    // }

    // [Fact]
    // public async Task Login_Does_Not_Ship_On_Exception()
    // {
    //     await Assert.ThrowsAnyAsync<DomainException>(async () =>
    //         await service.Login(TheCanonical.CoachName, TheCanonical.CoachEmail, TheCanonical.Password, "PLOEF", false));
    //     supervisor.Verify(a => a.Ship(), Times.Never);
    // }
}