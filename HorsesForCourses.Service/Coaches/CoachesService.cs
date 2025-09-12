using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Coaches;

public interface ICoachesService
{
    Task<IdPrimitive> RegisterCoach(Actor actor, string name, string email);
    Task<bool> UpdateSkills(Actor actor, IdPrimitive id, IEnumerable<string> skills);
    Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize);
    Task<CoachDetail?> GetCoachDetail(Actor actor, IdPrimitive id);
}

public class CoachesService(
    IAmASuperVisor Supervisor,
    IGetCoachById GetCoachById,
    IGetCoachSummaries GetCoachSummaries,
    IGetCoachDetailQuery GetCoachDetailQuery) : ICoachesService
{
    public async Task<IdPrimitive> RegisterCoach(Actor actor, string name, string email)
    {
        var coach = Coach.Create(actor, name, email);
        await Supervisor.Enlist(coach);
        await Supervisor.Ship();
        return coach.Id.Value;
    }

    public async Task<bool> UpdateSkills(Actor actor, IdPrimitive id, IEnumerable<string> skills)
    {
        var coach = await GetCoachById.One(id);
        if (coach == null) return false;
        coach.UpdateSkills(actor, skills);
        await Supervisor.Ship();
        return true;
    }

    public async Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize)
        => await GetCoachSummaries.Paged(new PageRequest(page, pageSize));

    public async Task<CoachDetail?> GetCoachDetail(Actor actor, IdPrimitive id)
        => await GetCoachDetailQuery.One(actor, id);
}