using System.Security.Claims;

namespace HorsesForCourses.Core.Domain.Accounts;

public record Actor
{
    private readonly Dictionary<string, string> stakes = [];

    public Actor Declare(string type, string value)
    {
        stakes[type] = value;
        return this;
    }

    public IEnumerable<Claim> ClaimIt()
        => stakes.Select(a => new Claim(a.Key, a.Value));

    public static Actor From(IEnumerable<Claim> claims)
    {
        var actor = new Actor();
        claims.ToList().ForEach(a => actor.Declare(a.Type, a.Value));
        return actor;
    }

    public void JustIs()
    {
        if (stakes.Count == 0) // no name, no nothing, empty claims => not authenticated
            throw new UnauthorizedAccessException();
    }
}