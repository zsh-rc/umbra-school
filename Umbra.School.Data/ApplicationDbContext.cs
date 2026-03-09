using Coravel.Pro.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Chinese;
using Umbra.School.Data.Dashboard;
using Umbra.School.Data.English;
using Umbra.School.Data.Notebook;
using Umbra.School.Data.PersonalData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Umbra.School.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options), ICoravelProDbContext
{

    public DbSet<CoravelJobHistory> Coravel_JobHistory { get; set; }
    public DbSet<CoravelScheduledJob> Coravel_ScheduledJobs { get; set; }
    public DbSet<CoravelScheduledJobHistory> Coravel_ScheduledJobHistory { get; set; }

    public DbSet<EnglishWord> EnglishWords { get; set; }
    public DbSet<EnglishPhrase> EnglishPhrases { get; set; }
    public DbSet<EnglishTranslation> EnglishTranslations { get; set; }
    public DbSet<UserEnglishWordRating> UserEnglishWordRatings { get; set; }
    public DbSet<UserEnglishPhraseRating> UserEnglishPhraseRatings { get; set; }
    public DbSet<ChineseClassicalQuestion> ChineseClassicalQuestions { get; set; }
    public DbSet<AssessmentInfo> AssessmentInfos { get; set; }
    public DbSet<AssessmentResult> AssessmentResults { get; set; }
    public DbSet<WordsAssessment> WordsAssessments { get; set; }
    public DbSet<WordsAssessmentDetail> WordsAssessmentDetails { get; set; }
    public DbSet<NotebookInfo> NotebookInfos { get; set; }

    public DbSet<ReportEnWordsCount> ReportEnWordsCounts { get; set; }
    public DbSet<ReportUserAssessment> ReportUserAssessments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the ASP.NET Identity model and override the defaults.
        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        // Rename the AspNetRoles table to 'Roles'
        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
        // Rename the AspNetUserClaims table to 'UserClaims'
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        // Rename the AspNetUserLogins table to 'UserLogins'
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        // Rename the AspNetUserRoles table to 'UserRoles'
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        // Rename the AspNetRoleClaims table to 'RoleClaims'
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        // Rename the AspNetUserTokens table to 'UserTokens'
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        // Rename the AspNetUserPasskeys table to 'UserPasskeys'
        modelBuilder.Entity<IdentityUserPasskey<string>>().ToTable("UserPasskeys");

        // Hardcode specific GUID strings for IDs to prevent them from changing
        string adminRoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210";
        string userRoleId = "3d6f185f-4c1f-557g-97bg-594e67ge8321";
        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = adminRoleId,
                Name = "Administrators",
                NormalizedName = "ADMINISTRATORS",
                Enabled = true,
                ConcurrencyStamp = "86640c2a-6056-433c-83b3-8f601b0f156d"
            },
            new ApplicationRole
            {
                Id = userRoleId,
                Name = "Users",
                NormalizedName = "USERS",
                Enabled = true,
                ConcurrencyStamp = "b1f8b88d-e6a3-4903-b097-9430c68a429b"
            }
        );

    }
}
