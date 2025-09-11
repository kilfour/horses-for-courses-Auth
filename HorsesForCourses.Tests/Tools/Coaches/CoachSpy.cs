using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Tests.Tools.Coaches;

public class CoachSpy : Coach
{
    public CoachSpy() : base(TheCanonical.CoachName, TheCanonical.CoachEmail) { }
    public bool UpdateSkillsCalled;
    public IEnumerable<string>? UpdateSkillsSeen;
    public override void UpdateSkills(Actor actor, IEnumerable<string> skills)
    {
        UpdateSkillsCalled = true; UpdateSkillsSeen = skills;
        base.UpdateSkills(actor, skills);
    }
}