using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;
using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Core.Domain.Accounts;

public class ApplicationUser : DomainEntity<ApplicationUser>
{
    public ApplicationUserName Name { get; init; } = ApplicationUserName.Empty;
    public ApplicationUserEmail Email { get; init; } = ApplicationUserEmail.Empty;
    public string PasswordHash { get; init; } = string.Empty;

    public HashSet<Permission> Permissions { get; } = [];
    public Id<Coach> OwnedCoach { get; } = Id<Coach>.Empty;

    private ApplicationUser() { }

    private ApplicationUser(string name, string email, string passwordHash)
    {
        Name = new ApplicationUserName(name);
        Email = new ApplicationUserEmail(email);
        PasswordHash = passwordHash;
    }

    public static ApplicationUser Create(string name, string email, string pass, string confirmPass)
    {
        if (pass != confirmPass)
            throw new PasswordAndPasswordConfirmDoNotMatch();

        if (string.IsNullOrWhiteSpace(pass))
            throw new PasswordCanNotBeEmpty();

        return new ApplicationUser(name, email, new Pbkdf2PasswordHasher().Hash(pass));
    }
}

