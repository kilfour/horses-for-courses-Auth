using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Core.ValidationHelpers;

namespace HorsesForCourses.Core.Domain.Coaches;

public class Coach : DomainEntity<Coach>
{
    public CoachName Name { get; init; } = CoachName.Empty;
    public CoachEmail Email { get; init; } = CoachEmail.Empty;

    public IReadOnlyCollection<Skill> Skills => skills.ToList().AsReadOnly();
    private readonly HashSet<Skill> skills = [];

    public IReadOnlyCollection<Course> AssignedCourses => assignedCourses.AsReadOnly();
    private readonly List<Course> assignedCourses = [];

    private Coach() { /*** EFC Was Here ****/ }
    protected Coach(string name, string email)
    {
        Name = new CoachName(name);
        Email = new CoachEmail(email);
    }

    public static Coach From(Actor actor, string name, string email)
    {
        actor.IsAuthenticated();
        return new(name, email);
    }

    public virtual void UpdateSkills(Actor actor, IEnumerable<string> newSkills)
    {
        actor.IsAuthenticated();
        //OnlyActorsWhoRegisteredAsCoachCanEdit();
        NotAllowedWhenThereAreDuplicateSkills();
        OverwriteSkills();
        // ------------------------------------------------------------------------------------------------
        // --
        // bool OnlyActorsWhoRegisteredAsCoachCanEdit()
        //     => permission.CoachId != Id ? throw new UnauthorizedAccessException() : true;

        bool NotAllowedWhenThereAreDuplicateSkills()
            => newSkills.NoDuplicatesAllowed(a => new CoachAlreadyHasSkill(string.Join(",", a)));
        void OverwriteSkills()
        {
            skills.Clear();
            newSkills.Select(Skill.From)
                .ToList()
                .ForEach(a => skills.Add(a));
        }
        // ------------------------------------------------------------------------------------------------
    }

    public bool IsSuitableFor(Course course)
        => course.RequiredSkills.All(Skills.Contains);

    public bool IsAvailableFor(Course course)
        => CheckIf.ImAvailable(this).For(course);

    public void AssignCourse(Course course)
        => assignedCourses.Add(course);
}
