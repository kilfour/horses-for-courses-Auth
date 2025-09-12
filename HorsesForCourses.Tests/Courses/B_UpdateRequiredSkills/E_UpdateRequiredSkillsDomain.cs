using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class E_UpdateRequiredSkillsDomain : CourseDomainTests
{
    [Fact]
    public void CreateSkill_Valid_ShouldSucceed()
        => Assert.Equal("DotNet", Skill.From("DotNet").Value);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Skill_Empty_ShouldThrow(string? value)
        => Assert.Throws<SkillValueCanNotBeEmpty>(() => Skill.From(value!));

    [Fact]
    public void UpdateRequiredSkills_WithValidData_ShouldSucceed()
    {
        Entity.UpdateRequiredSkills(TheCanonical.AdminActor(), TheCanonical.Skills);
        Assert.Equal(TheCanonical.HardSkillsList, Entity.RequiredSkills);
    }

    [Fact]
    public void UpdateRequiredSkills_WithInValidSkill_Throws()
    {
        var skills = new List<string> { "" };
        Assert.Throws<SkillValueCanNotBeEmpty>(() => Entity.UpdateRequiredSkills(TheCanonical.AdminActor(), skills));
    }

    [Fact]
    public void UpdateRequiredSkills_With_Duplicates_Throws()
    {
        var skills = new List<string> { "a", "a" };
        Assert.Throws<CourseAlreadyHasSkill>(() => Entity.UpdateRequiredSkills(TheCanonical.AdminActor(), skills));
    }

    [Fact]
    public void UpdateRequiredSkills_When_Confirmed_Throws()
        => Assert.Throws<CourseAlreadyConfirmed>(() =>
            Entity.UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
                .Confirm(TheCanonical.AdminActor())
                .UpdateRequiredSkills(TheCanonical.AdminActor(), ["one"]));

    [Fact]
    public void UpdateRequiredSkills_With_Authenticated_actor_should_throw()
        => Assert.Throws<UnauthorizedAccessException>(
            () => Entity.UpdateRequiredSkills(TheCanonical.AuthenticatedActor(), TheCanonical.Skills));

    [Fact]
    public void UpdateRequiredSkills_With_Couch_actor_should_throw()
        => Assert.Throws<UnauthorizedAccessException>(
            () => Entity.UpdateRequiredSkills(TheCanonical.CoachActor(), TheCanonical.Skills));
}
