using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Actors;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Actors;

public interface IAccountsService
{
    Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach);
}


public class AccountsService : IAccountsService
{
    private IAmASuperVisor supervisor;

    public AccountsService(IAmASuperVisor supervisor)
    {
        this.supervisor = supervisor;
    }

    public Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach)
    {
        supervisor.Enlist(ApplicationUser.Create(name, email, pass, passConfirm));
        supervisor.Ship();
        return Task.FromResult(true);
    }
}
