using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests.Tools.Courses;

public class CourseSpy : Course
{
    public CourseSpy() : base(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd) { }
    public bool RequiredSkillsCalled;
    public IEnumerable<string>? RequiredSkillsSeen;
    public override Course UpdateRequiredSkills(Actor actor, IEnumerable<string> skills)
    {
        RequiredSkillsCalled = true; RequiredSkillsSeen = skills;
        base.UpdateRequiredSkills(actor, skills);
        return this;
    }

    public bool TimeSlotsCalled;
    public IEnumerable<TimeSlot>? TimeSlotsSeen;

    public override Course UpdateTimeSlots<T>(Actor actor, IEnumerable<T> timeSlotInfo, Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        TimeSlotsCalled = true;
        base.UpdateTimeSlots(actor, timeSlotInfo, getTimeSlot);
        TimeSlotsSeen = TimeSlots;
        return this;
    }

    public bool AssignCoachCalled;
    public Coach? AssignCoachSeen;
    public override Course AssignCoach(Actor actor, Coach coach)
    {
        AssignCoachCalled = true; AssignCoachSeen = coach;
        base.AssignCoach(actor, coach);
        return this;
    }
}