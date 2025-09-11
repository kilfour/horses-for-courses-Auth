1. Lock Coaches, do it at service level
> Anonieme gebruiker krijgt geen toegang tot de methodes van de CoachesController (route: **/Coaches**)
Making life harder at first, safer later on.
Actor.From(User.Claims) in controller, service passes actor down to domain
in method f.i. `Coach.UpdateSkills(...)`: `actor.CanUpdateSkills()`. 

really usefull test : `StringFields.AllStringsHaveLengthDefined()`

Todo : HorsesForCourses.Tests.Accounts.B_Login.C_LoginService

We could do with an *is everything resolvable* test. Not a priority, just QoL.

*Problem ?* not logged in after register
