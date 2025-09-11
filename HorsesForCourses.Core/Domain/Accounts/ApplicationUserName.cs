using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Accounts;

public record ApplicationUserName : DefaultString<ApplicationUserNameCanNotBeEmpty, ApplicationUserNameCanNotBeTooLong>
{
    public ApplicationUserName(string value) : base(value) { }
    protected ApplicationUserName() { }
    public static ApplicationUserName Empty => new();
}
