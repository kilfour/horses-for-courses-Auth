using System.Security.Claims;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Actors;

namespace HorsesForCourses.Service.Actors;

public static class Mint
{


    public static IEnumerable<Claim> ClaimsFromActor(ApplicationUser actor)
    {
        var claims = new List<Claim>();
        foreach (var permission in actor.Permissions)
        {
            claims.AddRange(permission switch
            {
                Permission.UpdateCoachSkills => GetCoachUpdateSkillsClaims(actor),
                _ => throw new NotImplementedException()
            });
        }
        return claims;
    }

    private static IEnumerable<Claim> GetCoachUpdateSkillsClaims(ApplicationUser actor)
    {
        return []; //actor.OwnedCoachIds.Select(a => new Claim("capability", $"coach:update-skills-{a}"));
    }

    //     public static Capability CapabilityFromActor()
    //     {

    //     }
}