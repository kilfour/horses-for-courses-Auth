using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Accounts;

namespace HorsesForCourses.Service.Warehouse.Accounts;

public class ApplicationUserDataConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> coach)
    {
        coach.HasKey(c => c.Id);

        var id = coach.Property(c => c.Id)
            .HasConversion(new IdValueConverter<ApplicationUser>())
            .Metadata;
        id.SetValueComparer(new IdValueComparer<ApplicationUser>());
        id.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        id.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        coach.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnType("INTEGER")
            .HasAnnotation("Sqlite:Autoincrement", true);

        coach.OwnsOne(c => c.Name, name =>
        {
            name.Property(a => a.Value)
                .IsRequired()
                .HasMaxLength(DefaultString.MaxLength);
        });
        coach.OwnsOne(c => c.Email, email =>
        {
            email.Property(a => a.Value)
                .IsRequired()
                .HasMaxLength(DefaultString.MaxLength);
        });
    }
}
