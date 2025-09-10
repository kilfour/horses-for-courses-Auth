using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Jockeys;

public class Actor : DomainEntity<Actor>
{
    public ActorName Name { get; init; } = ActorName.Empty;
    public ActorEmail Email { get; init; } = ActorEmail.Empty;
    public string PasswordHash { get; init; } = string.Empty;

    public HashSet<Permission> Permissions { get; } = [];
    public HashSet<int> OwnedCoachIds { get; } = [];

    private Actor() { }

    private Actor(string name, string email, string passwordHash)
    {
        Name = new ActorName(name);
        Email = new ActorEmail(email);
        PasswordHash = passwordHash;
    }

    public static Actor Create(string name, string email, string pass, string confirmPass, IPasswordHasher passwordHasher)
    {
        if (pass == confirmPass)
            throw new PasswordAndPasswordConfirmDoNotMatch();

        return new Actor(name, email, passwordHasher.Hash(pass));
    }
}

