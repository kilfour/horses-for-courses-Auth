using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Accounts.A_RegisterAccount;

public class E_RegisterAccountDomain
{
    private ApplicationUser CreateValidEntity(string role = "")
        => ApplicationUser.Create(
            TheCanonical.CoachName,
            TheCanonical.CoachEmail,
            TheCanonical.Password,
            TheCanonical.Password,
            role);

    [Fact]
    public void RegisterAccount_WithValidData_ShouldSucceed()
    {
        var applicationUser = CreateValidEntity("any role");
        Assert.Equal(TheCanonical.CoachName, applicationUser.Name.Value);
        Assert.Equal(TheCanonical.CoachEmail, applicationUser.Email.Value);
        Assert.Equal("any role", applicationUser.Role);
        Assert.NotEmpty(applicationUser.PasswordHash);
    }

    [Fact]
    public void RegisterAccount_WithValidData_does_not_assign_id()
        => Assert.Equal(default, CreateValidEntity().Id.Value);

    [Fact]
    public void RegisterAccount_WithEmptyName_ShouldThrow()
        => Assert.Throws<ApplicationUserNameCanNotBeEmpty>(
            () => ApplicationUser.Create(
                    "",
                    TheCanonical.CoachEmail,
                    TheCanonical.Password,
                    TheCanonical.Password,
                    ""));

    [Fact]
    public void RegisterAccount_WithLongName_ShouldThrow()
        => Assert.Throws<ApplicationUserNameCanNotBeTooLong>(
            () => ApplicationUser.Create(
            new string('-', 101),
            TheCanonical.CoachEmail,
            TheCanonical.Password,
            TheCanonical.Password,
            ""));

    [Fact]
    public void RegisterAccount_WithEmptyEmail_ShouldThrow()
        => Assert.Throws<ApplicationUserEmailCanNotBeEmpty>(
            () => ApplicationUser.Create(
            TheCanonical.CoachName,
            "",
            TheCanonical.Password,
            TheCanonical.Password,
            ""));

    [Fact]
    public void RegisterAccount_WithLongEmail_ShouldThrow()
        => Assert.Throws<ApplicationUserEmailCanNotBeTooLong>(
            () => ApplicationUser.Create(
            TheCanonical.CoachName,
            new string('-', 101),
            TheCanonical.Password,
            TheCanonical.Password,
            ""));

    [Fact]
    public void RegisterAccount_passwords_no_match_ShouldThrow()
        => Assert.Throws<PasswordAndPasswordConfirmDoNotMatch>(
            () => ApplicationUser.Create(
            TheCanonical.CoachName,
            new string('-', 101),
            TheCanonical.Password,
            "other"
            , ""));

    [Fact]
    public void RegisterAccount_passwords_empty_ShouldThrow()
        => Assert.Throws<PasswordCanNotBeEmpty>(
            () => ApplicationUser.Create(
            TheCanonical.CoachName,
            new string('-', 101),
            "",
            "",
            ""));
}
