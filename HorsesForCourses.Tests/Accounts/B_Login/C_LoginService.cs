using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Accounts;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Accounts.B_Login;

public class C_LoginService : AccountsServiceTests
{
    [Fact]
    public async Task Login_uses_the_query_object()
    {
        getApplicationUserByEmail.Setup(a => a.One(TheCanonical.CoachEmail)).ReturnsAsync(TheCanonical.ApplicationUser());
        await service.Login(TheCanonical.CoachEmail, TheCanonical.Password);
        getApplicationUserByEmail.Verify(a => a.One(TheCanonical.CoachEmail));
    }

    [Fact]
    public async Task Login_throws_when_query_returns_null()
    {
        getApplicationUserByEmail.Setup(a => a.One(TheCanonical.CoachEmail)).ReturnsAsync((ApplicationUser)null!);
        var ex = await Assert.ThrowsAsync<EmailOrPasswordAreInvalid>(() => service.Login(TheCanonical.CoachEmail, TheCanonical.Password));
    }

    [Fact]
    public async Task Login_calls_the_domain()
    {
        var spy = new ApplicationUserSpy();
        getApplicationUserByEmail
            .Setup(a => a.One(TheCanonical.CoachEmail))
            .ReturnsAsync(spy);
        await service.Login(TheCanonical.CoachEmail, TheCanonical.Password);
        Assert.True(spy.CheckPasswordCalled);
        Assert.Equal(TheCanonical.Password, spy.CheckPasswordSeen);
    }

    // TODO : Login calls the domain again to create an actor and pass on the Claims

    [Fact]
    public async Task Login_throws_when_hash_does_not_match_the_application_user_one()
    {
        getApplicationUserByEmail.Setup(a => a.One(TheCanonical.CoachEmail)).ReturnsAsync((ApplicationUser)null!);
        var ex = await Assert.ThrowsAsync<EmailOrPasswordAreInvalid>(() => service.Login(TheCanonical.CoachEmail, TheCanonical.Password));

    }
}