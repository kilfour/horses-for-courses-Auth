using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Actors;

namespace HorsesForCourses.Core.Domain.Coaches.Permissions;

public abstract record EditCoachSkills(Id<Coach> CoachId) : Capability;