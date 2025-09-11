using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;


namespace HorsesForCourses.Tests.Tools.Accounts;

public class ApplicationUserSpy : ApplicationUser
{
    public bool CheckPasswordCalled;
    public string CheckPasswordSeen = string.Empty;
    public override void CheckPassword(string password)
    {
        CheckPasswordCalled = true; CheckPasswordSeen = password;
    }
}