namespace HorsesForCourses.Core.Domain.Actors.InvalidationReasons;

public class PasswordAndPasswordConfirmDoNotMatch : DomainException { }

public class JockeyNameCanNotBeEmpty : DomainException { }

public class JockeyNameCanNotBeTooLong : DomainException { }

public class JockeyEmailCanNotBeEmpty : DomainException { }

public class JockeyEmailCanNotBeTooLong : DomainException { }