using HorsesForCourses.Service.Accounts;
using HorsesForCourses.Service.Accounts.GetApplicationUserByEmail;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using Microsoft.Extensions.DependencyInjection;

namespace HorsesForCourses.Service;

public static class BuilderExtensions
{
    public static IServiceCollection AddHorsesForCourses(this IServiceCollection services)
        => services
            // === Common === 
            .AddScoped<IAmASuperVisor, DataSupervisor>()
            .AddScoped<IGetCoachById, GetCoachById>()
            // === Coaches === 
            .AddScoped<IGetCoachSummaries, GetCoachSummaries>()
            .AddScoped<IGetCoachDetailQuery, GetCoachDetailQuery>()
            .AddScoped<ICoachesService, CoachesService>()
            // === Courses === 
            .AddScoped<IGetCourseById, GetCourseById>()
            .AddScoped<IGetCourseSummaries, GetCourseSummaries>()
            .AddScoped<IGetCourseDetail, GetCourseDetail>()
            .AddScoped<ICoursesService, CoursesService>()
            // === Accounts === 
            .AddScoped<IGetApplicationUserByEmail, GetApplicationUserByEmail>()
            .AddScoped<IAccountsService, AccountsService>();
}