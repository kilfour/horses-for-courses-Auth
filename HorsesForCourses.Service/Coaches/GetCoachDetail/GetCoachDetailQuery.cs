using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Service.Coaches.GetCoachDetail;

public interface IGetCoachDetailQuery
{
    Task<CoachDetail?> One(Actor actor, IdPrimitive id);
}

public class GetCoachDetailQuery(AppDbContext dbContext) : IGetCoachDetailQuery
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<CoachDetail?> One(Actor actor, IdPrimitive id)
    {
        actor.IsAuthenticated();
        var coachId = Id<Coach>.From(id);
        return await dbContext.Coaches
            .AsNoTracking()
            .Where(a => a.Id == coachId)
            .Select(a => new CoachDetail
            {
                Id = a.Id.Value,
                Name = a.Name.Value,
                Email = a.Email.Value,
                Skills = a.Skills.Select(a => a.Value).ToList(),
                Courses = a.AssignedCourses.Select(
                    b => new CoachDetail.CourseInfo(b.Id.Value, b.Name.Value)).ToList()
            }).SingleOrDefaultAsync();
    }
}