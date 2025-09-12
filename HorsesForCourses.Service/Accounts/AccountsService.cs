using System.Security.Claims;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Accounts.InvalidationReasons;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Accounts.GetApplicationUserByEmail;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Accounts;

public interface IAccountsService
{
    Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach, bool asAdmin);
    Task<IEnumerable<Claim>> Login(string coachEmail, string password);
}


public class AccountsService : IAccountsService
{
    private IAmASuperVisor supervisor;
    private readonly IGetApplicationUserByEmail getApplicationUserByEmail;

    public AccountsService(IAmASuperVisor supervisor, IGetApplicationUserByEmail getApplicationUserByEmail)
    {
        this.supervisor = supervisor;
        this.getApplicationUserByEmail = getApplicationUserByEmail;
    }

    public Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach, bool asAdmin)
    {
        var role = asAdmin ? ApplicationUser.AdminRole : asCoach ? ApplicationUser.CoachRole : string.Empty;
        supervisor.Enlist(ApplicationUser.Create(name, email, pass, passConfirm, role));
        if (role == ApplicationUser.CoachRole)
            supervisor.Enlist(Coach.Create(Actor.SystemActor(), name, email));
        supervisor.Ship();
        return Task.FromResult(true);
    }

    public async Task<IEnumerable<Claim>> Login(string email, string password)
    {
        var applicationUser = await getApplicationUserByEmail.One(email);
        if (applicationUser == null)
            throw new EmailOrPasswordAreInvalid();
        applicationUser.CheckPassword(password);
        return await Task.FromResult(applicationUser.EnterScene().ClaimIt());
    }
}
