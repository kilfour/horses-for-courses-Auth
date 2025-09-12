using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;

namespace HorsesForCourses.Tests;

public class CoachUnavailableForCourseUpdaterFromCoachTests
{
    static async IAsyncEnumerable<Course> One(Course course)
    {
        yield return course;
        await Task.CompletedTask;
    }

    static async IAsyncEnumerable<T> AsAsync<T>(IEnumerable<T> src)
    {
        foreach (var x in src) yield return x;
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Initial()
    {
        var actor = TheCanonical.AdminActor();
        var coach = TheCanonical.Coach();
        var course = TheCanonical.Course()
            .UpdateTimeSlots(actor, TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(actor);
        var results = new List<UnavailableFor>();
        await foreach (var item in CoachUnavailableForCourseUpdater.FromCoachAsync(coach, One(course)))
        {
            results.Add(item);
        }
        Assert.Equal([], results);
    }

    [Fact]
    public async Task First_no_go()
    {
        var actor = TheCanonical.AdminActor();
        var coach = TheCanonical.Coach();
        var course = TheCanonical.Course()
            .UpdateTimeSlots(actor, TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(actor)
            .AssignCoach(actor, coach);
        var courseTwo = TheCanonical.Course()
            .UpdateTimeSlots(actor, TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm(actor);
        var results = new List<UnavailableFor>();
        await foreach (var item in CoachUnavailableForCourseUpdater.FromCoachAsync(coach, One(courseTwo)))
        {
            results.Add(item);
        }
        Assert.Single(results);
    }
}


