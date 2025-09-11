using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Accounts;

public record ApplicationUserEmail : DefaultString<ApplicationUserEmailCanNotBeEmpty, ApplicationUserEmailCanNotBeTooLong>
{
    public ApplicationUserEmail(string value) : base(value) { }
    protected ApplicationUserEmail() { }
    public static ApplicationUserEmail Empty => new();
}
