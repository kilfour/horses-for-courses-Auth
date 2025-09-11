using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Service.Actors;

public interface IAccountService
{
    Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach);
}


public class AccountService
{
    public Task<bool> Register(string name, string email, string pass, string passConfirm, bool asCoach)
    {
        if (pass != passConfirm)
            throw new PasswordAndPasswordConfirmDoNotMatch();
        return Task.FromResult(true);
    }
}
