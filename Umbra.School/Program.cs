using ApexCharts;
using Coravel.Pro;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Umbra.School.Components;
using Umbra.School.Components.Account;
using Umbra.School.Data;
using Umbra.School.Services;
using Umbra.School.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Critical)
        .EnableSensitiveDataLogging(false)
    ); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;

        // Zach 6-Mar-26: Password Complexity Settings
        // 1. 长度限制：设置为更短的位数（默认 6）
        options.Password.RequiredLength = 6;
        // 2. 复杂度开关：全部设为 false 即可取消强制要求
        options.Password.RequireDigit = true;            // 是否需要数字
        options.Password.RequireLowercase = true;        // 是否需要小写字母
        options.Password.RequireUppercase = true;        // 是否需要大写字母
        options.Password.RequireNonAlphanumeric = false;  // 是否需要特殊字符 (@, #, $ 等)
        // 3. 唯一字符要求：设置为 1 表示不要求字符必须互不相同（默认 1）
        options.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//  Zach 15-Feb-26:
// - Layout state management service
builder.Services.AddScoped<LayoutStateService>();
// - Automapper
builder.Services.AddAutoMapper(options => { }, typeof(AutoMapperProfile));

// Zach 15-Feb-26: Register application services
builder.Services.AddScoped<IEnglishService, EnglishService>();
// Zach 17-Feb-26
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<AppSnackbarService>();
// Zach 18-Feb-26: Account service for user management
builder.Services.AddScoped<IAccountService, AccountService>();
// Zach 3-Mar-26: Dashboard service for dashboard data retrieval
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Zach 18-Feb-26
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();
builder.Services.AddScoped<UserProvider>();

// Zach 5-Mar-26: Add CoravelPro services
builder.Services.AddCoravelPro(typeof(ApplicationDbContext));
builder.Services.AddRazorPages().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Zach 2-Mar-26: To ensure database migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Zach 5-Mar-26: For CoravelPro Configurations (This must be after the database migration to ensure the CoravelPro tables are created)
app.MapRazorPages();
app.UseCoravelPro();

app.Run();
