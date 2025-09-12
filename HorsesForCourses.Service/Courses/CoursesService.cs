using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Courses;

public interface ICoursesService
{
    Task<IdPrimitive> CreateCourse(Actor actor, string name, DateOnly startDate, DateOnly endDate);
    Task<bool> UpdateRequiredSkills(Actor actor, IdPrimitive id, IEnumerable<string> skills);
    Task<bool> UpdateTimeSlots<T>(
        Actor actor,
        IdPrimitive id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot);
    Task<bool> ConfirmCourse(Actor actor, IdPrimitive id);
    Task<bool> AssignCoach(Actor actor, IdPrimitive courseId, IdPrimitive coachId);
    Task<PagedResult<CourseSummary>> GetCourses(int page, int pageSize);
    Task<CourseDetail?> GetCourseDetail(IdPrimitive id);
}

public class CoursesService(
    IAmASuperVisor Supervisor,
    IGetCourseById GetCourseById,
    IGetCoachById GetCoachById,
    IGetCourseSummaries GetCourseSummaries,
    IGetCourseDetail GetCourseDetailQuery) : ICoursesService
{
    public async Task<IdPrimitive> CreateCourse(Actor actor, string name, DateOnly start, DateOnly end)
    {
        var course = Course.Create(actor, name, start, end);
        await Supervisor.Enlist(course);
        await Supervisor.Ship();
        return course.Id.Value;
    }

    public async Task<bool> UpdateRequiredSkills(Actor actor, IdPrimitive id, IEnumerable<string> skills)
    {
        var course = await GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateRequiredSkills(actor, skills);
        await Supervisor.Ship();
        return true;
    }

    public async Task<bool> UpdateTimeSlots<T>(
        Actor actor,
        IdPrimitive id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        var course = await GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateTimeSlots(actor, timeSlotInfo, getTimeSlot);
        await Supervisor.Ship();
        return true;
    }

    public async Task<bool> ConfirmCourse(Actor actor, IdPrimitive id)
    {
        var course = await GetCourseById.Load(id);
        if (course == null) return false;
        course.Confirm(actor);
        await Supervisor.Ship();
        return true;
    }

    public async Task<bool> AssignCoach(Actor actor, IdPrimitive courseId, IdPrimitive coachId)
    {
        var course = await GetCourseById.Load(courseId);
        var coach = await GetCoachById.One(coachId);
        if (course == null || coach == null) return false;
        course.AssignCoach(actor, coach);
        await Supervisor.Ship();
        return true;
    }

    public async Task<PagedResult<CourseSummary>> GetCourses(int page, int pageSize)
        => await GetCourseSummaries.Paged(new PageRequest(page, pageSize));

    public async Task<CourseDetail?> GetCourseDetail(IdPrimitive id)
        => await GetCourseDetailQuery.One(id);
}