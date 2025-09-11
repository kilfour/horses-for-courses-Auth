using System.Security.Claims;

namespace HorsesForCourses.Core.Domain.Accounts;

public record Actor
{
    private readonly Dictionary<string, string> stakes = [];
    private bool HasStake(string type, string value)
    {
        if (!stakes.ContainsKey(type)) return false;
        if (stakes[type] != value) return false;
        return true;
    }

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

    public void IsAuthenticated()
    {
        if (stakes.Count == 0) // no name, no nothing, empty claims => not authenticated
            throw new UnauthorizedAccessException();
    }

    public void CanEditCoach(int coachId)
    {
        if (HasStake(ClaimTypes.Role, "admin")) return;
        if (HasStake(ClaimTypes.Role, "coach") && HasStake("owned-coach", coachId.ToString())) return;
    }
}