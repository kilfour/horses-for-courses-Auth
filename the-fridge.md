### Todo: 
  * HorsesForCourses.Tests.Accounts.B_Login.C_LoginService
  * Could do with an *is everything resolvable* test. Not a priority, just QoL.
  * Implement Login after Register.  
  * *Constantinize* "owned-coach"
  * AccountsService.Register(asCoach:true, ...): very brittle
  * We need more `Act` methods in tests. The actor thing was painfull.
  * There are a lot of `It.IsAny<Actor>()`'s around.

### Observations: 
* really usefull test: 
  * `StringFields.AllStringsHaveLengthDefined()`
* Ordering of Methods in Class definitions matters.
  - AccountController: Putting the methods in order of the *Happy Path* flow is so much easier on the brain.
* Expression of Intent leads to Late Binding. Example: InvalidationReasons

### Auth
1. Lock Coaches, do it at service level
> Anonieme gebruiker krijgt geen toegang tot de methodes van de CoachesController (route: **/Coaches**)

Making life harder at first, safer later on.  
Actor.From(User.Claims) in controller, service passes actor down to domain.  
Domain makes final decision.

**Example:**  
*In domain method:* `Coach.UpdateSkills(...)`   
*Use:* `actor.CanUpdateSkills()`. 

2. Make it Dynamic
Flow:
- Register: Create ApplicationUser In Db
- Login: Get ApplicationUser => Actor => Claims
- Any Request : Get Claims => Actor

> Coach Skills kunnen enkel gewijzigd worden door de gebruiker die bij registratie deze specifieke coach heeft aangemaakt.  
> Enkel *Admin Account* heeft toegang tot `Coaches\RegisterCoach`.  
> Enkel *Admin Account* kan wijzigingen aanbrengen aan `Courses` (inclusief aanmaken, verwijderen, ...).


