using System.Security.Claims;

namespace HorsesForCourses.Core.Domain.Accounts;

public record Actor
{
    // ------------------------------------------------------------
    // -- BusinessRules to Stakes, .... not quite yet
    //        => see `ApplicationUser.EnterScene()`
    public Actor Declare(string type, string value)
    {
        stakes[type] = value;
        return this;
    }
    // ------------------------------------------------------------
    // -- Stakes To BusinesRules
    public void IsAuthenticated()
    {
        if (stakes.Count == 0) // no name, no nothing, empty claims => not authenticated
            throw new UnauthorizedAccessException();
    }

    public void CanEditCoach(string coachEmail)
    {
        if (HasStake(ClaimTypes.Role, "admin")) return;
        if (HasStake(ClaimTypes.Role, "coach") && HasStake(ClaimTypes.Email, coachEmail)) return;
        throw new UnauthorizedAccessException();
    }
    // ------------------------------------------------------------
    // -- To Claims and Back
    public IEnumerable<Claim> ClaimIt()
        => stakes.Select(a => new Claim(a.Key, a.Value));
    public static Actor From(IEnumerable<Claim> claims)
    {
        var actor = new Actor();
        claims.ToList().ForEach(a => actor.Declare(a.Type, a.Value));
        return actor;
    }
    // ------------------------------------------------------------
    // -- internals
    private readonly Dictionary<string, string> stakes = [];
    private bool HasStake(string type, string value)
    {
        if (!stakes.ContainsKey(type)) return false;
        if (stakes[type] != value) return false;
        return true;
    }
    // ------------------------------------------------------------
}