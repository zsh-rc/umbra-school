using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Chinese;
using Umbra.School.Data.English;
using Umbra.School.Data.Notebook;
using Umbra.School.Data.PersonalData;

namespace Umbra.School.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<EnglishWord> EnglishWords { get; set; }
    public DbSet<EnglishPhrase> EnglishPhrases { get; set; }
    public DbSet<EnglishTranslation> EnglishTranslations { get; set; }
    public DbSet<UserEnglishWordRating> UserEnglishWordRatings { get; set; }
    public DbSet<ChineseClassicalQuestion> ChineseClassicalQuestions { get; set; }
    public DbSet<AssessmentInfo> AssessmentInfos { get; set; }
    public DbSet<AssessmentResult> AssessmentResults { get; set; }
    public DbSet<WordsAssessment> WordsAssessments { get; set; }
    public DbSet<WordsAssessmentDetail> WordsAssessmentDetails {  get; set; }
    public DbSet<NotebookInfo> NotebookInfos { get; set; }

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
    }
}
