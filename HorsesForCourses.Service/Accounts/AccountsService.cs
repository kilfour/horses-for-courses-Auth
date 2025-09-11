using System.Security.Claims;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Actors;

public interface IAccountsService
{
    Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach);
    Task<IEnumerable<Claim>> Login(string coachEmail, string password);
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

    public Task<IEnumerable<Claim>> Login(string email, string password)
    {
        throw new NotImplementedException();
    }
}
