using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Actors;

public record ActorEmail : DefaultString<JockeyEmailCanNotBeEmpty, JockeyEmailCanNotBeTooLong>
{
    public ActorEmail(string value) : base(value) { }
    protected ActorEmail() { }
    public static ActorEmail Empty => new();
}
