using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;


namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class E_RegisterCoachDomain : CoachDomainTests
{
    [Fact]
    public void RegisterCoach_WithValidData_ShouldSucceed()
    {
        Assert.Equal(TheCanonical.CoachName, Entity.Name.Value);
        Assert.Equal(TheCanonical.CoachEmail, Entity.Email.Value);
        Assert.Empty(Entity.Skills);
    }

    [Fact]
    public void RegisterCoach_WithValidData_does_not_assign_id()
        => Assert.Equal(default, Entity.Id.Value);

    [Fact]
    public void RegisterCoach_WithEmptyName_ShouldThrow()
        => Assert.Throws<CoachNameCanNotBeEmpty>(
            () => Coach.Create(TheCanonical.AdminActor(), string.Empty, TheCanonical.CoachEmail));

    [Fact]
    public void RegisterCoach_WithLongName_ShouldThrow()
        => Assert.Throws<CoachNameCanNotBeTooLong>(
            () => Coach.Create(TheCanonical.AdminActor(), new string('-', 101), TheCanonical.CoachEmail));

    [Fact]
    public void RegisterCoach_WithInvalidActor_ShouldThrow()
        => Assert.Throws<CoachNameCanNotBeTooLong>(
            () => Coach.Create(TheCanonical.AdminActor(), new string('-', 101), TheCanonical.CoachEmail));

    [Fact]
    public void RegisterCoach_WithEmptyEmail_ShouldThrow()
        => Assert.Throws<CoachEmailCanNotBeEmpty>(
            () => Coach.Create(TheCanonical.AdminActor(), TheCanonical.CoachName, string.Empty));

    [Fact]
    public void RegisterCoach_WithLongEmail_ShouldThrow()
        => Assert.Throws<CoachEmailCanNotBeTooLong>(
            () => Coach.Create(TheCanonical.AdminActor(), TheCanonical.CoachName, new string('-', 101)));


    [Fact]
    public void RegisterCoach_With_unauthenticated_ShouldThrow()
        => Assert.Throws<UnauthorizedAccessException>(
            () => Coach.Create(TheCanonical.EmptyActor, TheCanonical.CoachName, new string('-', 101)));

    [Fact]
    public void RegisterCoach_With_non_admin_ShouldThrow()
        => Assert.Throws<UnauthorizedAccessException>(
            () => Coach.Create(TheCanonical.CoachActor(), TheCanonical.CoachName, new string('-', 101)));
}
