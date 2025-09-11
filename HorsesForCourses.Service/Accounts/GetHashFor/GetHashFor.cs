namespace HorsesForCourses.Service.Accounts.GetHashFor;

public interface IGetHashFor
{
    Task<string> ThisEmail(string email);
}

public class GetHashFor : IGetHashFor
{
    public Task<string> ThisEmail(string email)
    {
        throw new NotImplementedException();
    }
}