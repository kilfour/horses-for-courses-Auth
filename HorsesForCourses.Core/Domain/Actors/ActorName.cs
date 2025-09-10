using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Actors;

public record ActorName : DefaultString<JockeyNameCanNotBeEmpty, JockeyNameCanNotBeTooLong>
{
    public ActorName(string value) : base(value) { }
    protected ActorName() { }
    public static ActorName Empty => new();
}
