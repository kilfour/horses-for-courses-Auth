using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using WibblyWobbly;


namespace HorsesForCourses.Tests.Courses.E_AssignCoach;

public class E_AssignCoachDomain : CourseDomainTests
{
    [Fact]
    public void AssignCoach_WithValidData_ShouldSucceed()
    {
        Entity
            .UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(TheCanonical.AdminActor())
            .AssignCoach(TheCanonical.AdminActor(), TheCanonical.Coach());
        Assert.NotNull(Entity.AssignedCoach);
        Assert.Equal(TheCanonical.CoachName, Entity.AssignedCoach.Name.Value);
    }

    [Fact]
    public void AssignCoach_When_Unconfirmed_Throws()
        => Assert.Throws<CourseNotYetConfirmed>(() =>
            Entity
                .UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
                .AssignCoach(TheCanonical.AdminActor(), TheCanonical.Coach()));

    [Fact]
    public void AssignCoach_Twice_Throws()
    {
        Entity
            .UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(TheCanonical.AdminActor()).AssignCoach(TheCanonical.AdminActor(), TheCanonical.Coach());
        Assert.Throws<CourseAlreadyHasCoach>(() => Entity.AssignCoach(TheCanonical.AdminActor(), TheCanonical.Coach()));
    }

    [Fact]
    public void Coach_Lacking_Skill_Throws()
    {
        Entity
            .UpdateRequiredSkills(TheCanonical.AdminActor(), ["not this one"])
            .UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(TheCanonical.AdminActor());
        Assert.Throws<CoachNotSuitableForCourse>(() => Entity.AssignCoach(TheCanonical.AdminActor(), TheCanonical.Coach()));
    }

    [Fact]
    public void CoachUnavailable_Throws()
    {
        var coach = TheCanonical.Coach();
        var course = TheCanonical.Course()
            .UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(TheCanonical.AdminActor())
            .AssignCoach(TheCanonical.AdminActor(), coach);
        Entity.UpdateTimeSlots(TheCanonical.AdminActor(), TheCanonical.TimeSlotsFullDayMonday(), a => a).Confirm(TheCanonical.AdminActor());
        Assert.Throws<CoachNotAvailableForCourse>(() => Entity.AssignCoach(TheCanonical.AdminActor(), coach));
    }

    private static void AssignTheCoach(Course courseA, Course courseB)
    {
        var coach = TheCanonical.Coach();
        courseA.Confirm(TheCanonical.AdminActor()).AssignCoach(TheCanonical.AdminActor(), coach);
        courseB.Confirm(TheCanonical.AdminActor()).AssignCoach(TheCanonical.AdminActor(), coach);
    }

    [Fact]
    public void CoachUnavailable_Case_1_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 19.August(2025)).FullDayOnMonday();
        var courseB = new CourseB(19.August(2025), 19.August(2025)).FullDayOnMonday();
        AssignTheCoach(courseA, courseB);
    }

    [Fact]
    public void CoachUnavailable_Case_2_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 19.August(2025)).FullDayOnTuesday();
        var courseB = new CourseB(20.August(2025), 25.August(2025)).FullDayOnTuesday(); // no tuesday here
        AssignTheCoach(courseA, courseB);
    }

    [Fact]
    public void CoachUnavailable_Case_3_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, 26.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 22.August(2025)).FullDayOnTuesday();
        var courseB = new CourseB(20.August(2025), 30.August(2025)).FullDayOnTuesday();
        AssignTheCoach(courseA, courseB);
    }
}
