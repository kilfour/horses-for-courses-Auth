using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;

namespace HorsesForCourses.Core.Domain;

public class UnavailableFor(Id<Coach> CoachId, Id<Course> CourseId) : DomainEntity<UnavailableFor>
{
    public Id<Coach> CoachId { get; } = CoachId;
    public Id<Course> CourseId { get; } = CourseId;
}
